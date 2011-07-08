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

            var twitterClient = Substitute.For<IBaseTwitterClient>();

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

            var twitterClient = Substitute.For<IBaseTwitterClient>();

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

        [Test]
        public void BeginVerifyCredentials_WithNoCallback_DoesNotThrowException()
        {
            var request = Substitute.For<RestRequest>();
            var response = Substitute.For<RestResponse>();
            response.Content.Returns(" { screen_name: \"shiftkey\" } ");

            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(request, response, null);
                         });

            var account = new Account(twitterClient);

            // act
            account.BeginVerifyCredentials(null);
        }

        [Test]
        public void BeginUpdateProfileImage_WithValidResponseFromClient_ContainsUser()
        {
            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<IDictionary<string, RESTBase.File>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var response = Substitute.For<RestResponse>();
                             response.Content.Returns(c.MapRequestPathToTestData());

                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(null, response, null);
                         });

            var account = new Account(twitterClient);

            // assert
            GenericResponseDelegate endUpdateProfileImage = (a, b, c) =>
            {
                var user = c as User;
                Assert.That(user, Is.Not.Null);
                Assert.That(user.ScreenName, Is.EqualTo("shiftkey"));
            };

            var f = new FileInfo("akihabara.png");

            // act
            account.BeginUpdateProfileImage(f, endUpdateProfileImage);
        }

        [Test]
        public void BeginUpdateProfileImage_WithNoCallback_DoesNotThrowException()
        {
            // arrange
            var twitterClient = Substitute.For<IBaseTwitterClient>();

            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<IDictionary<string, RESTBase.File>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                         {
                             var response = Substitute.For<RestResponse>();
                             response.Content.Returns(c.MapRequestPathToTestData());

                             var callback = c.Args().Last() as RestCallback;
                             if (callback != null)
                                 callback(null, response, null);
                         });

            var account = new Account(twitterClient);
            var f = new FileInfo("akihabara.png");

            // act
            account.BeginUpdateProfileImage(f, null);
        }

        [Test]
        public void BeginUpdateProfileImage_WithValidFile_ContainsFile()
        {
            // arrange
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<IDictionary<string, RESTBase.File>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                         .Do(c =>
                               {
                                   var files = c.Args()[2] as IDictionary<string, RESTBase.File>;
                                   Assert.That(files.Count() == 1);
                               });

            var account = new Account(twitterClient);
            var f = new FileInfo("akihabara.png");

            // act
            account.BeginUpdateProfileImage(f, null);
        }

        [Test]
        public void BeginUpdateProfileImage_WithInvalidFile_DoesNotSend()
        {
            // arrange
            var twitterClient = Substitute.For<IBaseTwitterClient>();
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<IDictionary<string, RESTBase.File>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                       .Do(c =>
                       {
                           var files = c.Args()[2] as IDictionary<string, RESTBase.File>;
                           Assert.That(files.Count() == 0);
                       });

            var account = new Account(twitterClient);
            var f = new FileInfo("fakefile.foo");

            // act
            account.BeginUpdateProfileImage(f, null);
        }
    }
}
