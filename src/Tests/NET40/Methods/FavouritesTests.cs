using System.Collections.Generic;
using System.Linq;
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
    public class FavouritesTests
    {
        [Test]
        public void BeginGetFavourites_ForAllScenarios_ReturnsStatus()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var directMessages = new Favourites(twitterClient);

            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<Tweet>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            directMessages.BeginGetFavourites(endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginCreate_ForAllScenarios_SetsParameter()
        {
            // arrange
            var wasReceived = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                                 {
                                     wasReceived = true;
                                     var url = c.Args().First().ToString();
                                     Assert.That(url.EndsWith("1234.json"));
                                 });
            var statuses = new Favourites(twitterClient);

            // act
            statuses.BeginCreate("1234", (a, b, c) => { });
            Assert.That(wasReceived);
        }

        [Test]
        public void BeginDestroy_ForAllScenarios_SetsParameter()
        {
            // arrange
            var wasReceived = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             wasReceived = true;
                             var url = c.Args().First().ToString();
                             Assert.That(url.EndsWith("1234.json"));
                         });
            var statuses = new Favourites(twitterClient);

            // act
            statuses.BeginDestroy("1234", (a, b, c) => { });
            Assert.That(wasReceived);
        }
    }
}
