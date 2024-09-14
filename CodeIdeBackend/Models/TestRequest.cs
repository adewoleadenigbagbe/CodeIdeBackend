using System;
using System.Collections.Generic;
using System.Linq;
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
            return new TestResponse();
        }
    }
}