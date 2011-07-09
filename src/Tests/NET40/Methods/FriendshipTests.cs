using System.Collections.Generic;
using Hammock;
using Hammock.Web;
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
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as User;
                Assert.That(results, Is.Not.Null);
            };

            // act
            friendships.BeginCreate("abcde", endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginCreate_WithValidUser_SetsParameter()
        {
            const string username = "abcde";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("screen_name", username));
            var friendships = new Friendship(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            friendships.BeginCreate(username, endSearch);
        }

        [Test]
        public void BeginDestroy_WithValidUserName_ReturnsValidUser()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var friendships = new Friendship(twitterClient);

            // assert
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as User;
                Assert.That(results, Is.Not.Null);
            };

            // act
            friendships.BeginDestroy("abcde", endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginDestroy_WithValidUser_SetsParameter()
        {
            const string username = "abcde";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("screen_name", username));
            var friendships = new Friendship(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            friendships.BeginDestroy(username, endSearch);
        }
    }
}
