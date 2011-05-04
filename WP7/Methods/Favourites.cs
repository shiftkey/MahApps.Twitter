using System;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Favourites : RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "favorites/";

        public Favourites(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginGetFavourites(GenericResponseDelegate callback)
        {
            Context.BeginRequest("favorites.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginCreate(String ID, GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "create/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginDestroy(String ID, GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "destroy/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
