using System.Security.Claims;
using Google.Apis.Auth;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Options;
using JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JawwedAPI.WebAPI.Controllers;

public class AuthController(IAuthService authService) : CustomBaseController
{
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        var token = await authService.LoginAndGenerateToken(request);
        return Ok(new { token = token });
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
}
