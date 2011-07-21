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
        private IBaseTwitterClient twitterClient;
        private Favourites favourites;

        [SetUp]
        public void SetUp()
        {
            twitterClient = Substitute.For<IBaseTwitterClient>();
            favourites = new Favourites(twitterClient);
        }

        [Test]
        public void BeginGetFavourites_ForAllScenarios_ReturnsStatus()
        {
            // arrange
            var wasCalled = false;
            twitterClient.SetReponseBasedOnRequestPath();

            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<Tweet>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            favourites.BeginGetFavourites(callback);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginCreate_ForAllScenarios_SetsParameter()
        {
            // arrange
            var wasReceived = false;
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                                 {
                                     wasReceived = true;
                                     var url = c.Args().First().ToString();
                                     Assert.That(url.EndsWith("1234.json"));
                                 });
            
            // act
            favourites.BeginCreate("1234", (a, b, c) => { });

            Assert.That(wasReceived);
        }

        [Test]
        public void BeginCreate_ForAllScenarios_ReturnsAValidTweet()
        {
            // arrange
            var wasCalled = false;
            twitterClient.SetReponseBasedOnRequestPath();

            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as Tweet;
                Assert.That(results, Is.Not.Null);
            };

            // act
            favourites.BeginCreate("1234", callback);

            Assert.That(wasCalled);
        }

        [Test]
        public void BeginDestroy_ForAllScenarios_SetsParameter()
        {
            // arrange
            var wasReceived = false;
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             wasReceived = true;
                             var url = c.Args().First().ToString();
                             Assert.That(url.EndsWith("1234.json"));
                         });
            
            // act
            favourites.BeginDestroy("1234", (a, b, c) => { });

            Assert.That(wasReceived);
        }

        [Test]
        public void BeginDestroy_ForAllScenarios_ReturnsAValidTweet()
        {
            // arrange
            var wasCalled = false;
            twitterClient.SetReponseBasedOnRequestPath();

            GenericResponseDelegate callback = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as Tweet;
                Assert.That(results, Is.Not.Null);
            };

            // act
            favourites.BeginDestroy("1234", callback);

            Assert.That(wasCalled);
        }
    }
}
