using Doc_Patient_Backend.Models.DTOs;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (succeeded, errorMessage) = await _authService.RegisterAsync(registerDto);

            if (succeeded)
            {
                return Ok(new { Message = "User registered successfully." });
            }

            return BadRequest(new { Message = errorMessage });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (token, errorMessage) = await _authService.LoginAsync(loginDto);

            if (token != null)
            {
                return Ok(new LoginResponseDto { Token = token });
            }

            return Unauthorized(new { Message = errorMessage });
        }
        
        // GET: api/auth/me
        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Me()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var roles = new List<string>();
            foreach (var claim in User.FindAll(System.Security.Claims.ClaimTypes.Role))
            {
                roles.Add(claim.Value);
            }

            return Ok(new { id, email, roles });
        }
    }
}
