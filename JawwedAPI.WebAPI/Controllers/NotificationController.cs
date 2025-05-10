using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(
    IGenericRepository<ApplicationUser> userRepo,
    INotificationService notificationService,
    IGoalsService goalsService
) : CustomBaseController
{
    // register or refresh FCM token
    [HttpPost("register-device")]
    public async Task<IActionResult> RegisterDeviceToken(
        [FromBody] RegisterDeviceRequest notificationRequest
    )
    {
        // extract deviceToken
        if (string.IsNullOrWhiteSpace(notificationRequest.DeviceToken))
            return BadRequest("deviceToken is required.");

        // get userId from JWT
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idClaim, out var userId))
            return Unauthorized();

        // load and update user
        var user = await userRepo.GetOne(userId);
        if (user is null)
            return NotFound("User not found");

        user.DeviceToken = notificationRequest.DeviceToken;
        user.EnableNotifications = true;

        userRepo.Update(user);
        await userRepo.SaveChangesAsync();

        return Ok();
    }
}
