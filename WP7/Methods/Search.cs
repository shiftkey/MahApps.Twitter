using System;
using System.Collections.Generic;
using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Search : RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "http://search.twitter.com";
        private String basePath = "search.json";

        public Search(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginSearch(String q, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("q", q);

            BeginRequest(p, WebMethod.Get, (req, res, state) =>
                                                  {
                                                      ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper>(res.Content);

                                                      if (callback != null)
                                                          callback(req, res, ((ResultsWrapper)obj).Results);
                                                  });
        }


        internal void BeginRequest(Dictionary<String, String> Parameters, WebMethod Method,
                                RestCallback callback)
        {
            var request = new RestRequest
            {
                Path = basePath,
                Method = Method
            };

            if (Parameters != null)
                foreach (var p in Parameters)
                {
                    request.AddParameter(p.Key, p.Value);
                }

            var Client = new RestClient()
                             {
                                 Authority = baseAddress
                             };

            Client.BeginRequest(request, callback);
        }

        public void BeginGetSavedSearches(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest("/saved_searches.json", null, WebMethod.Get, (req, res, state) =>
                                                                                  {
                                                                                      ITwitterResponse obj = TwitterClient.Deserialise<ResultsWrapper<SavedSearch>>(res.Content);

                                                                                      if (callback != null)
                                                                                          callback(req, res, obj);
                                                                                  });
        }
    }
}
