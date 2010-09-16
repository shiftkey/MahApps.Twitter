using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class Friendship : RestMethodsBase<TwitterClient>
    {
        public Friendship(TwitterClient Context)
            : base(Context)
        {
        }
    }
}
