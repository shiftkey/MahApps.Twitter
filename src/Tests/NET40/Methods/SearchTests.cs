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
    public class SearchTests
    {
        private IBaseTwitterClient twitterClient;
        private Search search;

        [SetUp]
        public void SetUp()
        {
            twitterClient = Substitute.For<IBaseTwitterClient>();
            search = new Search(twitterClient);
        }

        [Test]
        public void BeginGetSavedSearches_ForAllScenarios_ReturnsListOfSavedSearches()
        {
            // arrange
            var wasCalled = false;
            twitterClient.SetReponseBasedOnRequestPath();
            
            // assert
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<SavedSearch>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            search.BeginGetSavedSearches(endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginSearch_ForAllScenarios_ReturnsListOfSearchTweets()
        {
            // arrange
            var wasCalled = false;
            twitterClient.SetReponseBasedOnRequestPath();

            // assert
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<SearchTweet>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            search.BeginSearch("something", endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }


        [Test]
        public void BeginSearch_ForAllScenarios_UsesQueryTerm()
        {
            // arrange
            const string expected = "something";
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c => c.AssertParameter("q", expected));

            // act
            search.BeginSearch(expected, (a,b,c) => { });
        }
    }
}
