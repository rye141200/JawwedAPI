using System;

namespace JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;

public interface ITokenService
{
    Task<string?> ExtractClaimFromToken(string token, string claimType);
    Task<bool> IsValidQuizSessionToken(string token);
    string GenerateToken(Guid userId, string email, string? name, string role);
    string GenerateQuizSessionToken(string email, long expirationMinutes);
}
