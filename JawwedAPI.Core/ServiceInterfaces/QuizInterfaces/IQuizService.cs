using System;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;

public interface IQuizService
{
    Task<List<QuestionResponse>> GenerateQuizQuestions();
}
