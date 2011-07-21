using System;
using System.Collections.Generic;
#if !SILVERLIGHT
using System.Linq;
using System.Timers;
#endif
using System.Net;
using System.Text;
using System.Threading;
using Hammock;
using Hammock.Authentication.OAuth;
#if SILVERLIGHT
using Hammock.Silverlight.Compat;
#endif
using Hammock.Streaming;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.RESTBase.Delegates;
using MahApps.Twitter.Delegates;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebHeaderCollection = System.Net.WebHeaderCollection;

namespace MahApps.Twitter
{
    public class TwitterClient : RestClientBase, ITwitterClient
    {
        public Account Account { get; set; }
        public Statuses Statuses { get; set; }
        public Block Block { get; set; }
        public List Lists { get; set; }
        public Search Search { get; set; }
        public DirectMessages DirectMessages { get; set; }
        public Favourites Favourites { get; set; }
        public FriendsAndFollowers FriendsAndFollowers { get; set; }
        public Friendship Friendships { get; set; }
        public Users Users { get; set; }


        public bool Encode { get; set; }

        public TwitterClient(string consumerKey, string consumerSecret, string callback)
            : this(new RestClient
            {
                Authority = "https://api.twitter.com/oauth/",
#if SILVERLIGHT
                HasElevatedPermissions = true
#endif
            }, consumerKey, consumerSecret, callback)
        {

        }

        public TwitterClient(IRestClient client, string consumerKey, string consumerSecret, string callback)
            : base(client)
        {
            Encode = true;
            Statuses = new Statuses(this);
            Account = new Account(this);
            DirectMessages = new DirectMessages(this);
            Favourites = new Favourites(this);
            Block = new Block(this);
            Friendships = new Friendship(this);
            Lists = new List(this);
            Search = new Search(this);
            Users = new Users(this);
            FriendsAndFollowers = new FriendsAndFollowers(this);

            OAuthBase = "https://api.twitter.com/oauth/";
            TokenRequestUrl = "request_token";
            TokenAuthUrl = "authorize";
            TokenAccessUrl = "access_token";
            Authority = "https://api.twitter.com/";
            Version = "1";

#if !SILVERLIGHT
            ServicePointManager.Expect100Continue = false;
#endif
            Credentials = new OAuthCredentials
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
            };

            if (!string.IsNullOrEmpty(callback))
                ((OAuthCredentials)Credentials).CallbackUrl = callback;
        }

        public WebRequest DelegatedRequest(string url, Format format)
        {
            var restClient = (RestClient)Client;

            ((OAuthCredentials)Credentials).Version = "1.0";
            ((OAuthCredentials)Credentials).CallbackUrl = String.Empty;

            RestRequest request = new RestRequest
            {
                Credentials = Credentials,
                Path = "account/verify_credentials." + ((format == Format.Json) ? "json" : "xml"),
                Method = WebMethod.Get
            };

            var endpoint = request.BuildEndpoint(restClient);
            var x = new OAuthWebQueryInfo();
            var query = Credentials.GetQueryFor(endpoint.ToString(), request, x, WebMethod.Get);
            var info = (query.Info as OAuthWebQueryInfo);

            var XVerifyCredentialsAuthorization =
            String.Format("OAuth oauth_consumer_key=\"{0}\",oauth_token=\"{1}\",oauth_signature_method=\"{2}\",oauth_signature=\"{3}\",oauth_timestamp=\"{4}\",oauth_nonce=\"{5}\",oauth_version=\"{6}\"",
                info.ConsumerKey, info.Token, info.SignatureMethod, info.Signature, info.Timestamp, info.Nonce, info.Version);

            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Headers = new WebHeaderCollection();
            webReq.Headers["X-Auth-Service-Provider"] = endpoint.ToString();
            webReq.Headers["X-Verify-Credentials-Authorization"] = XVerifyCredentialsAuthorization;
            return webReq;
        }

        public void BeginXAuthAuthenticate(String username, String password, AccessTokenCallbackDelegate callback)
        {
            Dictionary<String, String> p = new Dictionary<string, string>();
            p.Add("x_auth_username", username);
            p.Add("x_auth_password", password);
            p.Add("x_auth_mode", "client_auth");

            BeginRequest("access_token", p, WebMethod.Post, (req, res, state) => EndGetAccessToken(req, res, state, callback));
        }

#if !SILVERLIGHT
        public IAsyncResult BeginSiteStream(string userId, TweetCallback callback)
        {

            Callback = callback;
            ((OAuthCredentials)Credentials).CallbackUrl = null;
            var streamClient = new RestClient
                                   {
                                       Authority = "http://sitestream.twitter.com/",
                                       VersionPath = "2b",
                                       Credentials = Credentials,
                                   };

            var req = new RestRequest
                          {
                              Path = "site.json?follow=" + userId,
                              StreamOptions = new StreamOptions
                                                  {
                                                      ResultsPerCallback = 1,
                                                  },
                          };
            return streamClient.BeginRequest(req, SiteStreamCallback);
        }

