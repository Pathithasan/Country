using Country.Api.Models;
using Country.Api.Repositories;
using Country.Api.Integrations.CountriesApi;
using AutoMapper;

namespace Country.Api.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    private readonly ICountryApiClient _countryApiClient;
    private readonly IMapper _mapper;

    public CountryService(
        ICountryRepository countryRepository,
        ICountryApiClient countryApiClient,
        IMapper mapper)
    {
        _countryRepository = countryRepository;
        _countryApiClient = countryApiClient;
        _mapper = mapper;
    }

    public async Task<List<CountryModel>> GetAllAsync()
    {
        var countries = await _countryRepository.GetAllAsync();

        if (countries.Any())
        {
            return _mapper.Map<List<CountryModel>>(countries);
        }

        var apiCountries = await _countryApiClient.GetAllCountries();

        foreach (var apiCountry in apiCountries)
        {
            var country = _mapper.Map<CountryModel>(apiCountry);
            await _countryRepository.AddAsync(country);
        }

        countries = await _countryRepository.GetAllAsync();

        return _mapper.Map<List<CountryModel>>(countries);
    }

    public async Task<CountryModel?> GetByCodeAsync(string code)
    {
        var country = await _countryRepository.GetByCodeAsync(code);

        if (country != null)
        {
            return _mapper.Map<CountryModel>(country);
        }

        var countries = await _countryRepository.GetAllAsync();

        if (!countries.Any())
        {
            var apiCountries = await _countryApiClient.GetAllCountries();

            foreach (var apiCountry in apiCountries)
            {
                var countryModel = _mapper.Map<CountryModel>(apiCountry);
                await _countryRepository.AddAsync(countryModel);
            }

            country = await _countryRepository.GetByCodeAsync(code);

            if (country != null)
            {
                return _mapper.Map<CountryModel>(country);
            }
        }
        else
        {
            var apiCountry = await _countryApiClient.GetCountriesByCodeAsync(code);

            if (apiCountry != null)
            {
                var countryModel = _mapper.Map<CountryModel>(apiCountry);
                await _countryRepository.AddAsync(countryModel);

                return countryModel;
            }
            return null;

        }

        return null;
    }
}