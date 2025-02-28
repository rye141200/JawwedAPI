using System.Security.Claims;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.AuthenticationInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
