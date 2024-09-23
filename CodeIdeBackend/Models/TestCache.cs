using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeIdeBackend.Models
{
    public static class TestResultHelper
    {
        public static ConcurrentDictionary<Guid, Dictionary<string, string>> TestCache = new ConcurrentDictionary<Guid, Dictionary<string, string>>();
    }
}