using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Favourites : RestMethodsBase<TwitterClient>
    {
        private const string BaseAddress = "favorites/";

        public Favourites(TwitterClient context)
            : base(context)
        {
        }

        public void BeginGetFavourites(GenericResponseDelegate callback)
        {
            Context.BeginRequest("favorites.json", null, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginCreate(string id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(BaseAddress + "create/" + id + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginDestroy(string id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(BaseAddress + "destroy/" + id + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
