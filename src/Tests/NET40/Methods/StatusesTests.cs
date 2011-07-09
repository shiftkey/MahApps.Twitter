using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class StatusesTests
    {
        [Test]
        public void BeginPublicTimeline_ForAnonymousUser_ReturnsAtLeastOneTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endPublishTimeline = (a, b, c) =>
                                                             {
                                                                 wasCalled = true;
                                                                 var tweets = c as IEnumerable<Tweet>;
                                                                 Assert.That(tweets.Count(), Is.GreaterThan(0));
                                                             };

            // act
            statuses.BeginPublicTimeline(endPublishTimeline);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginUserTimeline_ForAnonymousUser_ReturnsAtLeastOneTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endUserTimeline = (a, b, c) =>
            {
                wasCalled = true;
                var tweets = c as IEnumerable<Tweet>;
                Assert.That(tweets.Count(), Is.GreaterThan(0));
            };

            // act
            statuses.BeginUserTimeline("someone", endUserTimeline);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginGetTweet_WhichFindsATweet_ReturnsSuccessfulTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetResponse(File.ReadAllText(@".\Data\statuses\show-existing.txt"));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endGetTweet = (a, b, c) =>
                                                      {
                                                          wasCalled = true;
                                                          var tweet = c as Tweet;
                                                          Assert.That(tweet, Is.Not.Null);
                                                          Assert.That(tweet.Id, Is.EqualTo(16381619317248000));
                                                      };

            // act
            statuses.BeginGetTweet("16381619317248000", endGetTweet);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginGetTweet_WhichDoesNotFindATweet_ReturnsExceptionResponse()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetResponse(File.ReadAllText(@".\Data\statuses\show-missing.txt"));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endGetTweet = (a, b, c) =>
                                                                    {
                                                                        wasCalled = true;

                                                                        var exception = c as ExceptionResponse;
                                                                        Assert.That(exception, Is.Not.Null);
                                                                    };

            // act
            statuses.BeginGetTweet("12345", endGetTweet);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginRetweet_WhichFindsATweet_ReturnsSuccessfulTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetResponse(File.ReadAllText(@".\Data\statuses\retweet-existing.txt"));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endGetTweet = (a, b, c) =>
            {
                wasCalled = true;
                var tweet = c as Tweet;
                Assert.That(tweet, Is.Not.Null);
                Assert.That(tweet.Id, Is.EqualTo(16381619317248000));
            };

            // act
            statuses.BeginRetweet("16381619317248000", endGetTweet);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginRetweet_WhichDoesNotFindATweet_ReturnsExceptionResponse()
        {
            // arrange
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetResponse(File.ReadAllText(@".\Data\statuses\retweet-missing.txt"));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endGetTweet = (a, b, c) =>
            {
                wasCalled = true;

                var exception = c as ExceptionResponse;
                Assert.That(exception, Is.Not.Null);
            };

            // act
            statuses.BeginRetweet("12345", endGetTweet);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginUpdate_WithTextOnly_ReturnsValidResult()
        {
            // act
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetResponse(File.ReadAllText(@".\Data\statuses\update-plain.json"));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endBeginUpdate = (a, b, c) =>
            {
                wasCalled = true;
                var tweet = c as Tweet;
                Assert.That(tweet, Is.Not.Null);
                Assert.That(tweet.Id, Is.EqualTo(76362320393154561));
            };

            // act
            statuses.BeginUpdate("some words go here", endBeginUpdate);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginUpdate_WithInReplyToIdSet_PopulatesParameter()
        {
            // act
            const string inReplyToId = "12345";

            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
             .Do(c => c.AssertParameter("in_reply_to_status_id", inReplyToId));
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endBeginUpdate = (a, b, c) => { };

            // act
            statuses.BeginUpdate("some words go here", "12345", endBeginUpdate);
        }

        [Test]
        public void BeginUpdate_WithLocationSet_PopulatesParameters()
        {
            // act
            const double latitute = 10.0;
            const double longitude = 15.0;

            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
             .Do(c =>
             {
                 c.AssertParameter("lat", latitute);
                 c.AssertParameter("long", longitude);
             });
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endBeginUpdate = (a, b, c) => { };

            // act
            statuses.BeginUpdate("some words go here", "12345", latitute, longitude, endBeginUpdate);
        }

       

        [Test]
        public void BeginMentions_WithDefaultParameters_WillReturnList()
        {
            // act
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.SetReponseBasedOnRequestPath();
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endGetMentions = (a, b, c) =>
            {
                wasCalled = true;
                var tweet = c as ResultsWrapper<Tweet>;
                Assert.That(tweet, Is.Not.Null);
                Assert.That(tweet, Is.Not.Empty);
            };

            // act
            statuses.BeginMentions(endGetMentions);

            Assert.That(wasCalled, Errors.CallbackDidNotFire);
        }

        [Test]
        public void BeginMentions_WithDefaultParameters_HasParametersSet()
        {
            // act
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
             .Do(c =>
             {
                 c.AssertParameter("trim_user", false);
                 c.AssertParameter("include_entities", false);
             });
            var statuses = new Statuses(twitterClient);

            // act
            statuses.BeginMentions((a, b, c) => { });
        }

        [Test]
        public void BeginMentions_WithParameters_HasParametersSet()
        {
            // arrange
            const int sinceId = 100;
            const int maxId = 200;
            const int count = 20;
            const int page = 1;
            const bool trimUser = true;
            const bool includeEntities = true;
            
            // act
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
             .Do(c =>
             {
                 c.AssertParameter("since_id", sinceId);
                 c.AssertParameter("max_id", maxId);
                 c.AssertParameter("count", count);
                 c.AssertParameter("page", page);
                 c.AssertParameter("trim_user", trimUser);
                 c.AssertParameter("include_entities", includeEntities);
             });
            var statuses = new Statuses(twitterClient);

            // act
            statuses.BeginMentions(sinceId, maxId, count, page, trimUser, includeEntities, (a, b, c) => { });
        }
    }
}
