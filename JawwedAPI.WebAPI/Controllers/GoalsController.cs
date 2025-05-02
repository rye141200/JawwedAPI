using System.Security.Claims;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.NotificationInterfaces;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Authorize]
public class GoalsController(IGoalsService goalsService, INotificationService notificationService)
    : CustomBaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateGoal(CreateGoalRequest createGoalRequest)
    {
        if (createGoalRequest is null)
            return BadRequest("goal cannot be null");
        createGoalRequest.UserId = Guid.Parse(
            HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );
        await goalsService.CreateGoalAsync(createGoalRequest);
        return Created();
    }

    [HttpGet("{goalId}")]
    public async Task<IActionResult> GetGoalById(Guid goalId)
    {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        return Ok(await goalsService.GetGoalByIdAsync(goalId, Guid.Parse(userId)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals()
    {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        return Ok(await goalsService.GetAllUserGoalsAsync(Guid.Parse(userId)));
    }

    [HttpPut("{goalId}")]
    public async Task<IActionResult> UpdateGoal(Guid goalId, UpdateGoalRequest updateGoalRequest)
    {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var goalResponse = await goalsService.UpdateGoalAsync(
            updateGoalRequest,
            goalId,
            Guid.Parse(userId)
        );
        var completedSessions = goalResponse
            .ReadingSchedule.Where(s => s.Status == ReadingSessionStatus.Completed.ToString())
            .ToList();

        await notificationService.DeleteScheduledJobs(
            Guid.Parse(userId),
            goalId,
            completedSessions
        );
        return Ok();
    }

    [HttpDelete("{goalId}")]
    public async Task<IActionResult> DeleteGoal(Guid goalId)
    {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        await goalsService.DeleteGoalAsync(goalId, Guid.Parse(userId));
        var goalResponse = await goalsService.GetGoalByIdAsync(goalId, Guid.Parse(userId));
        await notificationService.DeleteScheduledJobs(
            Guid.Parse(userId),
            goalId,
            goalResponse.ReadingSchedule
        );
        return Ok();
    }
}
