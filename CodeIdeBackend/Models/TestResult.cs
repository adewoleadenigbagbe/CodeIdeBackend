using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeIdeBackend.Models
{
    public class TestResult
    {
        public TestStatus Status { get; set; }

        public string Message { get; set; }
    }

    public enum TestStatus : byte
    {
        Pending,
        Success,
        Failure
    }

}