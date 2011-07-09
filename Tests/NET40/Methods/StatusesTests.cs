using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hammock;
using Hammock.Web;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace MahApps.Twitter.NET40.UnitTests.Methods
{
    [TestFixture]
    public class StatusesTests
    {
        private const string CallbackDidNotFire = "A callback was expected, but did not fire";

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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
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

            Assert.That(wasCalled, CallbackDidNotFire);
        }

        [Test]
        public void BeginUpdate_WithInReplyToIdSet_PopulatesParameter()
        {
            // act
            const string inReplyToId = "12345";

            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
             .Do(c => AssertParameter(c, "in_reply_to_status_id", inReplyToId));
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
                 AssertParameter(c, "lat", latitute);
                 AssertParameter(c, "long", longitude);
             });
            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endBeginUpdate = (a, b, c) => { };

            // act
            statuses.BeginUpdate("some words go here", "12345", latitute, longitude, endBeginUpdate);
        }

        private static void AssertParameter(CallInfo c, string key, object value)
        {
            var parameters = c.Args()[1] as IDictionary<string, string>;
            if (!parameters.ContainsKey(key))
            {
                Assert.Fail("Expected key '{0}' but was not found", "in_reply_to_status_id");
            }

            var actual = parameters[key];
            if (actual != value.ToString())
            {
                Assert.Fail("Expected value '{0}' but got '{1}'", value, actual);
            }
        }
    }
}
