using System;
using System.Collections.Generic;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter
{
    public class TwitterClient : RestClientBase
    {
        public delegate void GenericResponseDelegate(RestRequest request, RestResponse response, object Response);
        
        public Account Account { get; set; }
        public Statuses Statuses { get; set; }
        public List Lists { get; set; }
        public DirectMessage DirectMessages { get; set; }
        public Favourites Favourites { get; set; }
        public Friendship Friendships { get; set;}

        public TwitterClient(String ConsumerKey, String ConsumerSecret)
        {
            Statuses = new Statuses(this);
            Account = new Account(this);
            OAuthBase = "https://api.twitter.com/oauth/";
            TokenRequestUrl = "request_token";
            TokenAuthUrl = "authorize";
            TokenAccessUrl = "access_token";
            Authority = "https://api.twitter.com/";
            Version = "1";

            Client = new RestClient
                         {
                             Authority = OAuthBase,
                             HasElevatedPermissions = true
                         };


            Credentials = new OAuthCredentials
                              {
                                  ConsumerKey = ConsumerKey,
                                  ConsumerSecret = ConsumerSecret,
                                  CallbackUrl = "http://www.mahtweets.com"
                              };
        }
    }
    

    


    
}
