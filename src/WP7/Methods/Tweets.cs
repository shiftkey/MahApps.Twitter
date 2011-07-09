using System;
using MahApps.RESTBase;

namespace MahApps.Twitter.Methods
{
    [Obsolete]
    public class Tweets : RestMethodsBase<IBaseTwitterClient>
    {
        public Tweets(IBaseTwitterClient context)
            : base(context)
        {
        }
    }
}
