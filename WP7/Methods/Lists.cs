using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class List : RestMethodsBase<ITwitterClient>
    {
        public List(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginGetSubscriptions(string username, GenericResponseDelegate callback)
        {
            Context.BeginRequest(username + "/lists/subscriptions.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ListResult>(res.Content);

                if (callback != null)
                {
                    if (obj is ListResult)
                        callback(req, res, ((ListResult)obj).Lists);
                    else
                        callback(req, res, obj);
                }

            });
        }

        public void BeginGetUserLists(string username, GenericResponseDelegate callback)
        {
            Context.BeginRequest(username + "/lists.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ListResult>(res.Content);

                if (callback != null)
                {
                    if (obj is ListResult)
                        callback(req, res, ((ListResult)obj).Lists);
                    else
                        callback(req, res, obj);
                }
            });
        }

        public void BeginGetList(string username, string id, long? sinceId, long? maxId, long? count, int? page, bool includeEntities, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string>();
            if (sinceId != null && (long)sinceId > 0)
                p.Add("since_id", sinceId.ToString());

            if (maxId != null)
                p.Add("max_id", maxId.ToString());

            if (count != null)
                p.Add("per_page", count.ToString());

            if (page != null)
                p.Add("page", page.ToString());

            p.Add("include_entities", includeEntities.ToString());

            Context.BeginRequest(username + "/lists/"+id+"/statuses.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
