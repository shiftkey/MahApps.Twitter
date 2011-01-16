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

    // TODO: can we unit test the OAuth bits?
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void BeginVerifyCredentials_WithInvalidResponseFromClient_DoesNotContainUser()
        {
            var request = Substitute.For<RestRequest>();
            var response = Substitute.For<RestResponse>();
            response.Content.Returns("foo");

            var twitterClient = Substitute.For<ITwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                                    {
                                        var callback = c.Args().Last() as RestCallback;
                                        if (callback != null)
                                            callback(request, response, null);
                                    });

            var account = new Account(twitterClient);

            // assert
            GenericResponseDelegate endVerifyCredentials = (a, b, c) =>
            {
                var user = c as User;
                Assert.That(user, Is.Null);
            };

            // act
            account.BeginVerifyCredentials(endVerifyCredentials);
        }

        [Test]
        public void BeginVerifyCredentials_WithValidResponseFromClient_ContainsUser()
        {
            var request = Substitute.For<RestRequest>();
            var response = Substitute.For<RestResponse>();
            response.Content.Returns(" { screen_name: \"shiftkey\" } ");

            var twitterClient = Substitute.For<ITwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(request, response, null);
                         });

            var account = new Account(twitterClient);

            // assert
            GenericResponseDelegate endVerifyCredentials = (a, b, c) =>
            {
                var user = c as User;
                Assert.That(user, Is.Not.Null);
                Assert.That(user.ScreenName, Is.EqualTo("shiftkey"));
            };

            // act
            account.BeginVerifyCredentials(endVerifyCredentials);
        }
        
    }
}
