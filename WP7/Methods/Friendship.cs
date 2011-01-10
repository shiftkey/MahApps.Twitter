using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Friendship : RestMethodsBase<TwitterClient>
    {
        private String baseAddress = "friendships/";
        public Friendship(TwitterClient Context)
            : base(Context)
        {
        }

        public void BeginCreate(String Username, TwitterClient.GenericResponseDelegate callback)
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
        public void BeginDestroy(String Username, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest(baseAddress + "destroy.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
