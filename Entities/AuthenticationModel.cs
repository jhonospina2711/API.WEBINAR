using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class AuthenticationModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}