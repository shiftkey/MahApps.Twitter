using System;
using MahApps.Twitter.Models;
using Newtonsoft.Json;

namespace MahApps.Twitter.Extensions
{
    public static class JsonExtensions
    {
        public static ITwitterResponse Deserialize<T>(this string json) where T : ITwitterResponse
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (JsonSerializationException ex)
            {
                return new ExceptionResponse
                           {
                               Content = json,
                               ErrorMessage = ex.Message
                           };
            }
            catch (NullReferenceException ex)
            {
                return new ExceptionResponse
                           {
                               Content = json,
                               ErrorMessage = ex.Message
                           };
            }
            catch (Exception ex)
            {
                return new ExceptionResponse
                           {
                               Content = json,
                               ErrorMessage = ex.Message
                           };
            }
        }
    }
}
