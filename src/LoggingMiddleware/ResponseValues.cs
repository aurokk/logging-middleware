using System.Collections.Generic;

namespace Aurokk.LoggingMiddleware
{
    public class ResponseValues
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}