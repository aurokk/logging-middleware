namespace Aurokk.LoggingMiddleware.Settings
{
    public class LoggingMiddlewareSettings
    {
        public string AllFieldsNamespace { get; set; } = "HttpContext";

        public string RequestFieldsNamespace { get; set; } = "Request";
        public string RequestFieldsPrefix { get; set; } = string.Empty;

        public string ResponseFieldsNamespace { get; set; } = "Response";
        public string ResponseFieldsPrefix { get; set; } = string.Empty;

        public bool AddRequestLog { get; set; }
        public RequestLogFieldsSettings RequestLogFields { get; set; } = new RequestLogFieldsSettings();

        public bool AddResponseLog { get; set; }
        public ResponseLogFieldsSettings ResponseLogFields { get; set; } = new ResponseLogFieldsSettings();

        public bool AddContext { get; set; }
        public ContextFieldsSettings ContextFields { get; set; } = new ContextFieldsSettings();
    }
}