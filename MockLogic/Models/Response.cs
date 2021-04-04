namespace MockLogic.Models
{
    public class Response
    {
        const string DefaultHeaders = "{\"Content-Type\": \"application/json\"}";

        private string _headers;
        public string Headers
        {
            get
            {
                if (string.IsNullOrEmpty(_headers))
                {
                    return DefaultHeaders;
                }

                return _headers;
            }
            set
            {
                _headers = value;
            }
        }

        public string Body { get; set; }
    }
}
