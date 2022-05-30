using DisneyAPI.Models;
using DisneyAPI.ViewModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //REGISTRO
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            // Verificar si existe el usuario
            var list = await _userManager.FindByNameAsync(model.Username);

            // Si existe, devolver error
            if (list != null)
                return BadRequest();

            // Si no existe, registrar usuario
            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };

            var userCreated = await _userManager.CreateAsync(user, model.Password);

            if (!userCreated.Succeeded)
                return StatusCode(500, new ResponseData {
                    Status = "Error",
                    Message = $"No se pudo crear el usuario! Error: {userCreated.Errors.Select(e => e.Description)}"
                });

            return StatusCode(201, new ResponseData
            {
                Status = "Created",
                Message = "Usuario creado!"
            });
        }

        //LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model) 
        {
            //Chequeo existencia de usuario
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            //Chequeo coincidencia de contraseña
            if (result.Succeeded) {
                var currentUser = await _userManager.FindByNameAsync(model.Username);

                if (currentUser.IsActive) 
                {
                    return Ok(await GetToken(currentUser));
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseData { Status = "Error", Message = "Usted no está autorizado." });
        }

        private async Task<LoginResponse> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(u => new Claim(ClaimTypes.Role, u)));

            var authSigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySuperLargaAUTORIZADORAdeUsuarios"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigingKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponse {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
}
