using Country.Api.Models;   

namespace Country.Api.Repositories
{
    
    public interface ICountryRepository
    {
        Task<List<CountryModel>> GetAllAsync();

        Task<CountryModel?> GetByCodeAsync(string code);

        Task AddAsync(CountryModel country);
    }
}
