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
        var handler = new HttpClientHandler();
        
        _httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
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
                var url = $"{BaseUrl}/all?fields=name,cca2,capital,region,subregion,population,languages,flags,continents,currencies";
                
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                _cachedCountries = JsonSerializer.Deserialize<List<CountryDto>>(content, JsonOptions) ?? new List<CountryDto>();
                return _cachedCountries;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"API Error: {ex.GetType().Name} - {ex.Message}");
                
                _cachedCountries = GetSampleCountries();
                return _cachedCountries;
            }
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    private static List<CountryDto> GetSampleCountries()
    {
        return new List<CountryDto>
        {
            new() { Code = "IT", Name = new CountryNameDto { Common = "Italy", Official = "Italian Republic" }, Region = "Europe", Capital = new List<string> { "Rome" }, Population = 59000000, Languages = new Dictionary<string, string> { { "ita", "Italian" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/it.png" } },
            new() { Code = "FR", Name = new CountryNameDto { Common = "France", Official = "French Republic" }, Region = "Europe", Capital = new List<string> { "Paris" }, Population = 67000000, Languages = new Dictionary<string, string> { { "fra", "French" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/fr.png" } },
            new() { Code = "DE", Name = new CountryNameDto { Common = "Germany", Official = "Federal Republic of Germany" }, Region = "Europe", Capital = new List<string> { "Berlin" }, Population = 83000000, Languages = new Dictionary<string, string> { { "deu", "German" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/de.png" } },
            new() { Code = "ES", Name = new CountryNameDto { Common = "Spain", Official = "Kingdom of Spain" }, Region = "Europe", Capital = new List<string> { "Madrid" }, Population = 47000000, Languages = new Dictionary<string, string> { { "spa", "Spanish" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/es.png" } },
            new() { Code = "GB", Name = new CountryNameDto { Common = "United Kingdom", Official = "United Kingdom of Great Britain and Northern Ireland" }, Region = "Europe", Capital = new List<string> { "London" }, Population = 67000000, Languages = new Dictionary<string, string> { { "eng", "English" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/gb.png" } },
            new() { Code = "US", Name = new CountryNameDto { Common = "United States", Official = "United States of America" }, Region = "Americas", Capital = new List<string> { "Washington, D.C." }, Population = 331000000, Languages = new Dictionary<string, string> { { "eng", "English" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/us.png" } },
            new() { Code = "BR", Name = new CountryNameDto { Common = "Brazil", Official = "Federative Republic of Brazil" }, Region = "Americas", Capital = new List<string> { "Brasília" }, Population = 212000000, Languages = new Dictionary<string, string> { { "por", "Portuguese" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/br.png" } },
            new() { Code = "JP", Name = new CountryNameDto { Common = "Japan", Official = "Japan" }, Region = "Asia", Capital = new List<string> { "Tokyo" }, Population = 126000000, Languages = new Dictionary<string, string> { { "jpn", "Japanese" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/jp.png" } },
            new() { Code = "CN", Name = new CountryNameDto { Common = "China", Official = "People's Republic of China" }, Region = "Asia", Capital = new List<string> { "Beijing" }, Population = 1400000000, Languages = new Dictionary<string, string> { { "zho", "Chinese" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/cn.png" } },
            new() { Code = "IN", Name = new CountryNameDto { Common = "India", Official = "Republic of India" }, Region = "Asia", Capital = new List<string> { "New Delhi" }, Population = 1380000000, Languages = new Dictionary<string, string> { { "hin", "Hindi" }, { "eng", "English" } }, Flags = new FlagDto { Png = "https://flagcdn.com/w320/in.png" } },
        };
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