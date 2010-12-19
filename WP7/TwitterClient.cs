using System;
using System.Net;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Streaming;
using Hammock.Web;
using MahApps.RESTBase;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using Newtonsoft.Json;
using WebHeaderCollection = System.Net.WebHeaderCollection;

#if !SILVERLIGHT
using System.Timers;
#endif

namespace MahApps.Twitter
{
    public class TwitterClient : RestClientBase
    {
        public delegate void GenericResponseDelegate(RestRequest request, RestResponse response, object data);

        public Account Account { get; set; }
        public Statuses Statuses { get; set; }
        public Block Block { get; set; }
        public List Lists { get; set; }
        public Search Search { get; set; }
        public DirectMessages DirectMessages { get; set; }
        public Favourites Favourites { get; set; }
        public Friendship Friendships { get; set; }

        public static ITwitterResponse Deserialise<T>(string content) where T : ITwitterResponse
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(content);
                return obj;
            }
            catch (JsonSerializationException ex)
            {
                return new ExceptionResponse
                {
                               Content = content,
                               ErrorMessage = ex.Message
                           };
            }
            catch (NullReferenceException ex)
            {
                return new ExceptionResponse
                {
                    Content = content,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ExceptionResponse
                {
                    Content = content,
                    ErrorMessage = ex.Message
                };
            }
        }

        public TwitterClient(string consumerKey, string consumerSecret, string callback)
        {
            Statuses = new Statuses(this);
            Account = new Account(this);
            DirectMessages = new DirectMessages(this);
            Favourites = new Favourites(this);
            Block = new Block(this);
            Friendships = new Friendship(this);
            Lists = new List(this);
            Search = new Search(this);

            OAuthBase = "https://api.twitter.com/oauth/";
            TokenRequestUrl = "request_token";
            TokenAuthUrl = "authorize";
            TokenAccessUrl = "access_token";
            Authority = "https://api.twitter.com/";
            Version = "1";

#if !SILVERLIGHT
            ServicePointManager.Expect100Continue = false;
#endif

            Client = new RestClient
                         {
                             Authority = OAuthBase,
#if SILVERLIGHT
                             HasElevatedPermissions = true
#endif
                         };


            Credentials = new OAuthCredentials
                              {
                                  ConsumerKey = consumerKey,
                                  ConsumerSecret = consumerSecret,
                              };

            if (!String.IsNullOrEmpty(callback))
                Credentials.CallbackUrl = callback;
        }

        public enum Format
        {
            Xml,
            Json
        }

        public WebRequest DelegatedRequest(string url, Format format)
        {
            var restClient = (RestClient)Client;

            Credentials.Version = "1.0";
            Credentials.CallbackUrl = String.Empty;

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
        #if !SILVERLIGHT 
        #region SiteStreams
        public void BeginSiteStream(String UserID, TweetCallback callback)
        {
            
            Callback = callback;
            this.Credentials.CallbackUrl = null;
            var streamClient = new RestClient()
            {
                Authority = "http://betastream.twitter.com/",
                VersionPath = "2b",
                Credentials = Credentials,
            };

            var req = new RestRequest()
            {
                Path = "site.json?follow=" + UserID,
                StreamOptions = new StreamOptions()
                {
                    ResultsPerCallback = 1,
                },
            };
            streamClient.BeginRequest(req, SiteStreamCallback);
        }
        void SiteStreamCallback(Hammock.RestRequest request, Hammock.RestResponse response, object userState)
        {
            try
            {
                var SaneText = response.Content.Trim();
                //Console.WriteLine(SaneText);
                ITwitterResponse deserialisedResponse = null;

                deserialisedResponse = JsonConvert.DeserializeObject<SiteStreamsWrapper>(SaneText); 
                Callback(request, response, deserialisedResponse);
            }
            catch
            {
                
            }
        }

        #endregion
#endif
        #region User Stream bits
#if !SILVERLIGHT 
        private System.Timers.Timer _timer = null;
        private System.Timers.Timer _reconnectTimer = null;
#endif
        private DateTime _lastConnectAttempt;
        public bool Reconnect = true;

        public delegate void VoidDelegate();
        public delegate void TweetCallback(RestRequest request, RestResponse response, ITwitterResponse deserialisedResponse);

        public event VoidDelegate StreamingReconnectAttemptEvent;

        public TweetCallback Callback { get; set; }
        public IAsyncResult StreamingAsyncResult { get; set; }
        public IAsyncResult BeginStream(TweetCallback callback)
        {
            if (StreamingAsyncResult == null)
            {
                _lastConnectAttempt = DateTime.Now;
                Callback = callback;
                Credentials.CallbackUrl = null;
                var streamClient = new RestClient()
                {
                    Authority = "https://userstream.twitter.com",
                    VersionPath = "2",
                    Credentials = Credentials,
#if SILVERLIGHT
                             HasElevatedPermissions = true
#endif
                };

                var req = new RestRequest()
                {
                    Path = "user.json",
                    StreamOptions = new StreamOptions()
                    {
                        ResultsPerCallback = 1,
                    },
                };
#if !SILVERLIGHT 
                if (_timer == null)
                {
                    _timer = new System.Timers.Timer(20 * 1000);
                    _timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                    _timer.Start();
                }

                StreamingAsyncResult = streamClient.BeginRequest(req, StreamCallback);
                streamClient.BeginRequest(req, StreamCallback);
#endif
            }
            return StreamingAsyncResult;
        }
#if !SILVERLIGHT 
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
#endif

        void StreamCallback(RestRequest request, RestResponse response, object userState)
        {
            try
            {
                var saneText = response.Content.Trim();
                
                ITwitterResponse deserialisedResponse = null;

                if (saneText.StartsWith("{\"direct_message\""))
                {
                    var obj = JsonConvert.DeserializeObject<DirectMessageContainer>(saneText);
                    deserialisedResponse = obj.DirectMessage;

                }
                else if (saneText.StartsWith("{\"target\":"))
                {
                    deserialisedResponse = JsonConvert.DeserializeObject<StreamEvent>(saneText);
                }
                else if (saneText.Contains("\"retweeted_status\":{"))
                {
                    deserialisedResponse = JsonConvert.DeserializeObject<Tweet>(saneText);
                }
                else
                {
                    deserialisedResponse = JsonConvert.DeserializeObject<Tweet>(saneText);

                }

                if (deserialisedResponse != null)
                    Callback(request, response, deserialisedResponse);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error streaming:" + ex.Message + response.Content.Trim() + "END ERROR");
            }

        }
        #endregion
    }
}
