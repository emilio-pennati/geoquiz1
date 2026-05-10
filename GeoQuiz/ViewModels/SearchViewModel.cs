using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeoQuiz.Models;
using GeoQuiz.Services;

namespace GeoQuiz.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    private readonly ICountryService _countryService;

    public SearchViewModel(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasData;

    [ObservableProperty]
    private bool isEmptyState;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public ObservableCollection<CountryListItem> Countries { get; } = new();

    [RelayCommand]
    private async Task LoadAsync() => await LoadCountriesAsync();

    [RelayCommand]
    private async Task SearchAsync() => await SearchCountriesAsync();

    partial void OnSearchTextChanged(string value)
    {
        _ = SearchCountriesAsync();
    }

    private async Task LoadCountriesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var countries = await _countryService.GetAllCountriesAsync();

            Countries.Clear();
            foreach (var country in countries.OrderBy(c => c.CommonName))
            {
                Countries.Add(CountryListItem.FromCountry(country));
            }

            HasData = Countries.Count > 0;
            IsEmptyState = Countries.Count == 0;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Unable to reach the server. Please check your internet connection.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "The request timed out. Please try again.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SearchCountriesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        ErrorMessage = null;

        try
        {
            var countries = string.IsNullOrWhiteSpace(SearchText)
                ? await _countryService.GetAllCountriesAsync()
                : await _countryService.SearchByNameAsync(SearchText);

            Countries.Clear();
            foreach (var country in countries.OrderBy(c => c.CommonName))
            {
                Countries.Add(CountryListItem.FromCountry(country));
            }

            HasData = Countries.Count > 0;
            IsEmptyState = Countries.Count == 0 && !string.IsNullOrWhiteSpace(SearchText);
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Unable to reach the server. Please check your internet connection.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "The request timed out. Please try again.";
            HasData = false;
            IsEmptyState = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            HasData = false;
            IsEmptyState = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}