using System;
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
    public class UsersTests
    {
        [Test]
        public void BeginSearch_WithValidUser_ReturnsUsers()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var users = new Users(twitterClient);

            // assert
            GenericResponseDelegate endSearch = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<User>;
                Assert.That(results, Is.Not.Empty);
            };

            // act
            users.BeginSearch("abcde", endSearch);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginSearch_WithValidUser_SetsParameter()
        {
            const string username = "abcde";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("q", username));
            var users = new Users(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            users.BeginSearch(username, endSearch);
        }

        [Test]
        public void BeginLookup_WithValidUser_ReturnsUsers()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var users = new Users(twitterClient);

            // assert
            GenericResponseDelegate endSearch = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<User>;
                Assert.That(results, Is.Not.Empty);
            };

            // act
            users.BeginLookup(new[] { 1,2 }, endSearch);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BeginLookup_WithNoIds_ThrowsException()
        {
            var twitterClient = Substitute.For<IBaseTwitterClient>();

            var users = new Users(twitterClient);

            // act
            users.BeginLookup(null, (a, b, c) => { });

            twitterClient.DidNotReceive().BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>());
        }

        [Test]
        public void BeginLookup_WithOneId_SetsParameterCorrectly()
        {
            var values = new[] { 1, 2 };
            const string expected = "1,2";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("user_id", expected));
            var users = new Users(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            users.BeginLookup(values, endSearch);
        }

        [Test]
        public void BeginLookup_WithTwoIds_SetsParameterCorrectly()
        {
            var values = new[] { 1, 2 };
            const string expected = "1,2";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("user_id", expected));
            var users = new Users(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            users.BeginLookup(values, endSearch);
        }

        [Test]
        public void BeginLookup_WithThreeIds_SetsParameterCorrectly()
        {
            var values = new[] { 1, 2, 3 };
            const string expected = "1,2,3";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("user_id", expected));
            var users = new Users(twitterClient);

            GenericResponseDelegate endSearch = (a, b, c) => { };

            // act
            users.BeginLookup(values, endSearch);
        }
    }
}
