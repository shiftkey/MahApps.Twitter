using System;
using MahApps.Twitter.Extensions;
using NUnit.Framework;

namespace MahApps.Twitter.NET40.UnitTests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ParseDateTime_WithEmptyString_ThrowsException()
        {
            "".ParseDateTime();
        }

        [Test]
        public void ParseDateTime_WithTwitterDate_ReturnsValidDate()
        {
            var dateTime = "Sun Mar 18 06:42:26 +0000 2007".ParseDateTime();

            Assert.That(dateTime.DayOfWeek == DayOfWeek.Sunday);
            Assert.That(dateTime.Year == 2007);
        }
    }
}
