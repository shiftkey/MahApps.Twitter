﻿using System;
using System.Collections.Generic;
#if !SILVERLIGHT
using System.Diagnostics;
using System.Linq;
using System.Timers;
#endif
using System.Linq;
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
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using WebHeaderCollection = System.Net.WebHeaderCollection;

namespace MahApps.Twitter
{
    public class TwitterClient : RestClientBase, ITwitterClient
    {
        private const string AuthorizationTemplate = "OAuth oauth_consumer_key=\"{0}\",oauth_token=\"{1}\",oauth_signature_method=\"{2}\",oauth_signature=\"{3}\",oauth_timestamp=\"{4}\",oauth_nonce=\"{5}\",oauth_version=\"{6}\"";

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
            var credentials = ((OAuthCredentials)Credentials);
            credentials.Version = "1.0";
            credentials.CallbackUrl = string.Empty;

            var request = new RestRequest
            {
                Credentials = Credentials,
                Path = "account/verify_credentials." + ((format == Format.Json) ? "json" : "xml"),
                Method = WebMethod.Get
            };

            // TODO: cannot intercept BuildEndpoint without having to mock RestClient
            var restClient = (RestClient)Client;
            var endpoint = request.BuildEndpoint(restClient);

            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Headers = new WebHeaderCollection();
            webReq.Headers["X-Auth-Service-Provider"] = endpoint.ToString();

            var x = new OAuthWebQueryInfo();
            var query = Credentials.GetQueryFor(endpoint.ToString(), request, x, WebMethod.Get, true);
            var info = query.Info as OAuthWebQueryInfo;
            if (info != null)
            {
                var xVerifyCredentialsAuthorization = string.Format(AuthorizationTemplate, info.ConsumerKey, info.Token, info.SignatureMethod, info.Signature, info.Timestamp, info.Nonce, info.Version);
                webReq.Headers["X-Verify-Credentials-Authorization"] = xVerifyCredentialsAuthorization;
            }

            return webReq;
        }

        public void BeginXAuthAuthenticate(String username, String password, AccessTokenCallbackDelegate callback)
        {
            var p = new Dictionary<string, string>
                        {
                            {"x_auth_username", username},
                            {"x_auth_password", password},
                            {"x_auth_mode", "client_auth"}
                        };

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

        private void SiteStreamCallback(RestRequest request, RestResponse response, object userState)
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
        public TweetCallback Callback { get; private set; }

        public Action<RestResponse, ITwitterResponse> CallbackNoRequest { get; private set; }

        public IEnumerable<string> Tracks { get; private set; }

        public IAsyncResult StreamingAsyncResult { get; private set; }

        public IAsyncResult BeginStream(TweetCallback callback, IEnumerable<string> tracks)
        {
            Callback = callback;
            Tracks = tracks;

            return BeginStreamInternal();
        }

        public IAsyncResult BeginStream(Action<RestResponse, ITwitterResponse> callback, IEnumerable<string> tracks)
        {
            CallbackNoRequest = callback;
            Tracks = tracks;

            return BeginStreamInternal();
        }

        private IAsyncResult BeginStreamInternal()
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

            if (Tracks != null)
                req.AddParameter("track", string.Join(",", Tracks.ToArray()));

#if !SILVERLIGHT
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(20000);
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

            if (!StreamingAsyncResult.IsCompleted)
                return;

            if (!Reconnect)
                return;

            if (StreamingDisconnectedEvent != null)
                StreamingDisconnectedEvent();

            Trace.WriteLine("Stream disconnected -- attempting reconnect");

            if (DateTime.Now.Subtract(_lastConnectAttempt) <= TimeSpan.FromMinutes(2))
                return;

            _lastConnectAttempt = DateTime.Now;
            BeginStream(Callback, Tracks);

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
                Trace.WriteLine("Error streaming: " + ex.Message + " with content '" + response.Content.Trim() + "' END");
            }
        }
    }
}
