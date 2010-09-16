using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class Favourites : RestMethodsBase<TwitterClient>
    {
        public Favourites(TwitterClient Context)
            : base(Context)
        {
        }
    }
}
