
namespace Country.Api.Integrations.CountriesApi
{
    public interface ICountryApiClient
    {
        Task<List<CountryApiResponseModel>> GetCountriesByNameAsync(string name);
        Task<CountryApiResponseModel?> GetCountriesByCodeAsync(string code);
        Task<List<CountryApiResponseModel>> GetAllCountries();
    }
}