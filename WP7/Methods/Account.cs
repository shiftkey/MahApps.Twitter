using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Account : RestMethodsBase<ITwitterClient>
    {
        public Account(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginVerifyCredentials(GenericResponseDelegate callback)
        {
            Context.BeginRequest("account/verify_credentials.json", null, WebMethod.Get, (req, res, state) =>
            {
                var obj = TwitterClient.Deserialise<User>(res.Content);
                callback(req, res, obj);
            });
        }

    }
}
