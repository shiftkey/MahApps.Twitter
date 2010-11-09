using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class List : RestMethodsBase<TwitterClient>
    {
        public List(TwitterClient Context)
            : base(Context)
        {
        }

        public void BeginGetSubscriptions(string Username, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(Username + "/lists/subscriptions.json", null, WebMethod.Get, (req, res, state) =>
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

        public void BeginGetUserLists(string Username, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(Username + "/lists.json", null, WebMethod.Get, (req, res, state) =>
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

        public void BeginGetList(string Username, string Id, long? SinceId, long? MaxId, long? Count, int? Page, bool IncludeEntities, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            if (SinceId != null && (long)SinceId > 0)
                p.Add("since_id", SinceId.ToString());

            if (MaxId != null)
                p.Add("max_id", MaxId.ToString());

            if (Count != null)
                p.Add("per_page", Count.ToString());

            if (Page != null)
                p.Add("page", Page.ToString());

            p.Add("include_entities", IncludeEntities.ToString());

            Context.BeginRequest(Username + "/lists/"+Id+"/statuses.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
