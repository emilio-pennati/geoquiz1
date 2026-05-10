using System.Net.Http;
using System.Text.Json;
using GeoQuiz.Models;

namespace GeoQuiz.Services;

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://restcountries.com/v3.1";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private List<CountryDto>? _cachedCountries;
    private readonly SemaphoreSlim _cacheLock = new(1, 1);

    public CountryService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<List<CountryDto>> GetAllCountriesAsync()
    {
        await _cacheLock.WaitAsync();
        try
        {
            if (_cachedCountries != null)
            {
                return _cachedCountries;
            }

            try
            {
                var response = await _httpClient.GetStringAsync(
                    $"{BaseUrl}/all?fields=name,cca2,capital,region,subregion,population,languages,flags,timezones,continents,borders,currencies");
                _cachedCountries = JsonSerializer.Deserialize<List<CountryDto>>(response, JsonOptions) ?? new List<CountryDto>();
                return _cachedCountries;
            }
            catch (HttpRequestException)
            {
                if (_cachedCountries != null)
                {
                    return _cachedCountries;
                }
                throw new Exception("Unable to reach the server. Please check your internet connection.");
            }
            catch (TaskCanceledException)
            {
                if (_cachedCountries != null)
                {
                    return _cachedCountries;
                }
                throw new Exception("The request timed out. Please try again.");
            }
            catch (JsonException)
            {
                throw new Exception("Received malformed data from the service.");
            }
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    public async Task<List<CountryDto>> SearchByNameAsync(string name)
    {
        var allCountries = await GetCachedOrAllAsync();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            return allCountries;
        }

        return allCountries.Where(c => 
            c.CommonName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
            (c.OfficialName?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false)
        ).ToList();
    }

    public async Task<List<CountryDto>> GetByRegionAsync(string region)
    {
        var allCountries = await GetCachedOrAllAsync();
        
        return allCountries.Where(c => 
            c.Region.Equals(region, StringComparison.OrdinalIgnoreCase)
        ).ToList();
    }

    public async Task<CountryDto?> GetByCodeAsync(string code)
    {
        var allCountries = await GetCachedOrAllAsync();
        return allCountries.FirstOrDefault(c => 
            c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)
        );
    }

    private async Task<List<CountryDto>> GetCachedOrAllAsync()
    {
        await _cacheLock.WaitAsync();
        try
        {
            if (_cachedCountries != null)
            {
                return _cachedCountries;
            }
            return await GetAllCountriesAsync();
        }
        finally
        {
            _cacheLock.Release();
        }
    }
}