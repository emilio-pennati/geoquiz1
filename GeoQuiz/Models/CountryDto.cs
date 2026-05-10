using System.Text.Json.Serialization;

namespace GeoQuiz.Models;

public class CountryDto
{
    [JsonPropertyName("cca2")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public CountryNameDto Name { get; set; } = new();

    [JsonPropertyName("capital")]
    public List<string>? Capital { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("subregion")]
    public string? Subregion { get; set; }

    [JsonPropertyName("population")]
    public int Population { get; set; }

    [JsonPropertyName("languages")]
    public Dictionary<string, string>? Languages { get; set; }

    [JsonPropertyName("flags")]
    public FlagDto Flags { get; set; } = new();

    [JsonPropertyName("currencies")]
    public Dictionary<string, CurrencyDto>? Currencies { get; set; }

    [JsonPropertyName("timezones")]
    public List<string>? Timezones { get; set; }

    [JsonPropertyName("continents")]
    public List<string>? Continents { get; set; }

    [JsonPropertyName("borders")]
    public List<string>? Borders { get; set; }

    public string CommonName => Name.Common;
    public string? OfficialName => Name.Official;
    public string? FlagUrl => Flags.Png;
    public string? CapitalCity => Capital?.FirstOrDefault();
    public string LanguagesList => Languages?.Values != null 
        ? string.Join(", ", Languages.Values) 
        : "N/A";
    public string CurrenciesList => Currencies?.Values != null 
        ? string.Join(", ", Currencies.Values.Select(c => c.Name)) 
        : "N/A";
    public string ContinentsList => Continents != null 
        ? string.Join(", ", Continents) 
        : "N/A";
    public string PopulationFormatted => Population.ToString("N0");
}

public class CountryNameDto
{
    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;

    [JsonPropertyName("official")]
    public string? Official { get; set; }
}

public class FlagDto
{
    [JsonPropertyName("png")]
    public string? Png { get; set; }

    [JsonPropertyName("svg")]
    public string? Svg { get; set; }

    [JsonPropertyName("alt")]
    public string? Alt { get; set; }
}

public class CurrencyDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }
}