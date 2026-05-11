using GeoQuiz.Models;

namespace GeoQuiz.Services;

public interface IQuizService
{
    List<QuizQuestion> GenerateQuiz(List<CountryDto> countries, int questionCount, QuizType type, string? regionFilter = null);
    List<string> ShuffleOptions(List<string> options);
}

public class QuizService : IQuizService
{
    private readonly Random random = new();

    public List<QuizQuestion> GenerateQuiz(List<CountryDto> countries, int questionCount, QuizType type, string? regionFilter = null)
    {
        var filteredCountries = string.IsNullOrEmpty(regionFilter)
            ? countries
            : countries.Where(c => c.Region.Equals(regionFilter, StringComparison.OrdinalIgnoreCase)).ToList();

        if (filteredCountries.Count < 4)
        {
            throw new Exception("Not enough countries to generate quiz. Need at least 4.");
        }

        var questions = new List<QuizQuestion>();
        var shuffledCountries = filteredCountries.OrderBy(_ => random.Next()).Take(questionCount).ToList();

        foreach (var country in shuffledCountries)
        {
            var question = type switch
            {
                QuizType.Capital => GenerateCapitalQuestion(country, filteredCountries),
                QuizType.Flag => GenerateFlagQuestion(country, filteredCountries),
                QuizType.Continent => GenerateContinentQuestion(country, filteredCountries),
                QuizType.Language => GenerateLanguageQuestion(country, filteredCountries),
                _ => GenerateCapitalQuestion(country, filteredCountries)
            };

            questions.Add(question);
        }

        return questions;
    }

    private QuizQuestion GenerateCapitalQuestion(CountryDto country, List<CountryDto> allCountries)
    {
        var correctAnswer = country.CapitalCity ?? "N/A";
        var wrongAnswers = allCountries
            .Where(c => c.CapitalCity != null && c.CapitalCity != correctAnswer)
            .Select(c => c.CapitalCity!)
            .OrderBy(_ => random.Next())
            .Take(3)
            .ToList();

        var options = ShuffleOptions(new List<string> { correctAnswer }.Concat(wrongAnswers).ToList());

        return new QuizQuestion
        {
            QuestionText = $"What is the capital of {country.CommonName}?",
            Options = options,
            CorrectAnswer = correctAnswer,
            Type = QuizType.Capital,
            Country = country
        };
    }

    private QuizQuestion GenerateFlagQuestion(CountryDto country, List<CountryDto> allCountries)
    {
        var correctAnswer = country.CommonName;
        var wrongAnswers = allCountries
            .Where(c => c.CommonName != correctAnswer)
            .Select(c => c.CommonName)
            .OrderBy(_ => random.Next())
            .Take(3)
            .ToList();

        var options = ShuffleOptions(new List<string> { correctAnswer }.Concat(wrongAnswers).ToList());

        return new QuizQuestion
        {
            QuestionText = "Which country does this flag belong to?",
            Options = options,
            CorrectAnswer = correctAnswer,
            Type = QuizType.Flag,
            Country = country
        };
    }

    private QuizQuestion GenerateContinentQuestion(CountryDto country, List<CountryDto> allCountries)
    {
        var correctAnswer = country.Continents?.FirstOrDefault() ?? country.Region;
        var continents = new List<string> { "Africa", "Asia", "Europe", "North America", "South America", "Oceania", "Antarctica" };
        var wrongAnswers = continents
            .Where(c => c != correctAnswer)
            .OrderBy(_ => random.Next())
            .Take(3)
            .ToList();

        var options = ShuffleOptions(new List<string> { correctAnswer }.Concat(wrongAnswers).ToList());

        return new QuizQuestion
        {
            QuestionText = $"Which continent does {country.CommonName} belong to?",
            Options = options,
            CorrectAnswer = correctAnswer,
            Type = QuizType.Continent,
            Country = country
        };
    }

    private QuizQuestion GenerateLanguageQuestion(CountryDto country, List<CountryDto> allCountries)
    {
        var correctAnswer = country.LanguagesList;
        var wrongAnswers = allCountries
            .Where(c => c.LanguagesList != correctAnswer && c.LanguagesList != "N/A")
            .Select(c => c.LanguagesList)
            .Distinct()
            .OrderBy(_ => random.Next())
            .Take(3)
            .ToList();

        var options = ShuffleOptions(new List<string> { correctAnswer }.Concat(wrongAnswers).ToList());

        return new QuizQuestion
        {
            QuestionText = $"What is the main language(s) of {country.CommonName}?",
            Options = options,
            CorrectAnswer = correctAnswer,
            Type = QuizType.Language,
            Country = country
        };
    }

    public List<string> ShuffleOptions(List<string> options)
    {
        return options.OrderBy(_ => random.Next()).ToList();
    }
}