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
            Context.BeginRequest(baseAddress + "home_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                callback(req, res, obj);
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
