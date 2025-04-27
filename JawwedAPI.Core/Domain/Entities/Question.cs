using System;
using System.ComponentModel.DataAnnotations;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.Domain.Entities;

public class Question
{
    [Key]
    public int QuestionID { get; set; }

    [Required]
    public string QuestionHeader { get; set; } = string.Empty;

    [Required]
    public string OptionA { get; set; } = string.Empty;

    [Required]
    public string OptionB { get; set; } = string.Empty;

    [Required]
    public string Answer { get; set; } = string.Empty;

    [Required]
    public int Difficulty { get; set; }

    public QuestionResponse ToQuestionResponse() =>
        new()
        {
            QuestionID = QuestionID,
            QuestionHeader = QuestionHeader,
            Options = [OptionA, Answer, OptionB],
        };
}
