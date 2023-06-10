using System;

namespace LeagueActivityBot.Exceptions
{
    public class HttpClientException : ApplicationException
    {
        public HttpClientException()
        {
        }

        public HttpClientException(string message) : base(message)
        {
        }
        
        public HttpClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}