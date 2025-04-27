using System;

namespace JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;

public interface ITokenService
{
    string GenerateToken(Guid userId, string email, string? name, string role);
    string GenerateQuizSessionToken(string email, long expirationMinutes);
}
