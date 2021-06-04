using System.ComponentModel.DataAnnotations;

namespace MockService.Dtos.Input
{
    public class Mock
    {
        [Required]
        public Request Request { get; set; }
        [Required]
        public Response Response { get; set; }
    }
}
