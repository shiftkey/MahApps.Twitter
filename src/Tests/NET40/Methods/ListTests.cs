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
        public void BeginGetAll_ForSomeUser_SetsParameter()
        {
            // arrange
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                      .Do(c => c.AssertParameter("screen_name", "myself"));
            // act
            lists.BeginGetAll("myself", (a,b,c)=> { });
        }
    }
}
