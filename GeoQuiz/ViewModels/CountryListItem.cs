using CommunityToolkit.Mvvm.ComponentModel;
using GeoQuiz.Models;

namespace GeoQuiz.ViewModels;

public partial class CountryListItem : ObservableObject
{
    [ObservableProperty]
    private string code = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? flagUrl;

    [ObservableProperty]
    private string? capital;

    [ObservableProperty]
    private string region = string.Empty;

    public static CountryListItem FromCountry(CountryDto country)
    {
        return new CountryListItem
        {
            Code = country.Code,
            Name = country.CommonName,
            FlagUrl = country.FlagUrl,
            Capital = country.CapitalCity,
            Region = country.Region
        };
    }
}