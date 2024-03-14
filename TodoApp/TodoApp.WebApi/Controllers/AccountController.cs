using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Model.Entities;
using TodoApp.WebApi.DTO;

namespace TodoApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        public AccountController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<AccountController> logger,
            IConfiguration configuration)
        {

            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("UserRegister")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(userDto == null)
            {
                return BadRequest();
            }

            try
            {

                var user = _mapper.Map<ApplicationUser>(userDto);

                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                        return BadRequest(ModelState);
                    }
                }

                await _userManager.AddToRoleAsync(user, "User");

            }catch (Exception ex) {

                _logger.LogError(ex, $"Error {nameof(Register)} with user {userDto.UserName} ");
                return Problem(ex.ToString());
            }

            return Ok();
        }


        [HttpPost]
        [Route("UserLogin")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (loginDto == null)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);

                if (user == null)
                {
                    return Unauthorized(loginDto);
                }

                var checkPassword = await _userManager.CheckPasswordAsync(user,loginDto.Password);

                if (!checkPassword)
                {
                    return Unauthorized(loginDto);
                }

                string token = await GenerateToken(user);
                var response = new LoginResponseDto
                {
                    UserName = user.UserName,
                    TokenString = token,
                    UserId = user.Id
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error {nameof(Login)} with user {loginDto.UserName} ");
                return Problem(ex.ToString());
            }

            return Ok();
        }


        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jWTSetting:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["jWTSetting:Issuer"],
                audience: _configuration["jWTSetting:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["jWTSetting:Duration"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
