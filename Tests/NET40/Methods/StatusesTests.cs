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
        public void BeginPublicTimeline()
        {
            var request = Substitute.For<RestRequest>();
            var response = Substitute.For<RestResponse>();
            response.Content.Returns( File.ReadAllText(@".\Data\statuses\public_timeline.txt"));

            var twitterClient = Substitute.For<ITwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(request, response, null);
                         });

            var statuses = new Statuses(twitterClient);

            // assert
            GenericResponseDelegate endPublishTimeline = (a, b, c) =>
            {
                var tweets = c as IEnumerable<Tweet>;
                Assert.That(tweets.Count(), Is.GreaterThan(0));
            };

            // act
            statuses.BeginPublicTimeline(endPublishTimeline);
        }
    }
}
