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

        public void BeginUpdate(String Text, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("status", Text);

            Context.BeginRequest(baseAddress + "update.json", p, WebMethod.Post, (req, res, state) =>
            {
                User obj = JsonConvert.DeserializeObject<User>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginPublicTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "public_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
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
                try
                {
                    List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                    callback(req, res, obj);
                }
                catch (Exception ex)
                {

                }
                
            });
        }



        public void BeginFriendsTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "friends_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginUserTimeline(String Username, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("user", Username);

            Context.BeginRequest(baseAddress + "user_timeline_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginMentions(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "mentions.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginRetweetedByMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_by_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginRetweetedToMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_to_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginRetweetedOfMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_of_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

    }
}
