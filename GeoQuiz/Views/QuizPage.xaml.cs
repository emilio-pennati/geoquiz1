using GeoQuiz.ViewModels;

namespace GeoQuiz.Views;

public partial class QuizPage : ContentPage
{
    public QuizPage(QuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is QuizViewModel viewModel)
        {
            viewModel.RestartQuizCommand.Execute(null);
        }
    }
}