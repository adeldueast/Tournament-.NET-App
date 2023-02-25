using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode statusCode, object? value = null) : base(value.ToString())
        {

            (StatusCode, Value) = ((int)statusCode, value);
        }


        public int StatusCode { get; }

        public object? Value { get; }
    }
}
