using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute.Core;
using NUnit.Framework;

namespace MahApps.Twitter.Tests
{
    public static class CallInfoExtensions
    {
        public static string MapRequestPathToTestData(this CallInfo c)
        {
            var path = c.Args().First() as string;
            return GetTestData(path);
        }

        public static string GetTestData(string path)
        {
            var url = path.Replace("/", "\\");
            var fileName = @".\Data\" + url;

            return File.ReadAllText(fileName);
        }

        public static void AssertParameter(this CallInfo c, string key, object value)
        {
            var parameters = c.Args()[1] as IDictionary<string, string>;
            if (!parameters.ContainsKey(key))
            {
                Assert.Fail("Expected key '{0}' but was not found", key);
            }

            var actual = parameters[key];
            if (actual != value.ToString())
            {
                Assert.Fail("Expected value '{0}' but got '{1}'", value, actual);
            }
        }
    }
}
