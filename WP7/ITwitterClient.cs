﻿using System;
using System.Collections.Generic;
using System.Net;
using MahApps.RESTBase;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;

namespace MahApps.Twitter
{
    public interface IBaseTwitterClient : IRestClientBase
    {
        ITwitterResponse Deserialise<T>(string content) where T : ITwitterResponse;
        bool Encode { get; set; }

        Account Account { get; }
        Statuses Statuses { get; }
        Block Block { get; }
        List Lists { get; }
        Search Search { get; }
        FriendsAndFollowers FriendsAndFollowers { get; set; }
        DirectMessages DirectMessages { get; }
        Favourites Favourites { get; }

        Friendship Friendships { get; }
        Users Users { get; }
    }

    public interface ITwitterClient : IBaseTwitterClient
    {
        WebRequest DelegatedRequest(String Url, TwitterClient.Format format);
        event TwitterClient.VoidDelegate StreamingReconnectAttemptEvent;
        event TwitterClient.VoidDelegate StreamingDisconnectedEvent;
        IAsyncResult BeginStream(TwitterClient.TweetCallback callback, List<string> tracks);
        void SetOAuthToken(Credentials credentials);
    }
}