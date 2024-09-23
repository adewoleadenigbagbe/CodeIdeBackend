using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CodeIdeBackend.Models
{
    public class TestRequest
    {
        public Guid UserId { get; set; }

        public string Code { get; set; }
    }


    public class TestResponse
    {
        public List<TestResult> TestResults { get; set; }
    }


    public class Handler
    {
        public string Handle(TestRequest request)
        {
            //check cache 
            if (TestResultHelper.TestCache.TryGetValue(request.UserId, out var userTestResults) && (userTestResults?.TryGetValue(request.Code, out var response) ?? false))
            {
                return response;
            }

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var loParameters = new CompilerParameters();

            //Add reference library
            loParameters.ReferencedAssemblies.Add("System.dll");

            //Load the result assembly into memory
            loParameters.GenerateInMemory = true;

            //compile the code
            var userCodeCompiled = provider.CompileAssemblyFromSource(loParameters, request.Code);

            if(userCodeCompiled.Errors.HasErrors)
            {
                string lcErrorMsg = "";

                lcErrorMsg = userCodeCompiled.Errors.Count.ToString() + " Errors:";
                for (int x = 0; x < userCodeCompiled.Errors.Count; x++)
                {
                    lcErrorMsg = lcErrorMsg + "\r\nLine: " + userCodeCompiled.Errors[x].Line.ToString() + " - " +
                                userCodeCompiled.Errors[x].ErrorText;
                }
                throw new Exception(lcErrorMsg);
            }


            //Add reference library
            var loParameters2 = new CompilerParameters();

            //Add reference library
            loParameters2.ReferencedAssemblies.Add("System.dll");

            loParameters2.GenerateInMemory = true;
            var testCompiled = provider.CompileAssemblyFromSource(loParameters2, UnitTestCode.TestCode);


            if (testCompiled.Errors.HasErrors)
            {
                string lcErrorMsg = "";

                lcErrorMsg = testCompiled.Errors.Count.ToString() + " Errors:";
                for (int x = 0; x < testCompiled.Errors.Count; x++)
                {
                    lcErrorMsg = lcErrorMsg + "\r\nLine: " + testCompiled.Errors[x].Line.ToString() + " - " +
                                testCompiled.Errors[x].ErrorText;
                }
                throw new Exception(lcErrorMsg);
            }

            string output = string.Empty;
            using (var process = new Process())
            {
                var startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                startInfo.FileName = "vstest.console";

                //this going to be the unit test argument
                startInfo.Arguments = testCompiled.CompiledAssembly.GetName().Name;

                process.Start();

                process.WaitForExit();

                output = process.StandardOutput.ReadToEnd();
            }

            // Read the output of the process.
            Console.WriteLine(output);

            if (TestResultHelper.TestCache.ContainsKey(request.UserId))
            {
                var testResults = TestResultHelper.TestCache[request.UserId];
                testResults.Add(request.Code, output);
            }
            else
            {
                TestResultHelper.TestCache[request.UserId] = new Dictionary<string, string>
                {
                    [request.Code] = output
                };
            }
            return output;
        }
    }
}