using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class Tweets : RestMethodsBase<ITwitterClient>
    {
        public Tweets(ITwitterClient context)
            : base(context)
        {
        }
    }
}
