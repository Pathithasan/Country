using Newtonsoft.Json;

namespace Country.Api.Integrations.CountriesApi
{
    public class CountryApiClient : ICountryApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl;
        public CountryApiClient(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _baseUrl = configuration["CountriesApiUrl"];
        }

        public async Task<List<CountryApiResponseModel>> GetCountriesByNameAsync(string name)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/name/{name}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<CountryApiResponseModel>();
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CountryApiResponseModel>>(content) ?? new List<CountryApiResponseModel>();
        }
        public async Task<List<CountryApiResponseModel>> GetAllCountries()
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/countries");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CountryApiResponseModel>>(content)?? new List<CountryApiResponseModel>();
        }
        public async Task<CountryApiResponseModel?> GetCountriesByCodeAsync(string code)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/alpha/{code}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CountryApiResponseModel>(content) ?? new CountryApiResponseModel();
        }
    }
}