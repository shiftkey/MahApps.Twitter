using MahApps.Twitter.Delegates;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using NSubstitute;
using NUnit.Framework;

namespace MahApps.Twitter.Tests.Methods
{
    [TestFixture]
    public class FriendshipTests
    {
        [Test]
        public void BeginCreate_WithValidUserName_ReturnsValidUser()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var friendships = new Friendship(twitterClient);

            // assert
            GenericResponseDelegate endSearch = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as User;
                Assert.That(results, Is.Not.Null);
            };

            // act
            friendships.BeginCreate("abcde", endSearch);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }
    }
}
