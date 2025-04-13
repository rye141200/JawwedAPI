using System.Security.Claims;
using Google.Apis.Auth;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JawwedAPI.WebAPI.Controllers;

public class AuthController(IAuthService authService, IOptions<JwtOptions> options)
    : CustomBaseController
{
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        AuthResponse response;
        try
        {
            response = await authService.GoogleLogin(request);
            HttpContext.Response.Cookies.Append(
                "JawwedAuthCookie",
                response.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpirationMinutes),
                }
            );
        }
        catch (Exception ex)
        {
            return Problem(
                ex.Message + "Stack trace: " + ex.StackTrace,
                statusCode: 400,
                title: "Invalid JWT"
            );
        }
        return Ok(response);
    }

    [Authorize]
    [HttpGet("user-info")]
    public IActionResult GetUserInfo() =>
        Ok(
            new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Name = User.FindFirstValue(ClaimTypes.Name),
            }
        );

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers() => Ok(await authService.GetAllUsers());
}
