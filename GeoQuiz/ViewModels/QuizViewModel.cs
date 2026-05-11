using System.Collections.ObjectModel;
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
        if (QuestionTimeRemaining > 0)
        {
            QuestionTimeRemaining--;
        }
        else
        {
            timer?.Stop();
            HandleTimeout();
        }
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
            timer?.Stop();
            timer = new Timer(1000);
            timer.Elapsed += OnTimerElapsed;
            QuestionTimeRemaining = QuestionTimeLimit;
            IsTimeUp = false;
            timer.Start();
        }
    }

    private void StopTimer()
    {
        timer?.Stop();
        timer?.Dispose();
        timer = null;
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
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
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
        
        Options.Clear();
        foreach (var option in question.Options)
        {
            Options.Add(option);
        }

        HasAnswered = false;
        IsCorrect = false;
        SelectedOption = null;
        IsTimeUp = false;

        StartTimer();
    }

    [RelayCommand]
    private async Task SubmitAnswerAsync(string option)
    {
        if (HasAnswered || currentQuestionIndex >= questions.Count) return;

        StopTimer();

        SelectedOption = option;
        HasAnswered = true;

        var question = questions[currentQuestionIndex];
        IsCorrect = option == question.CorrectAnswer;

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