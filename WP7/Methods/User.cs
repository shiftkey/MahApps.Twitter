using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Users : RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "users/";
        public Users(ITwitterClient context)
            : base(context)
        {

        }

        public void BeginSearch(string q,  GenericResponseDelegate callback)
        {
            var p = new Dictionary<string, string> { { "q", q } };

            Context.BeginRequest(baseAddress + "search.json", p, WebMethod.Get, (req, res, state) =>
            {
                var obj = Context.Deserialise<ResultsWrapper<User>>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