        void SiteStreamCallback(RestRequest request, RestResponse response, object userState)
        {
            try
            {
                ITwitterResponse deserialisedResponse = StatusProcessor.Process<SiteStreamsWrapper>(response.Content);
                if (Callback != null)
                    Callback(request, response, deserialisedResponse);
                else if (CallbackNoRequest != null)
                    CallbackNoRequest(response, deserialisedResponse);
            }
            catch
            {

            }
        }

#endif

#if !SILVERLIGHT
        private System.Timers.Timer _timer = null;
#endif
        private DateTime _lastConnectAttempt;
        public bool Reconnect = true;

        [Obsolete("Not a fan of delegates any more")]
        public delegate void VoidDelegate();
        [Obsolete("Not a fan of delegates any more")]
        public delegate void TweetCallback(RestRequest request, RestResponse response, ITwitterResponse DeserialisedResponse);

        public event VoidDelegate StreamingReconnectAttemptEvent;
        public event VoidDelegate StreamingDisconnectedEvent;

        [Obsolete("To be deprecated")]
        public TweetCallback Callback { get; set; }

        public Action<RestResponse, ITwitterResponse> CallbackNoRequest { get; set; }
        public IAsyncResult StreamingAsyncResult { get; set; }

        public IAsyncResult BeginStream(TweetCallback callback, IEnumerable<string> tracks)
        {
            Callback = callback;

            return BeginStreamInternal(tracks);
        }

        public IAsyncResult BeginStream(Action<RestResponse, ITwitterResponse> callback, IEnumerable<string> tracks)
        {
            CallbackNoRequest = callback;

            return BeginStreamInternal(tracks);
        }

        private IAsyncResult BeginStreamInternal(IEnumerable<string> tracks)
        {
            if (StreamingAsyncResult != null)
            {
                if (!StreamingAsyncResult.IsCompleted)
                    return StreamingAsyncResult;
            }

            _lastConnectAttempt = DateTime.Now;
            ((OAuthCredentials)Credentials).CallbackUrl = null;
            var streamClient = new RestClient
                                   {
                                       Authority = "https://userstream.twitter.com",
                                       VersionPath = "2",
                                       Credentials = Credentials,
#if SILVERLIGHT
                                       HasElevatedPermissions = true
#endif
                                   };

            var req = new RestRequest
                          {
                              Path = "user.json",
                              StreamOptions = new StreamOptions
                                                  {
                                                      ResultsPerCallback = 1,
                                                  },
                          };

            if (tracks != null)
                req.AddParameter("track", string.Join(",", tracks.ToArray()));

#if !SILVERLIGHT
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(20 * 1000);
                _timer.Elapsed += TimerElapsed;
                _timer.Start();
            }

            StreamingAsyncResult = streamClient.BeginRequest(req, StreamCallback);
#else
            streamClient.BeginRequest(req, StreamCallback);
#endif
            return StreamingAsyncResult;
        }

#if !SILVERLIGHT
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (StreamingAsyncResult == null) 
                return;

            if (!StreamingAsyncResult.IsCompleted || !Reconnect) 
                return;

            if (StreamingDisconnectedEvent != null)
                StreamingDisconnectedEvent();

            Console.WriteLine("stream disconnected, attempting reconnect");

            if (DateTime.Now.Subtract(_lastConnectAttempt) <= TimeSpan.FromMinutes(2)) 
                return;

            _lastConnectAttempt = DateTime.Now;
            BeginStream(Callback, null);

            if (StreamingReconnectAttemptEvent != null)
                StreamingReconnectAttemptEvent();
        }
#endif

        private void StreamCallback(RestRequest request, RestResponse response, object userState)
        {
            try
            {
                var deserialisedResponse = StatusProcessor.Process(response.Content);

                if (deserialisedResponse == null)
                    return;

                if (Callback != null)
                    Callback(request, response, deserialisedResponse);
                else if (CallbackNoRequest != null)
                    CallbackNoRequest(response, deserialisedResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error streaming: " + ex.Message + " with content '" + response.Content.Trim() + "' END");
            }
        }
    }
}
