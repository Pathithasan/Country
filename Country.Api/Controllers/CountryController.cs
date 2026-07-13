using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Country.Api.Services;
using Country.Api.ViewModels;


namespace Country.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ControllerBase
{ 
    private readonly ILogger<CountryController> _logger;
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public CountryController(ILogger<CountryController> logger, ICountryService countryService,
        IMapper mapper)
    {
        _logger = logger;
        _countryService = countryService;
        _mapper = mapper;
    }

    [HttpGet("code/{code:length(3,3)}")]
    public async Task<ActionResult<CountryVM>> GetByCode(string code)
    {
        var country = await _countryService.GetByCodeAsync(code);

        if (country == null )
        {
            return NotFound("Country not found.");
        }

        var result = _mapper.Map<CountryVM>(country);

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CountryVM>>> GetAllCountries()
    {
        var countries = await _countryService.GetAllAsync();

        if (countries == null || countries.Count == 0)
        {
            return NotFound("No countries found.");
        }

        var result = _mapper.Map<List<CountryVM>>(countries);

        return Ok(result);
    }
}
