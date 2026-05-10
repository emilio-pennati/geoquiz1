using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeoQuiz.Models;
using GeoQuiz.Services;

namespace GeoQuiz.ViewModels;

public partial class QuizViewModel : ObservableObject
{
    private readonly IQuizService _quizService;
    private readonly ICountryService _countryService;
    private List<CountryDto> _allCountries = new();
    private List<QuizQuestion> _questions = new();
    private int _currentQuestionIndex;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isQuizActive;

    [ObservableProperty]
    private bool _isQuizComplete;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private int _currentQuestionNumber;

    [ObservableProperty]
    private int _totalQuestions = 10;

    [ObservableProperty]
    private string _questionText = string.Empty;

    [ObservableProperty]
    private string? _questionImageUrl;

    [ObservableProperty]
    private bool _showQuestionImage;

    [ObservableProperty]
    private string _option1 = string.Empty;

    [ObservableProperty]
    private string _option2 = string.Empty;

    [ObservableProperty]
    private string _option3 = string.Empty;

    [ObservableProperty]
    private string _option4 = string.Empty;

    [ObservableProperty]
    private ObservableCollection<string> _options = new();

    [ObservableProperty]
    private string? _selectedOption;

    [ObservableProperty]
    private bool _hasAnswered;

    [ObservableProperty]
    private bool _isCorrect;

    [ObservableProperty]
    private string? _option1Selected;

    [ObservableProperty]
    private string? _option2Selected;

    [ObservableProperty]
    private string? _option3Selected;

    [ObservableProperty]
    private string? _option4Selected;

    [ObservableProperty]
    private int _score;

    [ObservableProperty]
    private int _correctAnswers;

    [ObservableProperty]
    private QuizType _currentQuizType = QuizType.Capital;

    public QuizViewModel(IQuizService quizService, ICountryService countryService)
    {
        _quizService = quizService;
        _countryService = countryService;
        Options = new ObservableCollection<string>();
    }

    [RelayCommand]
    private async Task StartQuizAsync()
    {
        if (IsLoading) return;

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            _allCountries = await _countryService.GetAllCountriesAsync();
            
            if (_allCountries.Count < 4)
            {
                ErrorMessage = "Not enough countries to start quiz";
                return;
            }

            _questions = _quizService.GenerateQuiz(_allCountries, TotalQuestions, CurrentQuizType);
            _currentQuestionIndex = 0;
            Score = 0;
            CorrectAnswers = 0;
            IsQuizActive = true;
            IsQuizComplete = false;
            
            LoadCurrentQuestion();
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
        if (_currentQuestionIndex >= _questions.Count)
        {
            IsQuizActive = false;
            IsQuizComplete = true;
            return;
        }

        var question = _questions[_currentQuestionIndex];
        CurrentQuestionNumber = _currentQuestionIndex + 1;
        QuestionText = question.QuestionText;
        
        ShowQuestionImage = question.Type == QuizType.Flag;
        QuestionImageUrl = question.Country?.FlagUrl;
        
        Options.Clear();
        var opts = question.Options;
        Option1 = opts.Count > 0 ? opts[0] : "";
        Option2 = opts.Count > 1 ? opts[1] : "";
        Option3 = opts.Count > 2 ? opts[2] : "";
        Option4 = opts.Count > 3 ? opts[3] : "";

        HasAnswered = false;
        IsCorrect = false;
        SelectedOption = null;
    }

    [RelayCommand]
    private async Task SubmitAnswerAsync(string option)
    {
        if (HasAnswered || _currentQuestionIndex >= _questions.Count) return;

        SelectedOption = option;
        HasAnswered = true;

        var question = _questions[_currentQuestionIndex];
        IsCorrect = option == question.CorrectAnswer;

        Option1Selected = option == Option1 ? (IsCorrect ? "Correct" : "Wrong") : (Option1 == question.CorrectAnswer ? "Correct" : null);
        Option2Selected = option == Option2 ? (IsCorrect ? "Correct" : "Wrong") : (Option2 == question.CorrectAnswer ? "Correct" : null);
        Option3Selected = option == Option3 ? (IsCorrect ? "Correct" : "Wrong") : (Option3 == question.CorrectAnswer ? "Correct" : null);
        Option4Selected = option == Option4 ? (IsCorrect ? "Correct" : "Wrong") : (Option4 == question.CorrectAnswer ? "Correct" : null);

        if (IsCorrect)
        {
            Score += 10;
            CorrectAnswers++;
        }

        await Task.Delay(1500);
        
        _currentQuestionIndex++;
        LoadCurrentQuestion();
    }

    [RelayCommand]
    private void RestartQuiz()
    {
        IsQuizActive = false;
        IsQuizComplete = false;
        _currentQuestionIndex = 0;
        Score = 0;
        CorrectAnswers = 0;
    }
}