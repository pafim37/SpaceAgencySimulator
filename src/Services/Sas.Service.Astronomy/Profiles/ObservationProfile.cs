using AutoMapper;
using Sas.Service.Astronomy.DTOs;
using Sas.Service.Astronomy.Models;

namespace Sas.Service.Astronomy.Profiles
{
    public class ObservationProfile : Profile
    {
        public ObservationProfile()
        {
            CreateMap<ObservationEntity, ObservationDTO>()
                .ForMember(dest => dest.ObservatoryName, opt => opt.MapFrom(src => src.Observatory.Name));
        }
    }
}
