using Country.Api.Models;
namespace Country.Api.Services;
public interface ICountryService
{
    Task<List<CountryModel>> GetAllAsync();

    Task<CountryModel?> GetByCodeAsync(string code);
}