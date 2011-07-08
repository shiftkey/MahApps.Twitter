using System.Collections.Generic;
using System.IO;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Extensions;
using MahApps.Twitter.Models;
using File = MahApps.RESTBase.File;

namespace MahApps.Twitter.Methods
{
    public class Account : RestMethodsBase<IBaseTwitterClient>
    {
        public Account(IBaseTwitterClient context)
            : base(context)
        {
        }

        public void BeginVerifyCredentials(GenericResponseDelegate callback)
        {
            Context.BeginRequest("account/verify_credentials.json", null, WebMethod.Get, (req, res, state) =>
            {
                ITwitterResponse obj = res.Content.Deserialize<User>();
                callback(req, res, obj);
            });
        }

        public void BeginUpdateProfileImage(FileInfo f, GenericResponseDelegate callback)
        {
            Dictionary<string, File> files = new Dictionary<string, File>();

            if (f.Exists)
                files.Add("image", new File(f.FullName, f.Name));

            Context.BeginRequest("account/update_profile_image.json", null, files, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = res.Content.Deserialize<User>();

                if (callback != null)
                    callback(req, res, obj);
            });
        }

    }
}
