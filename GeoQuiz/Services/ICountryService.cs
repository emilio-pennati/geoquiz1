using GeoQuiz.Models;

namespace GeoQuiz.Services;

public interface ICountryService
{
    Task<List<CountryDto>> GetAllCountriesAsync();
    Task<List<CountryDto>> SearchByNameAsync(string name);
    Task<List<CountryDto>> GetByRegionAsync(string region);
    Task<CountryDto?> GetByCodeAsync(string code);
}