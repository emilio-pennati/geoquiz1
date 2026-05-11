using CommunityToolkit.Mvvm.ComponentModel;
using GeoQuiz.Models;
using GeoQuiz.Services;

namespace GeoQuiz.ViewModels;

public partial class CountryDetailViewModel : ObservableObject
{
    private readonly ICountryService countryService;

    [ObservableProperty]
    private CountryDto? country;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasData;

    public CountryDetailViewModel(ICountryService countryService)
    {
        this.countryService = countryService;
    }

    public async Task LoadCountryAsync(string code)
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var country = await countryService.GetByCodeAsync(code);
            Country = country;
            HasData = country != null;

            if (country == null)
            {
                ErrorMessage = "Country not found";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            HasData = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}