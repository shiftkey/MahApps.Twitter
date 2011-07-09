using System.Collections.Generic;
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
        [Test]
        public void BeginGetSavedSearches_ForAllScenarios_ReturnsListOfSavedSearches()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var directMessages = new Search(twitterClient);

            // assert
            GenericResponseDelegate endCreate = (a, b, c) =>
            {
                wasCalled = true;
                var results = c as IEnumerable<SavedSearch>;
                Assert.That(results, Is.Not.Null);
                Assert.That(results, Is.Not.Empty);
            };

            // act
            directMessages.BeginGetSavedSearches(endCreate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }
    }
}
