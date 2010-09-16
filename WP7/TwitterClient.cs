using System;
using System.Collections.Generic;
#if !SILVERLIGHT
using System.Timers;
#endif

using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Streaming;
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

        public TwitterClient(String ConsumerKey, String ConsumerSecret, String Callback)
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
#if SILVERLIGHT
                             HasElevatedPermissions = true
#endif
                         };


            Credentials = new OAuthCredentials
                              {
                                  ConsumerKey = ConsumerKey,
                                  ConsumerSecret = ConsumerSecret,
                              };

            if (!String.IsNullOrEmpty(Callback))
                Credentials.CallbackUrl = Callback;
        }

#if !SILVERLIGHT && !WINDOWS_PHONE && !MONO
        #region User Stream bits
        private Timer _timer = null;
        private Timer _reconnectTimer = null;
        private DateTime _lastConnectAttempt;
        public bool Reconnect = true;

        public delegate void VoidDelegate();
        public delegate void TweetCallback(RestRequest request, RestResponse response, ITwitterResponse DeserialisedResponse);

        public event VoidDelegate StreamingReconnectAttemptEvent;

        public TweetCallback Callback { get; set;}
        public IAsyncResult StreamingAsyncResult { get; set; }
        public IAsyncResult BeginStream(TweetCallback callback)
        {
            if (StreamingAsyncResult == null)
            {
                _lastConnectAttempt = DateTime.Now;
                Callback = callback;
                Client = new RestClient()
                {
                    Authority = "https://userstream.twitter.com",
                    VersionPath = "2",
                    Credentials = Credentials,

                };

                var req = new RestRequest()
                {
                    Path = "user.json",
                    StreamOptions = new StreamOptions()
                    {
                        ResultsPerCallback = 1,
                    },
                };

                if (_timer == null)
                {
                    _timer = new Timer(20 * 1000);
                    _timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                    _timer.Start();
                }

                StreamingAsyncResult = Client.BeginRequest(req, StreamCallback);
            }
            return StreamingAsyncResult;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (StreamingAsyncResult != null)
                if (StreamingAsyncResult.IsCompleted == true && Reconnect == true)
                {
                    Console.WriteLine("stream disconnected, attempting reconnect");

                    if (DateTime.Now.Subtract(_lastConnectAttempt) > TimeSpan.FromMinutes(2))
                    {
                        _lastConnectAttempt = DateTime.Now;
                        BeginStream(Callback);
                        if (StreamingReconnectAttemptEvent != null)
                            StreamingReconnectAttemptEvent();
                    }
                }
        }

        void StreamCallback(Hammock.RestRequest request, Hammock.RestResponse response, object userState)
        {
            try
            {
                var SaneText = response.Content.Trim();
                Console.WriteLine(SaneText);
                ITwitterResponse deserialisedResponse = null;

                if (SaneText.StartsWith("{\"direct_message\""))
                {
                    DirectMessageContainer obj = JsonConvert.DeserializeObject<DirectMessageContainer>(SaneText);
                    deserialisedResponse = (DirectMessage)obj.DirectMessage;

                }
                else if (SaneText.StartsWith("{\"target\":"))
                {
                    StreamEvent obj = JsonConvert.DeserializeObject<StreamEvent>(SaneText);
                }
                else if (SaneText.Contains("\"retweeted_status\":{"))
                {
                    deserialisedResponse = (Retweet) JsonConvert.DeserializeObject<Retweet>(SaneText);
                }
                else
                {
                    deserialisedResponse = (Tweet)JsonConvert.DeserializeObject<Tweet>(SaneText);

                }

                if (deserialisedResponse != null)
                    Callback(request, response, deserialisedResponse);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error streaming:" + ex.Message +  response.Content.Trim() + "END ERROR");

            }

        }
        #endregion
#endif
    }
}
