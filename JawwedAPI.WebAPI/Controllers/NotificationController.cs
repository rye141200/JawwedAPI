using System;
using System.Security.Claims;
using FirebaseAdmin.Messaging;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Extensions;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    // register or refresh FCM token
    [HttpPost("register-device")]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest request)
    {
        await notificationService.RegisterDeviceAsync(User.GetUserId(), request.DeviceToken);
        return Ok();
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleNotifications(
        [FromBody] ToggleNotificationsRequest request
    )
    {
        await notificationService.ToggleNotificationsAsync(User.GetUserId(), request.Enable);
        return Ok();
    }
}
