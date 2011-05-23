using MahApps.RESTBase;
using MahApps.Twitter.Models;

namespace MahApps.Twitter
{
    public interface ITwitterClient : IRestClientBase
    {
        ITwitterResponse Deserialise<T>(string content) where T : ITwitterResponse;

        bool Encode { get; set; }
    }
}