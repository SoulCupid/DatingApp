using DatingApp.API.Entities;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        _userManager = userManager;
    }
    
    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            //anything added here will appear as a claim in the token
            //ex. if we a claim "Abc" with value "GDP" this will show on the claims if we decode the token
        };

        var userRoles = await _userManager.GetRolesAsync(user);

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenSigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = tokenSigningCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}