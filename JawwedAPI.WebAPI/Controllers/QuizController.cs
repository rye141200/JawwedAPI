using System.Security.Claims;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

public class QuizController(IQuizService quizService, ITokenService tokenService)
    : CustomBaseController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GenerateQuiz()
    {
        //!1) Generate quiz questions without answers
        var questions = await quizService.GenerateQuizQuestions();

        //!2) Generate quiz token
        var userEmail =
            GetUserEmail() ?? throw new GlobalErrorThrower(404, "Email not found in user claims!");
        string quizSessionToken = tokenService.GenerateQuizSessionToken(userEmail, 12);

        return Ok(new QuizResponse() { Questions = questions, QuizSessionID = quizSessionToken });
    }

    private string? GetUserEmail() =>
        User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
}
