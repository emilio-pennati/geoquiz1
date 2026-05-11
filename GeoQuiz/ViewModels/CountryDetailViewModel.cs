using System.Text.Json;
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
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasData;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

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
        catch (HttpRequestException)
        {
            ErrorMessage = "Unable to reach the server. Please check your internet connection.";
            HasData = false;
        }
        catch (JsonException)
        {
            ErrorMessage = "Received malformed data from the service.";
            HasData = false;
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "The request timed out. Please try again.";
            HasData = false;
        }
        catch (Exception)
        {
            ErrorMessage = "An unexpected error occurred. Please try again.";
            HasData = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}