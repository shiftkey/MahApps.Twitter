using System;
using System.Collections.Generic;
using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Extensions;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Search : RestMethodsBase<IBaseTwitterClient>
    {
        private String baseAddress = "http://search.twitter.com";
        private String basePath = "search.json";

        public Search(IBaseTwitterClient context)
            : base(context)
        {
        }

        public void BeginSearch(String q, GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("q", q);

            BeginRequest(p, WebMethod.Get, (req, res, state) =>
                                                  {
                                                      ITwitterResponse obj = res.Content.Deserialize<ResultsWrapper>();

                                                      if (callback != null)
                                                          callback(req, res, (!(obj is ExceptionResponse)) ? ((ResultsWrapper)obj).Results : null);
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

        public void BeginGetSavedSearches(GenericResponseDelegate callback)
        {
            Context.BeginRequest("/saved_searches.json", null, WebMethod.Get, (req, res, state) =>
                                                                                  {
                                                                                      ITwitterResponse obj = res.Content.Deserialize<ResultsWrapper<SavedSearch>>();

                                                                                      if (callback != null)
                                                                                          callback(req, res, obj);
                                                                                  });
        }
    }
}
