using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;

public interface IQuizService
{
    Task<List<QuestionResponse>> GenerateQuizQuestions();
    Task<(int, int)> EvaluateQuizScoreAsync(List<QuestionSubmitRequest> submittedQuestions);
    Task<QuizSubmitRequest> Cheat(QuizResponse quizResponse);
}
