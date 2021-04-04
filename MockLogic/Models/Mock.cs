using MockLogic.Models.Enums;
using System;

namespace MockLogic.Models
{
    public class Mock
    {
        public Guid Reference { get; }
        public Request Request { get; }
        public Response Response { get; set; }

        public Mock(Method method, string endpoint, string responseHeaders, string responseBody)
        {
            Reference = Guid.NewGuid();
            Request = new Request { Method = method, Endpoint = endpoint };
            Response = new Response { Headers = responseHeaders, Body = responseBody };
        }
    }
}