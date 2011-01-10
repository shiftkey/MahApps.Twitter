using System.Collections.Generic;
using System.IO;
using Hammock;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Models;
using Newtonsoft.Json;
using File = MahApps.RESTBase.File;

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

        public void BeginUpdateProfileImage(FileInfo f, TwitterClient.GenericResponseDelegate callback)
        {
            Dictionary<string, File> files = new Dictionary<string, File>();
            files.Add("image", new File(f.FullName, f.Name));

            Context.BeginRequest("account/update_profile_image.json", null, files, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = TwitterClient.Deserialise<User>(res.Content);

                if (callback != null)
                    callback(req, res, obj);
            });
        }

    }
}
