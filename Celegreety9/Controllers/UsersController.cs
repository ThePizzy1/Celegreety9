using Celegreety9.Features.TalentPricings.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Celegreety9.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _users;

        public UsersController(IUserRepository users)
        {
            _users = users;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string name, string email)
        {
            var id = await _users.RegisterUserAsync(name, email);
            return Ok(new { Id = id });
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _users.GetByEmailAsync(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
