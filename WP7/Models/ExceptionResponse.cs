// ReSharper disable CheckNamespace
namespace MahApps.Twitter.Models
// ReSharper restore CheckNamespace
{
    public class ExceptionResponse : ITwitterResponse
    {
        public string ErrorMessage { get; set; }
        public string Content { get; set; }
    }
}