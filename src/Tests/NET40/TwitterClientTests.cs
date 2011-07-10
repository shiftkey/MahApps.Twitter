﻿using Hammock;
using Hammock.Authentication.OAuth;
using NSubstitute;
using NUnit.Framework;

namespace MahApps.Twitter.Tests
{
    [TestFixture]
    public class TwitterClientTests
    {
        [Test]
        public void Constructor_WithMockService_SetsCredentials()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string consumerKey = "foo";
            const string consumerSecret = "bar";

            // act
            var client = new TwitterClient(restClient, consumerKey, consumerSecret, "");

            //assert
            var credentials = client.Credentials as OAuthCredentials;
            Assert.That(credentials.ConsumerKey == consumerKey);
            Assert.That(credentials.ConsumerSecret == consumerSecret);
        }

        [Test]
        public void Constructor_WithMockService_IsSetToTwittersAPI()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string consumerKey = "foo";
            const string consumerSecret = "bar";

            // act
            var client = new TwitterClient(restClient, consumerKey, consumerSecret, "");

            //assert
            Assert.That(client.OAuthBase == "https://api.twitter.com/oauth/");
            Assert.That(client.TokenRequestUrl == "request_token");
            Assert.That(client.TokenAuthUrl == "authorize");
            Assert.That(client.TokenAccessUrl == "access_token");
            Assert.That(client.Authority == "https://api.twitter.com/");
            Assert.That(client.Version == "1");
        }

        [Test]
        public void Constructor_WithCallback_IsSetToCredentials()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string callback = "http://something.com/foo";

            // act
            var client = new TwitterClient(restClient, "foo", "bar", callback);

            // assert
            var credentials = client.Credentials as OAuthCredentials;
            Assert.That(credentials.CallbackUrl == callback);
        }

        [Test]
        public void DelegatedRequest_WithParameters_SetsHeaders()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void DelegatedRequest_WithParameters_SetsCorrectPath()
        {
            Assert.Inconclusive();
        }

        public void BeginXAuthAuthenticate_WithParameters_CallsToTwitter()
        {
            Assert.Inconclusive();
        }
    }
}
