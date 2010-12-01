using System;
using System.Collections.Generic;
#if !SILVERLIGHT
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
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using Newtonsoft.Json;
using WebHeaderCollection = System.Net.WebHeaderCollection;

namespace MahApps.Twitter
{
    public class TwitterClient : RestClientBase
    {
        public delegate void GenericResponseDelegate(RestRequest request, RestResponse response, object Response);

        public Account Account { get; set; }
        public Statuses Statuses { get; set; }
        public Block Block { get; set; }
        public List Lists { get; set; }
        public Search Search { get; set; }
        public DirectMessages DirectMessages { get; set; }
        public Favourites Favourites { get; set; }
        public Friendship Friendships { get; set; }

        public static ITwitterResponse Deserialise<T>(String Content) where T : ITwitterResponse
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(Content);
                return obj;
            }
            catch (JsonSerializationException ex)
            {
                return new ExceptionResponse()
                           {
                               Content = Content,
                               ErrorMessage = ex.Message
                           };
            }
            catch (NullReferenceException ex)
            {
                return new ExceptionResponse()
                {
                    Content = Content,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Content = Content,
                    ErrorMessage = ex.Message
                };
            }

            return null;
        }

        public TwitterClient(String ConsumerKey, String ConsumerSecret, String Callback)
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
                                  ConsumerKey = ConsumerKey,
                                  ConsumerSecret = ConsumerSecret,
                              };

            if (!String.IsNullOrEmpty(Callback))
                Credentials.CallbackUrl = Callback;
        }

        public enum Format
        {
            Xml,
            Json
        }

        public WebRequest DelegatedRequest(String Url, Format format)
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

            var url = request.BuildEndpoint(restClient);
            var x = new OAuthWebQueryInfo();
            var query = Credentials.GetQueryFor(url.ToString(), request, x, WebMethod.Get);
            var info = (query.Info as OAuthWebQueryInfo);

            var XVerifyCredentialsAuthorization =
            String.Format("OAuth oauth_consumer_key=\"{0}\",oauth_token=\"{1}\",oauth_signature_method=\"{2}\",oauth_signature=\"{3}\",oauth_timestamp=\"{4}\",oauth_nonce=\"{5}\",oauth_version=\"{6}\"",
                info.ConsumerKey, info.Token, info.SignatureMethod, info.Signature, info.Timestamp, info.Nonce, info.Version);

            var webReq = (HttpWebRequest)WebRequest.Create(Url);
            webReq.Headers = new WebHeaderCollection();
            webReq.Headers["X-Auth-Service-Provider"] = url.ToString();
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
        public delegate void TweetCallback(RestRequest request, RestResponse response, ITwitterResponse DeserialisedResponse);

        public event VoidDelegate StreamingReconnectAttemptEvent;

        public TweetCallback Callback { get; set; }
        public IAsyncResult StreamingAsyncResult { get; set; }
        public IAsyncResult BeginStream(TweetCallback callback)
        {
            if (StreamingAsyncResult == null)
            {
                _lastConnectAttempt = DateTime.Now;
                Callback = callback;
                this.Credentials.CallbackUrl = null;
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
#endif
#if SILVERLIGHT
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

        void StreamCallback(Hammock.RestRequest request, Hammock.RestResponse response, object userState)
        {
            try
            {
                var SaneText = response.Content.Trim();
                //Console.WriteLine(SaneText);
                ITwitterResponse deserialisedResponse = null;

                if (SaneText.StartsWith("{\"direct_message\""))
                {
                    DirectMessageContainer obj = JsonConvert.DeserializeObject<DirectMessageContainer>(SaneText);
                    deserialisedResponse = (DirectMessage)obj.DirectMessage;

                }
                else if (SaneText.StartsWith("{\"target\":"))
                {
                    deserialisedResponse = (StreamEvent)JsonConvert.DeserializeObject<StreamEvent>(SaneText);
                }
                else if (SaneText.Contains("\"retweeted_status\":{"))
                {
                    deserialisedResponse = (Tweet)JsonConvert.DeserializeObject<Tweet>(SaneText);
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
                Console.WriteLine("Error streaming:" + ex.Message + response.Content.Trim() + "END ERROR");

            }

        }
        #endregion
    }
}
