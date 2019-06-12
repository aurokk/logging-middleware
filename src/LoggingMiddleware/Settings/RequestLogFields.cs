namespace Aurokk.LoggingMiddleware.Settings
{
    public class RequestLogFields
    {
        public bool RemoteIpAddress { get; set; }
        public bool ContentType { get; set; }
        public bool Method { get; set; }
        public bool Protocol { get; set; }
        public bool IsHttps { get; set; }
        public bool Scheme { get; set; }
        public bool Host { get; set; }
        public bool Path { get; set; }
        public bool QueryString { get; set; }
        public bool Query { get; set; }
        public bool Headers { get; set; }
        public bool Cookies { get; set; }
        public bool Body { get; set; }
    }
}