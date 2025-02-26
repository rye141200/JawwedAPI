using System;

namespace JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;

public interface ITokenService
{
    string GenerateToken(string userId, string email, string? name);
}
