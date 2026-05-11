using GeoQuiz.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GeoQuiz.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
    }

    private async void OnCountrySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CountryListItem country)
        {
            var detailVm = App.Current.Handler.MauiContext.Services.GetService<CountryDetailViewModel>();
            var detailPage = new CountryDetailPage(detailVm!);
            await Navigation.PushAsync(detailPage);
            
            if (detailPage.BindingContext is CountryDetailViewModel vm)
            {
                await vm.LoadCountryAsync(country.Code);
            }
            
            if (sender is CollectionView cv)
            {
                cv.SelectedItem = null;
            }
        }
    }
}