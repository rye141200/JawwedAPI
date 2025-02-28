using System;
using System.Text.Json;
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
        PrintExpirationTime(request);
        // Disable most validations for now to isolate the issue
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = [googleOptions.Value.ClientId],
            HostedDomain = null,
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        // Generate our token
        var token = tokenService.GenerateToken(payload.Subject, payload.Email, payload.Name);
        return token;
    }

    private void PrintExpirationTime(GoogleLoginRequest request)
    {
        // For debugging - log the current time to see if there's a mismatch
        Console.WriteLine($"Server UTC time: {DateTime.UtcNow}");
        Console.WriteLine($"Server local time: {DateTime.Now}");

        // Parse the token without validation to debug
        var tokenParts = request.IdToken.Split('.');
        if (tokenParts.Length >= 2)
        {
            var base64Payload = tokenParts[1];
            base64Payload = base64Payload.Replace('-', '+').Replace('_', '/');
            switch (base64Payload.Length % 4)
            {
                case 2:
                    base64Payload += "==";
                    break;
                case 3:
                    base64Payload += "=";
                    break;
            }
            var jsonPayload = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(base64Payload)
            );

            // Parse the JSON payload to extract the expiration date
            try
            {
                var payloadObj = System.Text.Json.JsonSerializer.Deserialize<
                    Dictionary<string, JsonElement>
                >(jsonPayload);

                if (payloadObj != null && payloadObj.ContainsKey("exp"))
                {
                    // Get the JsonElement and convert it properly
                    var expElement = payloadObj["exp"];
                    long expUnixTime = expElement.GetInt64(); // Use JsonElement's GetInt64() method

                    var tokenExpirationUtc = DateTimeOffset
                        .FromUnixTimeSeconds(expUnixTime)
                        .UtcDateTime;
                    var tokenExpirationLocal = tokenExpirationUtc.ToLocalTime();

                    Console.WriteLine($"Token expires at (UTC): {tokenExpirationUtc}");
                    Console.WriteLine($"Token expires at (Local): {tokenExpirationLocal}");
                    Console.WriteLine(
                        $"Time until expiration: {tokenExpirationUtc - DateTime.UtcNow}"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing token expiration: {ex.Message}");
            }
        }
    }
}

public class UtcSystemClock : Google.Apis.Util.IClock
{
    public DateTime Now => DateTime.UtcNow;
    public DateTime UtcNow => DateTime.UtcNow;
}
