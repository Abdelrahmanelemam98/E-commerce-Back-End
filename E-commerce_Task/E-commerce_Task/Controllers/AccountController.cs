using DomainLayer.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_commerce_Task.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _config;
		public AccountController(UserManager<IdentityUser> userManager, IConfiguration config)
		{
			_userManager= userManager;
			_config=config;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Registration(RegisterDto registerDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var user = new IdentityUser
					{
						UserName = registerDto.UserName,
						Email = registerDto.Email
					};

					var result = await _userManager.CreateAsync(user, registerDto.Password);

					if (result.Succeeded)
					{
						return Ok("Account Added Successfully");
					}
					else
					{
						// Handle multiple errors if there are more than one
						var errors = result.Errors.Select(e => e.Description);
						return BadRequest(string.Join(", ", errors));
					}
				}
				else
				{
					// Return a list of validation errors if the model state is not valid
					var modelErrors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage);

					return BadRequest(string.Join(", ", modelErrors));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserDto loginUserDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var user = await _userManager.FindByNameAsync(loginUserDto.UserName);

					if (user != null)
					{
						var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

						if (isPasswordValid)
						{
							// Create claims for the user
							var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, user.UserName),
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					};

							// Get user roles and add them as claims
							var roles = await _userManager.GetRolesAsync(user);
							foreach (var roleName in roles)
							{
								claims.Add(new Claim(ClaimTypes.Role, roleName));
							}

							// Create a JWT token
							var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
							var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
							var token = new JwtSecurityToken(
								issuer: _config["JWT:ValidIssuer"],
								audience: _config["JWT:ValidAudience"],
								claims: claims,
								expires: DateTime.Now.AddHours(1),
								signingCredentials: signingCredentials
							);

							return Ok(new
							{
								token = new JwtSecurityTokenHandler().WriteToken(token),
								expiration = token.ValidTo
							});
						}
					}

					// If the user is not found or password is invalid, return Unauthorized
					return Unauthorized();
				}
				else
				{
					// If ModelState is not valid, return BadRequest with validation errors
					var modelErrors = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage);

					return BadRequest(string.Join(", ", modelErrors));
				}
			}
			catch (Exception ex)
			{
				// Handle exceptions and log them
				return BadRequest(ex.Message);
			}
		}


	}
}
