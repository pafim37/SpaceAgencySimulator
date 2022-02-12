using AutoMapper;
using Sas.Astronomy.Service.DTOs;
using Sas.Astronomy.Service.Models;

namespace Sas.Astronomy.Service.Profiles
{
    public class ObservatoryProfile : Profile
    {
        public ObservatoryProfile()
        {
            CreateMap<ObservatoryEntity, ObservatoryDTO>().ReverseMap();
        }
    }
}
