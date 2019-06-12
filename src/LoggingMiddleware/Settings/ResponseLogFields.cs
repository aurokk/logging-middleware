namespace Aurokk.LoggingMiddleware.Settings
{
    public class ResponseLogFields
    {
        public bool StatusCode { get; set; }
        public bool ContentType { get; set; }
        public bool Headers { get; set; }
        public bool Body { get; set; }
    }
}