using System;
using Hammock;
using Hammock.Authentication.Basic;
using MahApps.RESTBase;
using MahApps.Twitter;
using MahApps.Twitter.Methods;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Identica
{
    public class IdenticaClient : RestClientBase, ITwitterClient
    {
        public Account Account { get; set; }
        public Statuses Statuses { get; set; }
        public Block Block { get; set; }
        public List Lists { get; set; }
        public Search Search { get; set; }
        public DirectMessages DirectMessages { get; set; }
        public FriendsAndFollowers FriendsAndFollowers { get; set; }
        public Favourites Favourites { get; set; }
        public Friendship Friendships { get; set; }
        public Users Users { get; set; }

        public IdenticaClient(string username, string password)
        {
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

            Credentials = new BasicAuthCredentials
            {
                Username = username,
                Password = password
            };

            Client = new RestClient
                         {
                             Authority = "http://identi.ca/api",

                         };            
            
            Authority = "http://identi.ca/api";

            Encode = false;
        }

        public ITwitterResponse Deserialise<T>(string content) where T : ITwitterResponse
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(content);
                return obj;
            }
            catch (JsonSerializationException ex)
            {
                return new ExceptionResponse()
                {
                    Content = content,
                    ErrorMessage = ex.Message
                };
            }
            catch (NullReferenceException ex)
            {
                return new ExceptionResponse()
                {
                    Content = content,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ExceptionResponse()
                {
                    Content = content,
                    ErrorMessage = ex.Message
                };
            }

            return null;
        }

        public bool Encode { get; set; }
    }
}
