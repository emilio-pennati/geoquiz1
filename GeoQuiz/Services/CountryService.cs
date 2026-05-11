using System.Net.Http;
using System.Text.Json;
using GeoQuiz.Models;

namespace GeoQuiz.Services;

public class CountryService : ICountryService
{
    private readonly HttpClient httpClient;
    private const string BaseUrl = "https://restcountries.com/v3.1";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private List<CountryDto>? cachedCountries;
    private readonly SemaphoreSlim cacheLock = new(1, 1);

    public CountryService()
    {
        var handler = new HttpClientHandler();
        
        httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<List<CountryDto>> GetAllCountriesAsync()
    {
        await cacheLock.WaitAsync();
        try
        {
            if (cachedCountries != null)
            {
                return cachedCountries;
            }

            try
            {
                var url = $"{BaseUrl}/all?fields=name,cca2,capital,region,subregion,population,languages,flags,continents,currencies";
                
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                cachedCountries = JsonSerializer.Deserialize<List<CountryDto>>(content, JsonOptions) ?? new List<CountryDto>();
                return cachedCountries;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch countries: {ex.Message}");
            }
        }
        finally
        {
            cacheLock.Release();
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
        await cacheLock.WaitAsync();
        try
        {
            if (cachedCountries != null)
            {
                return cachedCountries;
            }
            return await GetAllCountriesAsync();
        }
        finally
        {
            cacheLock.Release();
        }
    }
}