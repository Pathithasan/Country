using Newtonsoft.Json;

namespace Country.Api.Integrations.CountriesApi
{
    public class CountryApiResponseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alpha3Code")]
        public string? Alpha3Code { get; set; }

        [JsonProperty("capital")]
        public string? Capital { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("area")]
        public decimal Area { get; set; }

        [JsonProperty("flag")]
        public string? Flag { get; set; }

        [JsonProperty("flags")]
        public CountryFlagModel Flags { get; set; }
    }

    public class CountryFlagModel
    {
        [JsonProperty("png")]
        public string? Png { get; set; }

        [JsonProperty("svg")]
        public string? Svg { get; set; }
    }
}