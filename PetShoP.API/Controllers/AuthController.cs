using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Contract;
using PetShop.Application.DtosUser;
namespace PetShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            if (!ModelState.IsValid || !await _authService.RegisterAsync(userDto)) return BadRequest("El correo electrónico ya está en uso.");
            return StatusCode(201, "Registro exitoso.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return response == null ? Unauthorized("Credenciales inválidas.") : Ok(response);
        }
    }
}