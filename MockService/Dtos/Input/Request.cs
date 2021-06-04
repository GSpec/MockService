using System.ComponentModel.DataAnnotations;

namespace MockService.Dtos.Input
{
    public class Request
    {
        [Required]
        public string Method { get; set; }
        [Required]
        public string Endpoint { get; set; }
    }
}
