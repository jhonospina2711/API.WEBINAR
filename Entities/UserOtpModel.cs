using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class UserOtpModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Otp { get; set; }
    }
}