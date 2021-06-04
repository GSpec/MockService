using MockLogic.Models.Enums;
using System;

namespace MockLogic.Models
{
    public class Mock
    {
        public Guid Id { get; set; }
        public Method Method { get; set; }
        public string Endpoint { get; set; }
        public string RequestBody { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseBody { get; set; }
    }
}