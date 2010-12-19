using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Block : RestMethodsBase<TwitterClient>
    {
        private const string BaseAddress = "blocks/";
        public Block(TwitterClient context)
            : base(context)
        {
        }

        public void BeginBlock(string username, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "screen_name", username } };

            Context.BeginRequest(BaseAddress + "create.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginSpamBlock(string username, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "screen_name", username } };

            Context.BeginRequest("report_spam.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
