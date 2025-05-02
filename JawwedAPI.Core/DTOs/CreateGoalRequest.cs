using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JawwedAPI.Core.DTOs;

public class CreateGoalRequest
{
    [BindNever]
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "title not found")]
    public string Title { get; set; }

    [Required(ErrorMessage = "duration days not found")]
    public int DurationDays { get; set; }

    [Required(ErrorMessage = "total pages not found")]
    public int TotalPages { get; set; }

    [Required(ErrorMessage = "start page not found")]
    public int StartPage { get; set; }

    [Required(ErrorMessage = "Remainder time not found")]
    public TimeSpan ReminderTime { get; set; }
}
