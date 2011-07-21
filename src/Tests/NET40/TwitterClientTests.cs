using Hammock;
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
        public void BeginStream_ForSomeSituation_DoesSomething()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void BeginStream_WithSimpleCallback_ReturnsAValue()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string callback = "http://something.com/foo";

            // act
            var client = new TwitterClient(restClient, "foo", "bar", callback);
            var result = client.BeginStream((a, b) => { }, null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void BeginStream_CallingASecondTime_ReturnsSameValue()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string callback = "http://something.com/foo";

            // act
            var client = new TwitterClient(restClient, "foo", "bar", callback);
            var result = client.BeginStream((a, b) => { }, null);
            var result2 = client.BeginStream((a, b) => { }, null);

            Assert.That(result, Is.SameAs(result2));
        }

        [Test]
        public void BeginStream_WithSimpleCallback_SetsStreamingAsyncResult()
        {
            // arrange
            var restClient = Substitute.For<IRestClient>();
            const string callback = "http://something.com/foo";

            // act
            var client = new TwitterClient(restClient, "foo", "bar", callback);
            var result = client.BeginStream((a, b) => { }, null);

            Assert.That(client.StreamingAsyncResult, Is.Not.Null);
            Assert.That(client.StreamingAsyncResult, Is.SameAs(result));
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
