using System.Linq;
using Hammock;
using Hammock.Web;
using NSubstitute;

namespace MahApps.Twitter.NET40.UnitTests
{
    public static class ClientExtensions
    {
        public  static void SetInvalidResponse(this IBaseTwitterClient twitterClient, string text)
        {
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                .Do(c =>
                        {
                            var request = Substitute.For<RestRequest>();
                            var response = Substitute.For<RestResponse>();
                            response.Content.Returns(text);

                            var callback = c.Args().Last() as RestCallback;
                            if (callback != null)
                                callback(request, response, null);
                        });
        }

        public static void SetValidResponse(this IBaseTwitterClient twitterClient)
        {
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), null, Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
                .Do(c =>
                        {
                            var request = Substitute.For<RestRequest>();
                            var response = Substitute.For<RestResponse>();
                            response.Content.Returns(c.MapRequestPathToTestData());

                            var callback = c.Args().Last() as RestCallback;
                            if (callback != null)
                                callback(request, response, null);
                        });
        }
    }
}