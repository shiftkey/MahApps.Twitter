using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter.Methods
{
    public class Statuses : RestMethodsBase<TwitterClient>
    {
        private String baseAddress = "statuses/";
        public Statuses(TwitterClient Context)
            : base(Context)
        {
        }
        public void BeginGetTweet(String Id, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "show/" +Id + ".json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginRetweet(String ID, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweet/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginUpdate(String Text, String ID, double? Lat, double? Long, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("status", Text);

            if (!String.IsNullOrEmpty(ID))
                p.Add("in_reply_to_status_id", ID);

            if (Lat != null && Long != null)
            {
                p.Add("lat", Lat.ToString());
                p.Add("long", Long.ToString());
            }

            Context.BeginRequest(baseAddress + "update.json", p, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });

        }

        public void BeginUpdate(String Text, String ID, TwitterClient.GenericResponseDelegate callback)
        {
            BeginUpdate(Text, ID, null, null, callback);
        }


        public void BeginUpdate(String Text, TwitterClient.GenericResponseDelegate callback)
        {
            BeginUpdate(Text, null, callback);
        }

        public void BeginPublicTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "public_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
        public void BeginHomeTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, null, null, false, false, callback);
        }

        public void BeginHomeTimeline(Int64 Count, TwitterClient.GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, Count, null, false, false, callback);
        }

        public void BeginHomeTimeline(Int64? SinceId, Int64? MaxId, Int64? Count, int? Page, bool TrimUser, bool IncludeEntities, TwitterClient.GenericResponseDelegate callback)
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

            Context.BeginRequest(baseAddress + "home_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }



        public void BeginFriendsTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "friends_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginUserTimeline(String Username, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("user", Username);

            Context.BeginRequest(baseAddress + "user_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginMentions(TwitterClient.GenericResponseDelegate callback)
        {
            BeginMentions(null, null, null, null, false, false, callback);
        }

        public void BeginMentions(Int64? SinceId, Int64? MaxId, Int64? Count, int? Page, bool TrimUser, bool IncludeEntities, TwitterClient.GenericResponseDelegate callback)
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

            Context.BeginRequest(baseAddress + "mentions.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginRetweetedByMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_by_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginRetweetedToMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_to_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginRetweetedOfMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_of_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

    }
}
