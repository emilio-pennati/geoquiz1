using GeoQuiz.ViewModels;

namespace GeoQuiz.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnCountrySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CountryListItem country)
        {
            var code = Uri.EscapeDataString(country.Code);
            await Shell.Current.GoToAsync($"country-detail?code={code}");

            if (sender is CollectionView cv)
            {
                cv.SelectedItem = null;
            }
        }
    }
}