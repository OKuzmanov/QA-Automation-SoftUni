
using System.Text.Json.Serialization;

public class CountryInfo
{
    
    [JsonPropertyName("post code")]
    public string postcode { get; set; }
    public string country { get; set; }
    [JsonPropertyName("country abbreviation")]
    public string countryabbreviation { get; set; }
    public Place[] places { get; set; }
}

public class Place
{
    [JsonPropertyName("place name")]
    public string placename { get; set; }
    public string longitude { get; set; }
    public string state { get; set; }
    [JsonPropertyName("state abbreviation")]
    public string stateabbreviation { get; set; }
    public string latitude { get; set; }
}
