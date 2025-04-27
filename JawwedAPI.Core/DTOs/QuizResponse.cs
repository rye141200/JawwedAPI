using System;

namespace JawwedAPI.Core.DTOs;

public class QuizResponse
{
    public required string QuizSessionID { get; set; }
    public required List<QuestionResponse> Questions { get; set; }
}

public class QuestionResponse
{
    public int QuestionID { get; set; }
    public string QuestionHeader { get; set; } = string.Empty;
    public required List<string> Options { get; set; }
}
