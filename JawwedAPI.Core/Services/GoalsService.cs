using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.Jobs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

namespace JawwedAPI.Core.Services;

public class GoalsService(
    IMapper mapper,
    IGenericRepository<Goal> goalRepository,
    PushNotificationJob notificationJob
) : IGoalsService
{
    public async Task CreateGoalAsync(CreateGoalRequest request)
    {
        Goal goal = mapper.Map<Goal>(request);
        //! 2) create the goal
        await goalRepository.Create(goal);
        await goalRepository.SaveChangesAsync();
        //!  1)Schedule notifications for the new goal
        await notificationJob.ScheduleNotificationsForGoalAsync(request.UserId!.Value, goal.GoalId);
    }

    public async Task<GoalResponse> GetGoalByIdAsync(Guid goalId, Guid userId)
    {
        Goal? goal =
            await goalRepository.FindOne(u => u.UserId == userId && u.GoalId == goalId)
            ?? throw new GlobalErrorThrower(401, "Not found", "goal not found for specific user");

        GoalResponse goalResponse = mapper.Map<GoalResponse>(goal);
        return goalResponse;
    }

    public async Task<List<GoalResponse>> GetAllUserGoalsAsync(Guid userId)
    {
        IEnumerable<Goal> goals = await goalRepository.Find(u => u.UserId == userId);

        return goals.Select(mapper.Map<GoalResponse>).ToList();
    }

    public async Task<GoalResponse> UpdateGoalAsync(
        UpdateGoalRequest request,
        Guid goalId,
        Guid userId
    )
    {
        Goal? goal =
            await goalRepository.FindOne(u => u.UserId == userId && u.GoalId == goalId)
            ?? throw new GlobalErrorThrower(401, "Not found", "goal not found for specific user");
        int totalPagesRead = request.LastPageRead - goal.StartPage + 1;
        if (totalPagesRead < goal.ActualPagesRead)
            throw new GlobalErrorThrower(401, "invalid update data", "last page read is invalid");
        goal.ActualPagesRead = totalPagesRead;
        goal.LastVerseKeyRead = request.LastVerseKeyRead;
        if (goal.ActualPagesRead == goal.TotalPages)
        {
            goal.Status = GoalStatus.Completed;
            // Check and clean up jobs for completed goal
            await notificationJob.CheckAndUpdateGoalStatusAsync(userId, goalId);
        }
        await goalRepository.SaveChangesAsync();
        return mapper.Map<GoalResponse>(goal);
    }

    public async Task DeleteGoalAsync(Guid goalId, Guid userId)
    {
        Goal? goal =
            await goalRepository.FindOne(u => u.UserId == userId && u.GoalId == goalId)
            ?? throw new GlobalErrorThrower(401, "Not found", "goal not found for specific user");
        if (goal.Status == GoalStatus.Completed)
            return;
        goal.Status = GoalStatus.Canceled;
        goalRepository.Update(goal);
        await goalRepository.SaveChangesAsync();

        // Delete scheduled notifications for this goal
        await notificationJob.DeleteScheduledNotificationsAsync(userId, goalId);
    }
}
