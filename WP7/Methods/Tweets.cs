using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class Tweets : RestMethodsBase<TwitterClient>
    {
        public Tweets(TwitterClient Context)
            : base(Context)
        {
        }
    }
}
