using AutoMapper;
using Sas.Astronomy.Service.DTOs;
using Sas.Astronomy.Service.Models;
using Sas.Domain;

namespace Sas.Astronomy.Service.Profiles
{
    public class ObservatoryProfile : Profile
    {
        public ObservatoryProfile()
        {
            CreateMap<ObservatoryEntity, ObservatoryDTO>().ReverseMap();
            CreateMap<ObservatoryEntity, Observatory>().ReverseMap();
        }
    }
}
