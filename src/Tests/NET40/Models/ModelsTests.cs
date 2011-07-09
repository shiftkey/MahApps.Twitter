using System;
using MahApps.Twitter.Models;
using NUnit.Framework;

namespace MahApps.Twitter.Tests.Models
{
    [TestFixture]
    public class ModelsTests
    {
        [Test]
        public void Created_ForDirectMessageUsingTestTime_IsPopulated()
        {
            // arrange
            var directMessage = new DirectMessage();
            directMessage.CreatedDate = "Sun Mar 18 06:42:26 +0000 2007";

            // act
            var dateTime = directMessage.Created;

            Assert.That(dateTime.DayOfWeek == DayOfWeek.Sunday);
            Assert.That(dateTime.Year == 2007);
        }

        [Test]
        public void Created_ForTweetUsingTestTime_IsPopulated()
        {
            // arrange
            var directMessage = new Tweet();
            directMessage.CreatedDate = "Sun Mar 18 06:42:26 +0000 2007";

            // act
            var dateTime = directMessage.Created;

            Assert.That(dateTime.DayOfWeek == DayOfWeek.Sunday);
            Assert.That(dateTime.Year == 2007);
        }

        [Test]
        public void Created_ForSearchTweetUsingTestTime_IsPopulated()
        {
            // arrange
            var directMessage = new SearchTweet();
            directMessage.CreatedDate = "Sun Mar 18 06:42:26 +0000 2007";

            // act
            var dateTime = directMessage.Created;

            Assert.That(dateTime.DayOfWeek == DayOfWeek.Sunday);
            Assert.That(dateTime.Year == 2007);
        }

        [Test]
        public void Created_ForUserUsingTestTime_IsPopulated()
        {
            // arrange
            var directMessage = new User();
            directMessage.CreatedDate = "Sun Mar 18 06:42:26 +0000 2007";

            // act
            var dateTime = directMessage.Created;

            Assert.That(dateTime.DayOfWeek == DayOfWeek.Sunday);
            Assert.That(dateTime.Year == 2007);
        }
    }
}
