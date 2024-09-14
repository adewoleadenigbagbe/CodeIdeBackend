using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeIdeBackend.Models
{
    public static class TestCache
    {
        public static Dictionary<Guid, Dictionary<string, List<TestResult>>> Cache = new Dictionary<Guid, Dictionary<string, List<TestResult>>>(); 
    }
}