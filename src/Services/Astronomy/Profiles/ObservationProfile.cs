using AutoMapper;
using Sas.Astronomy.Service.DTOs;
using Sas.Astronomy.Service.Models;
using Sas.Domain.Observations;

namespace Sas.Astronomy.Service.Profiles
{
    public class ObservationProfile : Profile
    {
        public ObservationProfile()
        {
            CreateMap<ObservationEntity, ObservationDTO>()
                .ForMember(dest => dest.ObservatoryName, opt => opt.MapFrom(src => src.Observatory.Name));

            CreateMap<ObservationCreateInstantDTO, ObservationDTO>();
        }
    }
}
