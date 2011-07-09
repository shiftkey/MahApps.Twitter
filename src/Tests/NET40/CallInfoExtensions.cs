using System.IO;
using System.Linq;
using NSubstitute.Core;

namespace MahApps.Twitter.NET40.UnitTests
{
    public static class CallInfoExtensions
    {
        public static string MapRequestPathToTestData(this CallInfo c)
        {
            var path = c.Args().First() as string;
            var url = path.Replace("/", "\\");
            var fileName = @".\Data\" + url;

            return File.ReadAllText(fileName);
        }
    }
}
