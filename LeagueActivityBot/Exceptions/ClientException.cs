using System;

namespace LeagueActivityBot.Exceptions
{
    public class ClientException : ApplicationException
    {
        public ClientException()
        {
        }

        public ClientException(string message) : base(message)
        {
        }
        
        public ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}