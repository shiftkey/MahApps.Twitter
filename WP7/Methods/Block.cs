using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Block: RestMethodsBase<TwitterClient>
    {
        private String baseAddress = "block/";
        public Block(TwitterClient Context)
            : base(Context)
        {
        }

        public void BeginBlock(String Username, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest(baseAddress + "create.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginSpamBlock (String Username, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest("report_spam.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
