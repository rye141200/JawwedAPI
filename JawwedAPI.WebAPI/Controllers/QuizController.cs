using System.Security.Claims;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.Exceptions.CustomExceptions;
using JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;
using JawwedAPI.Core.ServiceInterfaces.TokenInterfaces;
using JawwedAPI.Core.ServiceInterfaces.UserInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

[Authorize]
public class QuizController(
    IQuizService quizService,
    ITokenService tokenService,
    IUserService userService
) : CustomBaseController
{
    [HttpGet]
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

    [HttpPost("submit")]
    public async Task<IActionResult> ValidateQuiz([FromBody] QuizSubmitRequest quizSubmitRequest)
    {
        //!1) Validate quiz session ID
        var userEmail = GetUserEmail();
        if (
            !await tokenService.IsValidQuizSessionToken(quizSubmitRequest.QuizSessionID)
            || userEmail == null
            || userEmail
                != await tokenService.ExtractClaimFromToken(
                    quizSubmitRequest.QuizSessionID,
                    ClaimTypes.Email
                )
        )
            return Unauthorized("Invalid quiz session ID");

        //!2) Evaluate the score
        var scores = (
            await quizService.EvaluateQuizScoreAsync(quizSubmitRequest.AnsweredQuestions)
        ).ToTuple();

        //!3) Change user role
        if (scores.Item1 / scores.Item2 >= 0.95)
        {
            await userService.UpgradeUserToPremiumAsync(userEmail);
            return Ok(
                new
                {
                    message = "Congrats for getting premium, well done and enjoy memorizing The Holy Quran 💖",
                    passed = false,
                }
            );
        }
        return Ok(
            new
            {
                message = "You did not make it this time, try harder next time and good luck!",
                passed = false,
            }
        );
    }

    /* [HttpPost("Cheat")]
    public async Task<IActionResult> CheatAndGetAnswers(QuizResponse quizResponse) =>
        Ok(await quizService.Cheat(quizResponse)); */

    private string? GetUserEmail() =>
        User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
}
