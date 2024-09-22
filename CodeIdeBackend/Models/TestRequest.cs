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
        public TestResponse Handle(TestRequest request)
        {
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

            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            //this going to be the unit test argument
            startInfo.Arguments = "";

            process.Start();

            process.WaitForExit();

            // Read the output of the process.
            string output = process.StandardOutput.ReadToEnd();

            Console.WriteLine(output);

            return new TestResponse();
        }
    }
}