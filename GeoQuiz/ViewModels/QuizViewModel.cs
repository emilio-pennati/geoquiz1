using System.Collections.ObjectModel;
using System.Text.Json;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeoQuiz.Models;
using GeoQuiz.Services;
using Timer = System.Timers.Timer;

namespace GeoQuiz.ViewModels;

public partial class QuizViewModel : ObservableObject
{
    private readonly IQuizService quizService;
    private readonly ICountryService countryService;
    private Timer? timer;
    private List<CountryDto> allCountries = new();
    private List<QuizQuestion> questions = new();
    private int currentQuestionIndex;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isQuizActive;

    [ObservableProperty]
    private bool isQuizComplete;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private int currentQuestionNumber;

    [ObservableProperty]
    private int totalQuestions = 10;

    [ObservableProperty]
    private string questionText = string.Empty;

    [ObservableProperty]
    private string? questionImageUrl;

    [ObservableProperty]
    private bool showQuestionImage;

    [ObservableProperty]
    private ObservableCollection<string> options = new();

    [ObservableProperty]
    private string option1 = string.Empty;

    [ObservableProperty]
    private string option2 = string.Empty;

    [ObservableProperty]
    private string option3 = string.Empty;

    [ObservableProperty]
    private string option4 = string.Empty;

    [ObservableProperty]
    private string option1Background = "#E0E0E0";

    [ObservableProperty]
    private string option2Background = "#E0E0E0";

    [ObservableProperty]
    private string option3Background = "#E0E0E0";

    [ObservableProperty]
    private string option4Background = "#E0E0E0";

    [ObservableProperty]
    private string? selectedOption;

    [ObservableProperty]
    private bool hasAnswered;

    [ObservableProperty]
    private bool isCorrect;

    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private int correctAnswers;

    [ObservableProperty]
    private QuizType currentQuizType = QuizType.Capital;

    [ObservableProperty]
    private bool isTimedMode;

    [ObservableProperty]
    private int questionTimeLimit = 15;

    [ObservableProperty]
    private int questionTimeRemaining;

    [ObservableProperty]
    private bool isTimeUp;

    public QuizViewModel(IQuizService quizService, ICountryService countryService)
    {
        this.quizService = quizService;
        this.countryService = countryService;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (QuestionTimeRemaining > 0)
            {
                QuestionTimeRemaining--;
            }
            else
            {
                timer?.Stop();
                HandleTimeout();
            }
        });
    }

    private void HandleTimeout()
    {
        if (!HasAnswered && IsQuizActive)
        {
            IsTimeUp = true;
            HasAnswered = true;
            SelectedOption = null;
        }
    }

    private void StartTimer()
    {
        if (IsTimedMode)
        {
            StopTimer();
            timer = new Timer(1000);
            timer.Elapsed += OnTimerElapsed;
            QuestionTimeRemaining = QuestionTimeLimit;
            IsTimeUp = false;
            timer.Start();
        }
    }

    private void StopTimer()
    {
        if (timer != null)
        {
            timer.Elapsed -= OnTimerElapsed;
            timer.Stop();
            timer.Dispose();
            timer = null;
        }
    }

    [RelayCommand]
    private async Task StartQuizAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            allCountries = await countryService.GetAllCountriesAsync();
            
            if (allCountries.Count < 4)
            {
                ErrorMessage = "Not enough countries to start quiz";
                return;
            }

            questions = quizService.GenerateQuiz(allCountries, TotalQuestions, CurrentQuizType);
            currentQuestionIndex = 0;
            Score = 0;
            CorrectAnswers = 0;
            IsQuizActive = true;
            IsQuizComplete = false;
            
            LoadCurrentQuestion();
            StartTimer();
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Unable to reach the server. Please check your internet connection.";
        }
        catch (JsonException)
        {
            ErrorMessage = "Received malformed data from the service.";
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "The request timed out. Please try again.";
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void LoadCurrentQuestion()
    {
        StopTimer();

        if (currentQuestionIndex >= questions.Count)
        {
            IsQuizActive = false;
            IsQuizComplete = true;
            return;
        }

        var question = questions[currentQuestionIndex];
        CurrentQuestionNumber = currentQuestionIndex + 1;
        QuestionText = question.QuestionText;
        
        ShowQuestionImage = question.Type == QuizType.Flag;
        QuestionImageUrl = question.Country?.FlagUrl;
        
        if (question.Options.Count >= 4)
        {
            Option1 = question.Options[0];
            Option2 = question.Options[1];
            Option3 = question.Options[2];
            Option4 = question.Options[3];
        }

        Option1Background = "#E0E0E0";
        Option2Background = "#E0E0E0";
        Option3Background = "#E0E0E0";
        Option4Background = "#E0E0E0";

        HasAnswered = false;
        IsCorrect = false;
        SelectedOption = null;
        IsTimeUp = false;

        StartTimer();
    }

    [RelayCommand]
    private async Task SubmitAnswerAsync(string optionIndex)
    {
        if (HasAnswered || currentQuestionIndex >= questions.Count) return;

        StopTimer();

        string selectedText = optionIndex switch
        {
            "1" => Option1,
            "2" => Option2,
            "3" => Option3,
            "4" => Option4,
            _ => string.Empty
        };

        SelectedOption = selectedText;
        HasAnswered = true;

        var question = questions[currentQuestionIndex];
        IsCorrect = selectedText == question.CorrectAnswer;

        string correctBg = "#90EE90";
        string wrongBg = "#FF6B6B";

        if (Option1 == question.CorrectAnswer)
            Option1Background = correctBg;
        else if (optionIndex == "1")
            Option1Background = wrongBg;

        if (Option2 == question.CorrectAnswer)
            Option2Background = correctBg;
        else if (optionIndex == "2")
            Option2Background = wrongBg;

        if (Option3 == question.CorrectAnswer)
            Option3Background = correctBg;
        else if (optionIndex == "3")
            Option3Background = wrongBg;

        if (Option4 == question.CorrectAnswer)
            Option4Background = correctBg;
        else if (optionIndex == "4")
            Option4Background = wrongBg;

        if (IsCorrect)
        {
            int timeBonus = IsTimedMode ? QuestionTimeRemaining : 0;
            Score += 10 + timeBonus;
            CorrectAnswers++;
        }

        await Task.Delay(1500);
        
        currentQuestionIndex++;
        LoadCurrentQuestion();
    }

    [RelayCommand]
    private void RestartQuiz()
    {
        StopTimer();
        IsQuizActive = false;
        IsQuizComplete = false;
        currentQuestionIndex = 0;
        Score = 0;
        CorrectAnswers = 0;
    }
}