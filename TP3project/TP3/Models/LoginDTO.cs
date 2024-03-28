using Microsoft.Build.Framework;

namespace TP4.Models
{
    public class LoginDTO
    {
        [Required]
        public String Username { get; set; } = null!;
        [Required]
        public String Password { get; set; } = null!; 
    }
}
