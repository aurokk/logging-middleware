namespace Aurokk.LoggingMiddleware.Settings
{
    public class ResponseLogFieldsSettings
    {
        public RequestLogFields Request { get; set; } = new RequestLogFields();
        public ResponseLogFields Response { get; set; } = new ResponseLogFields();
    }
}