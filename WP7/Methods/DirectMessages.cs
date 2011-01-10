using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter.Methods
{
    public class DirectMessages : RestMethodsBase<TwitterClient>
    {
        public DirectMessages(TwitterClient Context)
            : base(Context)
        {
        }

        private String baseAddress = "direct_messages";

        public void BeginDirectMessages(TwitterClient.GenericResponseDelegate callback)
        {
            BeginDirectMessages(null, null, null, null, false, false, callback);
        }

        public void BeginDirectMessages(long? SinceId, long? MaxId, long? Count, int? Page, bool TrimUser, bool IncludeEntities, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            if (SinceId != null)
                p.Add("since_id", SinceId.ToString());

            if (MaxId != null)
                p.Add("max_id", MaxId.ToString());

            if (Count != null)
                p.Add("count", Count.ToString());

            if (Page != null)
                p.Add("page", Page.ToString());

            p.Add("trim_user", TrimUser.ToString());
            p.Add("include_entities", IncludeEntities.ToString());

            Context.BeginRequest(baseAddress + ".json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginSentDirectMessages(TwitterClient.GenericResponseDelegate callback)
        {
            BeginSentDirectMessages(null, null, null, null, false, false, callback);
        }

        public void BeginSentDirectMessages(long? SinceId, long? MaxId, long? Count, int? Page, bool TrimUser, bool IncludeEntities, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            if (SinceId != null)
                p.Add("since_id", SinceId.ToString());

            if (MaxId != null)
                p.Add("max_id", MaxId.ToString());

            if (Count != null)
                p.Add("count", Count.ToString());

            if (Page != null)
                p.Add("page", Page.ToString());

            p.Add("trim_user", TrimUser.ToString());
            p.Add("include_entities", IncludeEntities.ToString());

            Context.BeginRequest(baseAddress + "/sent.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginCreate(String screen_name, String Text, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", screen_name);
            p.Add("text", Uri.EscapeDataString(Text));

            Context.BeginRequest(baseAddress + "/new.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<DirectMessage>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }
    }
}
