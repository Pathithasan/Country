using AutoMapper;
using Country.Api.ViewModels;
using Country.Api.Models;
using Country.Api.Integrations.CountriesApi; // Change to your Country entity namespace

namespace Country.Api.Mapper
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<CountryApiResponseModel, CountryModel>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Alpha3Code))
                .ForMember(dest => dest.FlagUrl, opt => opt.MapFrom(src => src.Flags.Png))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate,opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
                
            CreateMap<CountryModel, CountryVM>();
        }
    }
}