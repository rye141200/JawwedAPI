using System;
using System.Collections.Generic;
using AutoMapper;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.Enums;
using JawwedAPI.Core.DTOs;

namespace JawwedAPI.Core.Mapper;

/// <summary>
/// Resolver for generating reading schedule sessions for a Goal
/// </summary>
public class GoalResponseResolver : IValueResolver<Goal, GoalResponse, List<ReadingSessionResponse>>
{
    public List<ReadingSessionResponse> Resolve(
        Goal source,
        GoalResponse destination,
        List<ReadingSessionResponse> destMember,
        ResolutionContext context
    )
    {
        // Calculate pages per day, ensuring at least one page per day
        int pagesPerDay = Math.Max(1, source.TotalPages / source.DurationDays);
        int remainingPages = source.TotalPages % source.DurationDays;

        // Calculate how many full days have been completed based on ActualPagesRead
        int completedDays = source.ActualPagesRead / pagesPerDay; // 10
        int inProgressPages = source.ActualPagesRead % pagesPerDay; // 0

        // Cap completedDays to the duration of the goal
        completedDays = Math.Min(completedDays, source.DurationDays);

        var readingSchedule = new List<ReadingSessionResponse>();

        for (int day = 1; day <= source.DurationDays; day++)
        {
            // Calculate start and end page, handling any remainder pages
            int startPage = source.StartPage + (day - 1) * pagesPerDay;
            int endPage;

            if (day == source.DurationDays)
                endPage = startPage + pagesPerDay - 1 + remainingPages;
            else
                endPage = startPage + pagesPerDay - 1;

            DateTime scheduledDate = source.StartDate.AddDays(day - 1);

            var session = new ReadingSessionResponse
            {
                DayNumber = day,
                ScheduledDate = scheduledDate,
                StartPage = startPage,
                EndPage = endPage,
                Status = ReadingSessionStatus.NotStarted.ToString(), // Default status
            };

            // Set status based on completion state
            if (day <= completedDays)
            {
                // This day is fully completed
                session.Status = ReadingSessionStatus.Completed.ToString();
                session.ActualPagesRead = pagesPerDay;
            }
            else if (day == completedDays + 1 && inProgressPages >= 0)
            {
                // This day is partially completed
                session.Status = ReadingSessionStatus.InProgress.ToString();
                session.ActualPagesRead = inProgressPages;
            }
            else if (scheduledDate.Date < DateTime.Today.AddDays(3.0))
            {
                session.Status = ReadingSessionStatus.Missed.ToString();
            }

            readingSchedule.Add(session);
        }

        return readingSchedule;
    }
}
