using System;
using System.Net;

namespace LeagueActivityBot.Exceptions
{
    public class ApiResponseException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; }
        
        public ApiResponseException()
        {
        }

        public ApiResponseException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public ApiResponseException(string message, HttpStatusCode httpStatusCode, Exception innerException) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}