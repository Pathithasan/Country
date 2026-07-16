using Newtonsoft.Json;

namespace Country.Api.Integrations.CountriesApi
{
    public class CountryApiClient : ICountryApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<CountryApiClient> _logger;
        private readonly string _baseUrl;

        public CountryApiClient(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            ILogger<CountryApiClient> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;

            _baseUrl = configuration["CountriesApiUrl"]
                ?? throw new ArgumentNullException(nameof(configuration), "CountriesApiUrl configuration is missing.");
        }


        public async Task<List<CountryApiResponseModel>> GetCountriesByNameAsync(string name)
        {
            try 
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"{_baseUrl}/name/{name}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("No countries found for name: {Name}", name);
                    return new List<CountryApiResponseModel>();
                }

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<CountryApiResponseModel>>(content) ?? new List<CountryApiResponseModel>();
            
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling Countries API by Name:");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize Countries API responseby Name.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving country data by Name.");
                throw;
            }
        }
        public async Task<List<CountryApiResponseModel>> GetAllCountries()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"{_baseUrl}/countries");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<CountryApiResponseModel>>(content)?? new List<CountryApiResponseModel>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling all Countries API:");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize all Countries API response.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving all country data.");
                throw;
            }
            
        }
        public async Task<CountryApiResponseModel?> GetCountriesByCodeAsync(string code)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"{_baseUrl}/alpha/{code}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("No country found for code: {Code}", code);
                    return null;
                }

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<CountryApiResponseModel>(content) ?? new CountryApiResponseModel();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling Countries API by Code:");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize Countries API response by Code.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving country data by Code.");
                throw;
            }
            
        }
    }
}