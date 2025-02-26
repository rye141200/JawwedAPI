using System;
using Google.Apis.Auth;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JawwedAPI.Core.Services;

public class AuthService(
    IOptions<GoogleAuthenticationOptions> googleOptions,
    ITokenService tokenService
) : IAuthService
{
    public async Task<string> LoginAndGenerateToken(GoogleLoginRequest request)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = [googleOptions.Value.ClientId],
            ExpirationTimeClockTolerance = TimeSpan.FromMinutes(1),
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        //TODO: Implement the logic to check if a user exists if not, create a new user

        var token = tokenService.GenerateToken("", payload.Email, payload.Name);
        return token;
    }
}
