using Hammock;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Extensions
{
    public static class ResponseExtensions
    {
        public static void ParseResult<T>(this RestResponse res, GenericResponseDelegate callback, RestRequest req)
         where T : ITwitterResponse
        {
            var obj = TwitterClient.Deserialise<T>(res.Content);
            if (callback != null)
                callback(req, res, obj);
        }
    }
}
