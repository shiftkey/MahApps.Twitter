using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter.Methods
{
    public class Account : RestMethodsBase<TwitterClient>
    {
        public Account(TwitterClient Context)
            : base(Context)
        {
        }

        public void BeginVerifyCredentials(TwitterClient.GenericResponseDelegate callback)
        {
            Context.BeginRequest("account/verify_credentials.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<User>(res.Content);
                callback(req, res, obj);
            });
        }

    }
}
