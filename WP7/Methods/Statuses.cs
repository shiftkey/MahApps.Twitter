using System;
using System.Collections.Generic;

using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Extensions;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Statuses : RestMethodsBase<ITwitterClient>
    {
        private const string BaseAddress = "statuses/";

        public Statuses(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginGetTweet(string id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                string.Format("{0}show/{1}.json", BaseAddress, id),
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginRetweet(string id, GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                string.Format("{0}retweet/{1}.json", BaseAddress, id),
                null,
                WebMethod.Post,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginUpdate(string text, string id, double? latitude, double? longitude, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "status", text } };

            if (!string.IsNullOrEmpty(id))
                p.Add("in_reply_to_status_id", id);

            if (latitude != null && longitude != null)
            {
                p.Add("lat", latitude.ToString());
                p.Add("long", longitude.ToString());
            }

            Context.BeginRequest(
                BaseAddress + "update.json",
                p,
                WebMethod.Post,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
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
            Context.BeginRequest(
                BaseAddress + "public_timeline.json",
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginHomeTimeline(GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, null, null, false, false, callback);
        }

        public void BeginHomeTimeline(long count, GenericResponseDelegate callback)
        {
            BeginHomeTimeline(null, null, count, null, false, false, callback);
        }

        public void BeginHomeTimeline(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, GenericResponseDelegate callback)
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

            Context.BeginRequest(BaseAddress + "home_timeline.json",
                p,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginFriendsTimeline(GenericResponseDelegate callback)
        {
            Context.BeginRequest(BaseAddress + "friends_timeline.json",
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginUserTimeline(String username, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "user", username } };

            Context.BeginRequest(
                BaseAddress + "user_timeline.json",
                p,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginMentions(GenericResponseDelegate callback)
        {
            BeginMentions(null, null, null, null, false, false, callback);
        }

        public void BeginMentions(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, GenericResponseDelegate callback)
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

            Context.BeginRequest(
                BaseAddress + "mentions.json",
                p,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginRetweetedByMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                BaseAddress + "retweeted_by_me.json",
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginRetweetedToMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                BaseAddress + "retweeted_to_me.json",
                null,
                WebMethod.Get,
                  (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginRetweetedOfMe(GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                BaseAddress + "retweeted_of_me.json",
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<Tweet>>(callback, req));
        }

        public void BeginGetFriends(GenericResponseDelegate callback)
        {
            Context.BeginRequest(
                BaseAddress + "friends.json",
                null,
                WebMethod.Get,
                (req, res, state) => res.ParseResult<ResultsWrapper<User>>(callback, req));
        }
    }
}
