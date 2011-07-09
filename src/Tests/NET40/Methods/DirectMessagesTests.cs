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
    public class DirectMessagesTests
    {
        [Test]
        public void BeginDirectMessages_ForAllScenarios_ReturnsListOfMessages()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var directMessages = new DirectMessages(twitterClient);

            // assert
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<DirectMessage>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            directMessages.BeginDirectMessages(endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginDirectMessages_ForAllScenarios_SetsParameter()
        {
            // arrange
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             c.AssertParameter("trim_user", false);
                             c.AssertParameter("include_entities", false);
                         });
            var statuses = new DirectMessages(twitterClient);

            // act
            statuses.BeginDirectMessages((a, b, c) => { });
        }

        [Test]
        public void BeginDirectMessages_WithCustomParameters_SetsAllParameters()
        {
            // arrange
            const int sinceId = 1234;
            const int maxId = 5678;
            const int count = 10;
            const int page = 1;
            const bool trimUser = true;
            const bool includeEntities = true;

            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             c.AssertParameter("trim_user", trimUser);
                             c.AssertParameter("include_entities", includeEntities);
                             c.AssertParameter("since_id", sinceId);
                             c.AssertParameter("max_id", maxId);
                             c.AssertParameter("count", count);
                             c.AssertParameter("page", page);
                         });
            var statuses = new DirectMessages(twitterClient);

            // act
            statuses.BeginDirectMessages(sinceId, maxId, count, page, trimUser, includeEntities, (a, b, c) => { });
        }

        [Test]
        public void BeginSentDirectMessages_ForAllScenarios_ReturnsListOfMessages()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var directMessages = new DirectMessages(twitterClient);

            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<DirectMessage>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            directMessages.BeginSentDirectMessages(endCreate);

            // assert
            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginSentDirectMessages_ForAllScenarios_SetsParameter()
        {
            // act
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             c.AssertParameter("trim_user", false);
                             c.AssertParameter("include_entities", false);
                         });
            var statuses = new DirectMessages(twitterClient);

            // act
            statuses.BeginSentDirectMessages((a, b, c) => { });
        }

        [Test]
        public void BeginSentDirectMessages_WithCustomParameters_SetsAllParameters()
        {
            // arrange
            const int sinceId = 1234;
            const int maxId = 5678;
            const int count = 10;
            const int page = 1;
            const bool trimUser = true;
            const bool includeEntities = true;

            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             c.AssertParameter("trim_user", trimUser);
                             c.AssertParameter("include_entities", includeEntities);
                             c.AssertParameter("since_id", sinceId);
                             c.AssertParameter("max_id", maxId);
                             c.AssertParameter("count", count);
                             c.AssertParameter("page", page);
                         });
            var statuses = new DirectMessages(twitterClient);

            // act
            statuses.BeginSentDirectMessages(sinceId, maxId, count, page, trimUser, includeEntities, (a, b, c) => { });
        }

        [Test]
        public void BeginCreate_ForAllScenarios_ReturnsNewMessage()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var directMessages = new DirectMessages(twitterClient);

            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as DirectMessage;
                Assert.That(results, Is.Not.Null);
            };

            // act
            directMessages.BeginCreate("abcde", "foo", endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginCreate_ForAllScenarios_SetsParameter()
        {
            // arrange
            var screenName = "abcde";
            var text = "defgh";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             c.AssertParameter("screen_name", screenName);
                             c.AssertParameter("text", text);
                         });
            var statuses = new DirectMessages(twitterClient);

            // act
            statuses.BeginCreate(screenName, text, (a, b, c) => { });
        }
    }
}
