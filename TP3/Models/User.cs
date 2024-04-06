using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;

namespace TP4.Models
{
    public class User : IdentityUser
    {
        public virtual List<Score> Scores { get; set; } = null!;
    }
}
