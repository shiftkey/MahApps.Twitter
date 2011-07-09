using System.Collections.Generic;
using Hammock;
using Hammock.Web;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using NSubstitute;
using NUnit.Framework;

namespace MahApps.Twitter.NET40.UnitTests.Methods
{
    [TestFixture]
    public class BlockTests
    {
        [Test]
        public void BeginBlock_WithSomeName_ReturnsUser()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var statuses = new Block(twitterClient);

            // assert
            GenericResponseDelegate endBlock = (a, b, c) =>
            {
                wasCalled = true;
                var user = c as User;
                Assert.That(user, Is.Not.Null);
            };

            // act
            statuses.BeginBlock("abcde", endBlock);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginBlock_WithSomeName_ProvidesParameter()
        {
            // arrange
            string username = "abcde";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("screen_name", username));
            var statuses = new Block(twitterClient);

            // act
            statuses.BeginBlock(username, (a, b, c) => { });
        }

        [Test]
        public void BeginSpamBlock_WithSomeName_ReturnsUser()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var statuses = new Block(twitterClient);

            // assert
            GenericResponseDelegate endBlock = (a, b, c) =>
            {
                wasCalled = true;
                var user = c as User;
                Assert.That(user, Is.Not.Null);
            };

            // act
            statuses.BeginSpamBlock("abcde", endBlock);
            
            // assert
            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginSpamBlock_WithSomeName_ProvidesParameter()
        {
            // arrange
            string username = "abcde";
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("screen_name", username));
            var statuses = new Block(twitterClient);

            // act
            statuses.BeginSpamBlock(username, (a,b,c)=> { });
            
        }
    }
}
