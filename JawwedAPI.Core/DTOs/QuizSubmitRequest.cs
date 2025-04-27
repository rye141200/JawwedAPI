using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.DTOs;

public class QuizSubmitRequest
{
    [Required]
    public required string QuizSessionID { get; set; }

    [Required]
    public required List<QuestionSubmitRequest> AnsweredQuestions { get; set; }
}

public class QuestionSubmitRequest
{
    [Required]
    public required string QuestionHeader { get; set; }

    [Required]
    public required string QuestionAnswer { get; set; }
}
