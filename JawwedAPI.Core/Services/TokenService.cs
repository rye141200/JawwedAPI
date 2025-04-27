using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JawwedAPI.Core.Services;

//TODO Refactor to make encapsulate common logic
public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
{
    public string GenerateQuizSessionToken(string email, long expirationMinutes)
    {
        //!1) Config extraction
        var key = Encoding.ASCII.GetBytes(jwtOptions.Value.Key);
        if (key.Length < 64)
            throw new ArgumentException("Key length must be at least 64 characters!");

        //!2) Claims
        var claims = new List<Claim> { new(ClaimTypes.Email, email) };

        //!3) Security token descriptor
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = new ClaimsIdentity(claims),
            // Set expires to be at least one second after NotBefore
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
        };

        //!4) Token handler
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateToken(Guid userId, string email, string? name, string role)
    {
        //!1) Config extraction
        var key = Encoding.ASCII.GetBytes(jwtOptions.Value.Key);
        if (key.Length < 64)
            throw new ArgumentException("Key length must be at least 64 characters!");

        var expirationMinutes = jwtOptions.Value.ExpirationMinutes;

        //!2) Claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, name ?? ""),
            new(ClaimTypes.Role, role),
        };
        if (name != null)
            claims.Add(new(ClaimTypes.Name, name));

        //!3) Security token descriptor
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = new ClaimsIdentity(claims),
            // Set expires to be at least one second after NotBefore
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
        };

        //!4) Token handler
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
