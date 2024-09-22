using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
            CompilerResults loCompiled = provider.CompileAssemblyFromSource(loParameters, request.Code);

            if(loCompiled.Errors.HasErrors)
            {
                string lcErrorMsg = "";

                lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                for (int x = 0; x < loCompiled.Errors.Count; x++)
                {
                    lcErrorMsg = lcErrorMsg + "\r\nLine: " + loCompiled.Errors[x].Line.ToString() + " - " +
                                loCompiled.Errors[x].ErrorText;
                }
                throw new Exception(lcErrorMsg);
            }

            return new TestResponse();
        }
    }
}