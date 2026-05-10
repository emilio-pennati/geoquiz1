namespace GeoQuiz.Models;

public class UserStats
{
    public int TotalQuizzes { get; set; }
    public int TotalQuestions { get; set; }
    public int TotalCorrect { get; set; }
    public double OverallPercentage => TotalQuestions > 0 
        ? (double)TotalCorrect / TotalQuestions * 100 
        : 0;
    public TimeSpan TotalTime { get; set; }
    public double AverageTimePerQuestion => TotalQuestions > 0 
        ? TotalTime.TotalSeconds / TotalQuestions 
        : 0;
    public List<QuizSession> RecentSessions { get; set; } = new();
    public Dictionary<string, int> AttemptsByRegion { get; set; } = new();
    public Dictionary<QuizType, int> AttemptsByType { get; set; } = new();
}