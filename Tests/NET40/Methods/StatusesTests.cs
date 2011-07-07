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
        [Test]
        public void BeginPublicTimeline_ForAnonymousUser_ReturnsAtLeastOneTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                                 {
                                     var response = Substitute.For<RestResponse>();
                                     response.Content.Returns(File.ReadAllText(MapPathToTestData(c)));

                                     var callback = c.Args().Last() as RestCallback;
                                     if (callback != null)
                                         callback(null, response, null);
                                 });

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

            Assert.That(wasCalled, "A callback needs to occur");
        }

        private static string MapPathToTestData(CallInfo c)
        {
            var path = c.Args().First() as string;
            var url = path.Replace("/", "\\");
            return @".\Data\" + url;
        }

        [Test]
        public void BeginUserTimeline_ForAnonymousUser_ReturnsAtLeastOneTweet()
        {
            var wasCalled = false;
            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var response = Substitute.For<RestResponse>();
                             response.Content.Returns(File.ReadAllText(MapPathToTestData(c)));

                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(null, response, null);
                         });

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

            Assert.That(wasCalled, "A callback needs to occur");
        }

        [Test]
        public void BeginGetTweet_WhichFindsATweet_ReturnsSuccessfulTweet()
        {
            var wasCalled = false;

            var response = Substitute.For<RestResponse>();
            response.Content.Returns(File.ReadAllText(@".\Data\statuses\show-existing.txt"));

            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(null, response, null);
                         });

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

            Assert.That(wasCalled, "A callback needs to occur");
        }

        [Test]
        public void BeginGetTweet_WhichDoesNotFindATweet_ReturnsExceptionResponse()
        {
            var wasCalled = false;
            var request = Substitute.For<RestRequest>();
            var response = Substitute.For<RestResponse>();
            response.Content.Returns(File.ReadAllText(@".\Data\statuses\show-missing.txt"));

            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(request, response, null);
                         });

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

            Assert.That(wasCalled, "A callback needs to occur");
        }
    }
}
