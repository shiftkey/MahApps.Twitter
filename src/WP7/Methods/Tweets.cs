using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class Tweets : RestMethodsBase<IBaseTwitterClient>
    {
        public Tweets(IBaseTwitterClient context)
            : base(context)
        {
        }
    }
}
