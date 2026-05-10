namespace GeoQuiz.Models;

public class QuizQuestion
{
    public string QuestionText { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public string CorrectAnswer { get; set; } = string.Empty;
    public QuizType Type { get; set; }
    public CountryDto? Country { get; set; }
}

public enum QuizType
{
    Capital,
    Flag,
    Continent,
    Language
}

public class QuizSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public string? FilterRegion { get; set; }
    public QuizType QuizType { get; set; }

    public double ScorePercentage => TotalQuestions > 0 
        ? (double)CorrectAnswers / TotalQuestions * 100 
        : 0;

    public TimeSpan Duration => EndTime.HasValue 
        ? EndTime.Value - StartTime 
        : DateTime.Now - StartTime;
}

public enum QuizMode
{
    Study,
    Test
}