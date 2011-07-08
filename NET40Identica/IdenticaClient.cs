using Hammock;
using Hammock.Authentication.Basic;
using MahApps.RESTBase;
using MahApps.Twitter;
using MahApps.Twitter.Methods;

namespace MahApps.Identica
{
    public class IdenticaClient : RestClientBase, IBaseTwitterClient
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

        public bool Encode { get; set; }
    }
}
