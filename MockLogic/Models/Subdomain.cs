using System.ComponentModel.DataAnnotations;

namespace MockLogic.Models
{
    public class Subdomain
    {
        [Key]
        public string Name { get; set; }
    }
}