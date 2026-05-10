using System.Net.Http;
using System.Text.Json;
using GeoQuiz.Models;

namespace GeoQuiz.Services;

public interface ICountryService
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<List<Country>> SearchByNameAsync(string name);
    Task<List<Country>> GetByRegionAsync(string region);
    Task<Country?> GetByCodeAsync(string code);
}

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://restcountries.com/v3.1";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CountryService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<List<Country>> GetAllCountriesAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}/all?fields=name,cca2,capital,region,subregion,population,languages,flags,timezones,continents,borders");
            return JsonSerializer.Deserialize<List<Country>>(response, JsonOptions) ?? new List<Country>();
        }
        catch (HttpRequestException)
        {
            throw new Exception("Unable to reach the server. Please check your internet connection.");
        }
        catch (TaskCanceledException)
        {
            throw new Exception("The request timed out. Please try again.");
        }
        catch (JsonException)
        {
            throw new Exception("Received malformed data from the service.");
        }
    }

    public async Task<List<Country>> SearchByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await GetAllCountriesAsync();
        }

        try
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}/name/{Uri.EscapeDataString(name)}?fields=name,cca2,capital,region,subregion,population,languages,flags,timezones,continents,borders");
            return JsonSerializer.Deserialize<List<Country>>(response, JsonOptions) ?? new List<Country>();
        }
        catch (HttpRequestException)
        {
            throw new Exception("Unable to reach the server. Please check your internet connection.");
        }
        catch (TaskCanceledException)
        {
            throw new Exception("The request timed out. Please try again.");
        }
        catch (JsonException)
        {
            throw new Exception("Received malformed data from the service.");
        }
    }

    public async Task<List<Country>> GetByRegionAsync(string region)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}/region/{region}?fields=name,cca2,capital,region,subregion,population,languages,flags,timezones,continents,borders");
            return JsonSerializer.Deserialize<List<Country>>(response, JsonOptions) ?? new List<Country>();
        }
        catch (HttpRequestException)
        {
            throw new Exception("Unable to reach the server. Please check your internet connection.");
        }
        catch (TaskCanceledException)
        {
            throw new Exception("The request timed out. Please try again.");
        }
        catch (JsonException)
        {
            throw new Exception("Received malformed data from the service.");
        }
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        try
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}/alpha/{code}?fields=name,cca2,capital,region,subregion,population,languages,flags,timezones,continents,borders");
            var result = JsonSerializer.Deserialize<List<Country>>(response, JsonOptions);
            return result?.FirstOrDefault();
        }
        catch (HttpRequestException)
        {
            throw new Exception("Unable to reach the server. Please check your internet connection.");
        }
        catch (TaskCanceledException)
        {
            throw new Exception("The request timed out. Please try again.");
        }
        catch (JsonException)
        {
            throw new Exception("Received malformed data from the service.");
        }
    }
}