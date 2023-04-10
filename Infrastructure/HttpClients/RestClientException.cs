using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PimsPublisher.Infrastructure.HttpClients
{
    internal class RestClientException: Exception
    {
        internal string Req { get; set; } = string.Empty;
        internal string Res { get; set; } = string.Empty;
        internal string ResConent { get; set; } = string.Empty;
       
        public RestClientException(string message, Exception? innerException = null): base(message, innerException) 
        { 
        
        }

    }
}
