using GeoQuiz.ViewModels;

namespace GeoQuiz.Views;

[QueryProperty(nameof(CountryCode), "code")]
public partial class CountryDetailPage : ContentPage
{
    public string? CountryCode { get; set; }

    public CountryDetailPage(CountryDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is CountryDetailViewModel viewModel
            && !string.IsNullOrEmpty(CountryCode))
        {
            _ = viewModel.LoadCountryAsync(CountryCode);
        }
    }
}