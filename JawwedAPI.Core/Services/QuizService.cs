using System;
using System.Threading.Tasks;
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.Domain.RepositoryInterfaces;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuizInterfaces;

namespace JawwedAPI.Core.Services;

public class QuizService(
    IGenericRepository<Question> questionsRepository,
    ICacheService memoryCacheService
) : IQuizService
{
    private const string QuestionsCacheKey = "quiz:all_questions";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public async Task<List<QuestionResponse>> GenerateQuizQuestions()
    {
        //!1) Retrieve random questions (4 easy, 4 medium, 4 hard)
        var random = new Random();

        //?1.1 Replace with cache service
        var questions = await memoryCacheService.GetOrCreateAsync<List<Question>>(
            QuestionsCacheKey,
            async () => [.. await questionsRepository.GetAll()],
            CacheDuration
        );

        //?1.2 Extract the questions based on category
        var easyQuestions = questions.Where(question => question.Difficulty == 1).ToList();
        var mediumQuestions = questions.Where(question => question.Difficulty == 2).ToList();
        var hardQuestions = questions.Where(question => question.Difficulty == 3).ToList();

        //?1.3 Randomly select the questions
        var quizQuestions = new List<Question>();
        quizQuestions.AddRange(SelectRandomQuestions(easyQuestions, 4, random));
        quizQuestions.AddRange(SelectRandomQuestions(mediumQuestions, 4, random));
        quizQuestions.AddRange(SelectRandomQuestions(hardQuestions, 4, random));

        //!2) Convert to QuestionResponse
        return [.. quizQuestions.Select(question => question.ToQuestionResponse())];
    }

    List<Question> SelectRandomQuestions(List<Question> questions, int count, Random random)
    {
        if (questions.Count <= count)
            return questions;

        List<Question> randomQuestions = [];
        var selectedIndices = new HashSet<int>();

        while (selectedIndices.Count < count)
        {
            int index = random.Next(0, questions.Count);
            selectedIndices.Add(index);
        }

        // now build the result list
        foreach (int i in selectedIndices)
            randomQuestions.Add(questions[i]);

        return randomQuestions;
    }
}
