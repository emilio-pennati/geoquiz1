using CommunityToolkit.Mvvm.ComponentModel;
using GeoQuiz.Models;
using GeoQuiz.Services;

namespace GeoQuiz.ViewModels;

public partial class CountryDetailViewModel : ObservableObject
{
    private readonly ICountryService _countryService;

    [ObservableProperty]
    private CountryDto? _country;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasData;

    public CountryDetailViewModel(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task LoadCountryAsync(string code)
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var country = await _countryService.GetByCodeAsync(code);
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