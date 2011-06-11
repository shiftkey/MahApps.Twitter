using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class FriendsAndFollowers : RestMethodsBase<ITwitterClient>
    {
        public FriendsAndFollowers(ITwitterClient context)
            : base(context)
        {

        }
        public void BeginLookupFollowersIDs(string screenName, GenericResponseDelegate callback)
        {
            BeginLookupIDs(screenName, "followers", callback);
        }
        public void BeginLookupFriendsIDs(string screenName, GenericResponseDelegate callback)
        {
            BeginLookupIDs(screenName, "friends", callback);
        }

        private void BeginLookupIDs(string screenName, string addr, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> 
            { 
                { "screen_name", screenName }, 
                {"cursor", "-1"}
            };

            Context.BeginRequest(addr + "/ids.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = Context.Deserialise<IdLookup>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
