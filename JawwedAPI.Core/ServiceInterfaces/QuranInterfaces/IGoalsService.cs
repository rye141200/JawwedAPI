using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;

/// <summary>
/// Interface for managing Quran reading goals and tracking progress
/// </summary>
public interface IGoalsService
{
    Task CreateGoalAsync(CreateGoalRequest request);
    Task<GoalResponse> UpdateGoalAsync(UpdateGoalRequest request, Guid goalId, Guid userId);

    Task<GoalResponse> GetGoalByIdAsync(Guid goalId, Guid userId);
    Task<List<GoalResponse>> GetAllUserGoalsAsync(Guid userId);
    Task DeleteGoalAsync(Guid goalId, Guid userId);
}
