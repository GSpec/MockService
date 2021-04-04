using MockLogic.Models.Enums;
using System;

namespace MockLogic.Models
{
    public struct Request : IEquatable<Request>
    {
        public Method Method { get; set; }
        public string Endpoint { get; set; }
        public string Body { get; set; }

        public bool Equals(Request request)
        {
            return
                Method == request.Method &&
                Endpoint == request.Endpoint &&
                Body == request.Body;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37;

                result *= 397;
                result += Method.GetHashCode();

                result *= 397;
                if (Endpoint != null)
                    result += Endpoint.GetHashCode();

                result *= 397;
                if (Body != null)
                    result += Body.GetHashCode();

                return result;
            }
        }
    }
}