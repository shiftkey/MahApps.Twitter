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
            Context.BeginRequest(baseAddress + ".json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<DirectMessage> obj = JsonConvert.DeserializeObject<List<DirectMessage>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
        public void BeginSentDirectMessages(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest(baseAddress + "/sent.json", null, WebMethod.Get, (req, res, state) =>
            {
                //List<DirectMessage> obj = JsonConvert.DeserializeObject<List<DirectMessage>>(res.Content);
                ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<DirectMessage>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
        
    }
}
