using System;
using System.Collections.Generic;
using System.Net;
using MahApps.RESTBase;

namespace MahApps.Twitter
{
    public interface ITwitterClient : IBaseTwitterClient
    {
        WebRequest DelegatedRequest(string url, TwitterClient.Format format);
        event TwitterClient.VoidDelegate StreamingReconnectAttemptEvent;
        event TwitterClient.VoidDelegate StreamingDisconnectedEvent;
        IAsyncResult BeginStream(TwitterClient.TweetCallback callback, List<string> tracks);
        void SetOAuthToken(Credentials credentials);
    }
}