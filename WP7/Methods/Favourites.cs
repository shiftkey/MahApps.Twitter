using System;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Favourites : RestMethodsBase<TwitterClient>
    {
        private String baseAddress = "favorites/";

        public Favourites(TwitterClient Context)
            : base(Context)
        {
        }

        public void BeginCreate(String ID, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "create/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginDestroy(String ID, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "destroy/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
