using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class DirectMessages : RestMethodsBase<TwitterClient>
    {
        private const string BaseAddress = "direct_messages";

        public DirectMessages(TwitterClient context)
            : base(context)
        {
        }

        public void BeginDirectMessages(TwitterClient.GenericResponseDelegate callback)
        {
            BeginDirectMessages(null, null, null, null, false, false, callback);
        }

        public void BeginDirectMessages(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, TwitterClient.GenericResponseDelegate callback)
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

            Context.BeginRequest(BaseAddress + ".json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginSentDirectMessages(TwitterClient.GenericResponseDelegate callback)
        {
            BeginSentDirectMessages(null, null, null, null, false, false, callback);
        }

        public void BeginSentDirectMessages(long? sinceId, long? maxId, long? count, int? page, bool trimUser, bool includeEntities, TwitterClient.GenericResponseDelegate callback)
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

            Context.BeginRequest(BaseAddress + "/sent.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginCreate(string screenName, string text, TwitterClient.GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "screen_name", screenName }, { "text", text } };

            Context.BeginRequest(BaseAddress + "/new.json", p, WebMethod.Post, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<DirectMessage>(res.Content);

                if (callback != null)
                    callback(req, res, obj);

            });
        }
    }
}
