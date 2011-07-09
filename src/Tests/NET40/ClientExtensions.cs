using System.Collections.Generic;
using System.Linq;
using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using NSubstitute;

namespace MahApps.Twitter.NET40.UnitTests
{
    public static class ClientExtensions
    {
        public  static void SetResponse(this IBaseTwitterClient twitterClient, string text)
        {
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
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

        public static void SetReponseBasedOnRequestPath(this IBaseTwitterClient twitterClient)
        {
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string,string>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
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

        public static void SetFileReponseBasedOnRequestPath(this IBaseTwitterClient twitterClient)
        {
            twitterClient.When(a => a.BeginRequest(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<IDictionary<string,File>>(), Arg.Any<WebMethod>(), Arg.Any<RestCallback>()))
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