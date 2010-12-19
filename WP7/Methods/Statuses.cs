using System;
using System.Collections.Generic;

using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter.Methods
{
    public class Statuses : RestMethodsBase<TwitterClient>
    {
        private String baseAddress = "statuses/";
        public Statuses(TwitterClient context)
            : base(context)
        {
        }
        public void BeginGetTweet(string Id, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "show/" + Id + ".json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginRetweet(string id, TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweet/" + id + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginUpdate(string text, string id, double? latitude, double? longitude, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "status", text } };

            if (!String.IsNullOrEmpty(id))
                p.Add("in_reply_to_status_id", id);

            if (latitude != null && longitude != null)
            {
                p.Add("lat", latitude.ToString());
                p.Add("long", longitude.ToString());
            }

            Context.BeginRequest(baseAddress + "update.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<Tweet>(res.Content);
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
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginHomeTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, null, null, false, false, callback);
        }

        public void BeginHomeTimeline(long count, TwitterClient.GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, count, null, false, false, callback);
        }

        public void BeginHomeTimeline(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string>();
            if (sinceId != null)
                p.Add("since_id", sinceId.ToString());

            if (maxId != null)
                p.Add("max_id", maxId.ToString());

            if (count != null)
                p.Add("count", count.ToString());

            if (page != null)
                p.Add("page", page.ToString());

            p.Add("trim_user", trimUser.ToString());
            p.Add("include_entities", includeEntities.ToString());

            Context.BeginRequest(baseAddress + "home_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginFriendsTimeline(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "friends_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginUserTimeline(String username, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "user", username } };

            Context.BeginRequest(baseAddress + "user_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginMentions(TwitterClient.GenericResponseDelegate callback)
        {
            BeginMentions(null, null, null, null, false, false, callback);
        }

        public void BeginMentions(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string>();

            if (sinceId != null)
                p.Add("since_id", sinceId.ToString());

            if (maxId != null)
                p.Add("max_id", maxId.ToString());

            if (count != null)
                p.Add("count", count.ToString());

            if (page != null)
                p.Add("page", page.ToString());

            p.Add("trim_user", trimUser.ToString());
            p.Add("include_entities", includeEntities.ToString());

            Context.BeginRequest(baseAddress + "mentions.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginRetweetedByMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_by_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginRetweetedToMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_to_me.json",
                null,
                WebMethod.Get,
                (req, res, state) => ParseResult<ResultsWrapper<Tweet>>(res, callback, req));
        }

        public void BeginRetweetedOfMe(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_of_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginGetFriends(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "friends.json",
                null,
                WebMethod.Get,
                (req, res, state) => ParseResult<ResultsWrapper<User>>(res, callback, req));
        }

        private static void ParseResult<T>(RestResponse res, TwitterClient.GenericResponseDelegate callback, RestRequest req)
            where T : ITwitterResponse
        {
            var obj = TwitterClient.Deserialise<T>(res.Content);
            if (callback != null)
                callback(req, res, obj);
        }

    }
}
