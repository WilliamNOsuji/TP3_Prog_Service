using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using TP4.Data;
using TP4.Models;

namespace TP4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly UserManager<User> UserManager;

        private readonly TP4Context _context;

        public UsersController(UserManager<User> userManager)
        {
            this.UserManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            if(register.Password != register.PasswordConfirm)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Message = "Les deux mots de passe spécifiés sont différents." });
            }

            User user = new User()
            {
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult identityResult = await this.UserManager.CreateAsync(user, register.Password);
            if(!identityResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Lac création de l'utilisateur a échoué." });
            }
            return Ok();
        }

        //[HttpPost]
        //public async Task<ActionResult<User>> Login(User user)
        //{
        //    if (_context.User != null)
        //    {
        //        return Problem("Entity set 'TP4Context.User' is null");
        //    }
        //    var userLog = _context.User.FindAsync(user.Id);
        //    if(userLog == null)
        //    {
        //        return Problem("User is not within the Dataset 'TP4Context.User'");
        //    }
        //
        //    return CreatedAtAction("Login", userLog);
        //}
    }
}
