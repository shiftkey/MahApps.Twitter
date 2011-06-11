using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Statuses : RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "statuses/";
        public Statuses(ITwitterClient Context)
            : base(Context)
        {
        }
        public void BeginGetTweet(String Id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "show/" + Id + ".json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<Tweet>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginRetweet(String ID, GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweet/" + ID + ".json", null, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = Context.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginUpdate(string text, string id, double? lat, double? @long, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string>();

            p.Add("status", Context.Encode ? Uri.EscapeDataString(text) : text);

            if (!String.IsNullOrEmpty(id))
                p.Add("in_reply_to_status_id", id);

            if (lat != null && @long != null)
            {
                p.Add("lat", lat.ToString());
                p.Add("long", @long.ToString());
            }

            Context.BeginRequest(baseAddress + "update.json", p, WebMethod.Post, (req, res, state) =>
            {
                //Tweet obj = JsonConvert.DeserializeObject<Tweet>(res.Content);
                ITwitterResponse obj = Context.Deserialise<Tweet>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });

        }

        public void BeginUpdate(string text, string id, GenericResponseDelegate callback)
        {
            BeginUpdate(text, id, null, null, callback);
        }


        public void BeginUpdate(string text, GenericResponseDelegate callback)
        {
            BeginUpdate(text, null, callback);
        }

        public void BeginPublicTimeline(GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "public_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
        public void BeginHomeTimeline(GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, null, null, false, false, callback);
        }

        public void BeginHomeTimeline(long Count, GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, Count, null, false, false, callback);
        }

        public void BeginHomeTimeline(long? SinceId, long? MaxId, long? Count, int? Page, bool TrimUser, bool IncludeEntities, GenericResponseDelegate callback)
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
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }



        public void BeginFriendsTimeline(GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "friends_timeline.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<Tweet> obj = JsonConvert.DeserializeObject<List<Tweet>>(res.Content);
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginUserTimeline(String Username, GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest(baseAddress + "user_timeline.json", p, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                callback(req, res, obj);
            });
        }

        public void BeginMentions(GenericResponseDelegate callback)
        {
            BeginMentions(null, null, null, null, false, false, callback);
        }

        public void BeginMentions(long? SinceId, long? MaxId, long? Count, int? Page, bool TrimUser, bool IncludeEntities, GenericResponseDelegate callback)
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
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }

        public void BeginRetweetedByMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_by_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginRetweetedToMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_to_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginRetweetedOfMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "retweeted_of_me.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<Tweet>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginDestroy(String Id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "destroy/"+Id+".json", null, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<ResultsWrapper<User>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }


    }
}
