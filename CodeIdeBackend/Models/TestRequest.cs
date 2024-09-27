using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
        private readonly string dllFolderPath = ConfigurationManager.AppSettings["dllFolderPath"];
        private readonly string vsTestFile = ConfigurationManager.AppSettings["vsTestFile"];
        public string Handle(TestRequest request)
        {
            //check cache 
            if (TestResultHelper.TestCache.TryGetValue(request.UserId, out var userTestResults) && (userTestResults?.TryGetValue(request.Code, out var response) ?? false))
            {
                return response;
            }

            var provider = CodeDomProvider.CreateProvider("CSharp");

            var loParameters = new CompilerParameters();

            //Load the result assembly into memory
            loParameters.GenerateInMemory = false;

            //Add reference library
            loParameters.ReferencedAssemblies.Add("System.dll");
            loParameters.ReferencedAssemblies.Add("System.Linq.dll");
            loParameters.ReferencedAssemblies.Add(typeof(Assert).Assembly.Location);

            var outputAssembly = dllFolderPath +GetRandomFileName() +".dll";
            loParameters.OutputAssembly = outputAssembly;

            //compile code
            var compilerResults = provider.CompileAssemblyFromSource(loParameters, request.Code, UnitTestCode.TestCode);
            if (compilerResults.Errors.HasErrors)
            {
                var lcErrorMsg = compilerResults.Errors.Count.ToString() + " Errors:";
                for (int x = 0; x < compilerResults.Errors.Count; x++)
                {
                    lcErrorMsg = lcErrorMsg + "\r\nLine: " + compilerResults.Errors[x].Line.ToString() + " - " +
                                compilerResults.Errors[x].ErrorText;
                }
                throw new Exception(lcErrorMsg);
            }

            string output = "";
            string error = "";
            var processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                FileName = vsTestFile,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = compilerResults.CompiledAssembly.Location
            };

            using (var process = new Process())
            {
                process.StartInfo = processInfo;

                process.Start();
                process.WaitForExit();

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    output += line;
                }

                while (!process.StandardError.EndOfStream)
                {
                    var line = process.StandardError.ReadLine();
                    error += line;
                }

            }

            // Read the output of the process.
            Console.WriteLine(output);
            Console.WriteLine(error);

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

        private static string GetRandomFileName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}