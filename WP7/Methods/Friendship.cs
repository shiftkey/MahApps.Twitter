using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Friendship : RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "friendship/";
        public Friendship(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginCreate(string username, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "screen_name", username } };

            Context.BeginRequest(baseAddress + "create.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
        public void BeginDestroy(string username, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string>();
            p.Add("screen_name", username);

            Context.BeginRequest(baseAddress + "destroy.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
