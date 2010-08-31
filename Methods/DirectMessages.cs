using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    public class DirectMessages : RestMethodsBase<TwitterClient>
    {
        public DirectMessages(TwitterClient Context)
            : base(Context)
        {
        }
    }
}
