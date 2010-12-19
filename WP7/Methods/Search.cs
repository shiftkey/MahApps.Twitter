using System.Collections.Generic;
using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Search : RestMethodsBase<ITwitterClient>
    {
        private const string BaseAddress = "http://search.twitter.com";
        private const string BasePath = "search.json";

        public Search(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginSearch(string q, GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "q", q } };

            BeginRequest(p, WebMethod.Get, (req, res, state) =>
                                                  {
                                                      var obj = TwitterClient.Deserialise<ResultsWrapper>(res.Content);

                                                      if (callback != null)
                                                          callback(req, res, ((ResultsWrapper)obj).Results);
                                                  });
        }

        internal void BeginRequest(
            IDictionary<string, string> parameters,
            WebMethod method,
            RestCallback callback)
        {
            var request = new RestRequest
            {
                Path = BasePath,
                Method = method
            };

            if (parameters != null)
                foreach (var p in parameters)
                {
                    request.AddParameter(p.Key, p.Value);
                }

            var client = new RestClient
            {
                Authority = BaseAddress
            };

            client.BeginRequest(request, callback);
        }

        public void BeginGetSavedSearches(GenericResponseDelegate callback)
        {
            Context.BeginRequest("/saved_searches.json", null, WebMethod.Get, (req, res, state) =>
                                                                                  {
                                                                                      var obj = TwitterClient.Deserialise<ResultsWrapper<SavedSearch>>(res.Content);

                                                                                      if (callback != null)
                                                                                          callback(req, res, obj);
                                                                                  });
        }
    }
}
