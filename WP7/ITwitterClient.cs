using MahApps.RESTBase;
using MahApps.Twitter.Methods;

namespace MahApps.Twitter
{
    public interface ITwitterClient : IRestClientBase
    {
        Account Account { get; }
        Statuses Statuses { get; }
        Block Block { get; }
        List Lists { get; }
        Search Search { get; }
        DirectMessages DirectMessages { get; }
        Favourites Favourites { get; }
        Friendship Friendships { get; }
    }
}