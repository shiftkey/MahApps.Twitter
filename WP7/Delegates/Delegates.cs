using Hammock;

using MahApps.Twitter.Models;

namespace MahApps.Twitter.Delegates
{
    public delegate void VoidDelegate();
    public delegate void TweetCallback(RestRequest request, RestResponse response, ITwitterResponse deserialisedResponse);
    public delegate void GenericResponseDelegate(RestRequest request, RestResponse response, object data);

}
