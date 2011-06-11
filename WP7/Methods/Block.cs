﻿using System;
using System.Collections.Generic;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Models;

namespace MahApps.Twitter.Methods
{
    public class Block: RestMethodsBase<ITwitterClient>
    {
        private String baseAddress = "blocks/";
        public Block(ITwitterClient context)
            : base(context)
        {
        }

        public void BeginBlock(String Username, GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest(baseAddress + "create.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }

        public void BeginSpamBlock (String Username, GenericResponseDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("screen_name", Username);

            Context.BeginRequest("report_spam.json", p, WebMethod.Post, (req, res, state) =>
            {
                ITwitterResponse obj = Context.Deserialise<User>(res.Content);
                if (callback != null)
                    callback(req, res, obj);
            });
        }
    }
}
