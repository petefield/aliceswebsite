using AlicesWebsite.Shared.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/accounts")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AccountsController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager; 
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequest userForRegistration)
    {
        if (userForRegistration == null || !ModelState.IsValid)
            return BadRequest();

        var user = new IdentityUser(userForRegistration.Email);

        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new RegistrationResponse { Errors = errors }); 
        }

        await _userManager.AddClaimsAsync(user, new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Surname, userForRegistration.LastName),
                new Claim(ClaimTypes.GivenName, userForRegistration.FirstName),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        });


        return StatusCode(201);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult> LoginUserAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.UserName);

        if (user != null)
        {
            var validCredentials = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (validCredentials)
            {
                return Ok(await GetLoginResult(user));
            }
        }
        return new UnauthorizedResult();
    }

    [HttpGet("users")]
    public async Task<IEnumerable<User>> Users()
    {
        
        var users = await _userManager.Users.ToListAsync();

        List<User> result = new List<User>();

        foreach (var u in users)
        {
            var claims = await _userManager.GetClaimsAsync(u);

            var user =  new User(
                u.UserName ?? string.Empty,
                u.Email ?? string.Empty,
                claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "-",
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? "-");
            result.Add(user);
        }       

        return result;
    }

    private async Task<LoginResponse> GetLoginResult(IdentityUser user)
    {
        DateTime dtExpire = DateTime.UtcNow.AddMinutes(10);
        return new LoginResponse()
        {
            UserId = user.Id,
            ExpirationDate = dtExpire,
            Token = await GenerateJwtToken(user, dtExpire)
        };
    }

    private async Task<string> GenerateJwtToken(IdentityUser user, DateTime expry)
    {
        try
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes("super secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "fieldfamilylauughs",
                Issuer = "net.the-fields",
                Subject = new ClaimsIdentity(claims),
                Expires = expry,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return string.Empty;
        }
    }
}