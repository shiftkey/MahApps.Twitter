using System.Collections.Generic;
using Hammock;
using Hammock.Web;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using NSubstitute;
using NUnit.Framework;
using List = MahApps.Twitter.Methods.List;

namespace MahApps.Twitter.Tests.Methods
{
    [TestFixture]
    public class ListTests
    {
        private IBaseTwitterClient twitterClient;
        private List lists;

        [SetUp]
        public void SetUp()
        {
            twitterClient = Substitute.For<IBaseTwitterClient>();
            lists = new List(twitterClient);
        }

        [Test]
        public void BeginGetAll_ForSomeUser_ReturnsListOfSubscriptions()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();
            var wasCalled = false;
            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<TwitterList>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };
            
            // act
            lists.BeginGetAll("myself", callback);

            Assert.That(wasCalled);
        }

        [Test]
        public void BeginGetAll_WithNullCallback_DoesNotThrowException()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();
            
            // act
            lists.BeginGetAll("myself", null);
        }

        [Test]
        public void BeginGetAll_ForSomeUser_SetsParameter()
        {
            // arrange
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                      .Do(c => c.AssertParameter("screen_name", "myself"));
            // act
            lists.BeginGetAll("myself", (a,b,c)=> { });
        }

        [Test]
        public void BeginGetAll_WithNoUser_ReturnsListOfSubscriptions()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();
            var wasCalled = false;
            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<TwitterList>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            lists.BeginGetAll(callback);

            Assert.That(wasCalled);
        }

        [Test]
        public void BeginGetAll_WithNoUser_DoesNotThrowException()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();

            // act
            lists.BeginGetAll(null);
        }

        [Test]
        public void BeginGetAll_WithNoUser_DoesNotSetsParameter()
        {
            // arrange
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                      .Do(c => c.AssertNotSet("screen_name"));
            // act
            lists.BeginGetAll((a, b, c) => { });
        }

        [Test]
        public void BeginGetList_UsingListId_ReturnsListOfUpdates()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();
            var wasCalled = false;
            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<Tweet>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            lists.BeginGetList(123, callback);

            Assert.That(wasCalled);
        }

        [Test]
        public void BeginGetList_UsingListId_DoesNotSetsParameter()
        {
            // arrange
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                      .Do(c => c.AssertParameter("list_id", 123));
            // act
            lists.BeginGetList(123, (a, b, c) => { });
        }

        [Test]
        public void BeginGetList_UsingScreenName_ReturnsListOfUpdates()
        {
            // arrange
            twitterClient.SetReponseBasedOnRequestPath();
            var wasCalled = false;
            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<Tweet>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            lists.BeginGetList("list", "someone", callback);

            Assert.That(wasCalled);
        }

        [Test]
        public void BeginGetList_UsingScreenName_DoesSetParameters()
        {
            // arrange
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                .Do(c =>
                        {
                            c.AssertParameter("slug", "myslug");
                            c.AssertParameter("owner_screen_name", "owner");
                        });
            // act
            lists.BeginGetList("myslug", "owner", (a, b, c) => { });}
    }
}
