using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> manager, ITokenRepository tokenRepository) : ControllerBase {
        private readonly UserManager<IdentityUser> manager = manager;
        private readonly ITokenRepository tokenRepository = tokenRepository;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
            IdentityUser user = new() {
                UserName = registerDto.Username,
                Email = registerDto.Username,
            };

            IdentityResult creationResult = await manager.CreateAsync(user, registerDto.Password);

            if (
                creationResult.Succeeded &&
                registerDto.Roles != null &&
                registerDto.Roles.Length != 0
            ) {
                IdentityResult addRolesResult = await manager.AddToRolesAsync(
                    user, registerDto.Roles
                );

                if (addRolesResult.Succeeded) Ok(user);

            }

            return BadRequest("Error registering the user");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            string msg = "Error logging in, please try again or with different credentials";

            IdentityUser? user = await manager.FindByEmailAsync(loginDto.Username);

            if (user == null) return BadRequest(msg);

            bool validPassword = await manager.CheckPasswordAsync(user, loginDto.Password);

            if (validPassword) {
                // get roles for toke method
                var roles = await manager.GetRolesAsync(user);

                // generate jwt
                string? token = tokenRepository.CreateToken(user, [.. roles]);

                if (!token.Equals("")) {
                    LoginResponseDto response = new() {
                        Id = user.Id,
                        UserName = user.UserName ?? "No username provided",
                        Token = token,
                    };

                    return Ok(response);
                }
            };

            return BadRequest(msg);
        }

    }
}