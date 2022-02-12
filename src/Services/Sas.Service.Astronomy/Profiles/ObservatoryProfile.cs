using AutoMapper;
using Sas.Service.Astronomy.DTOs;
using Sas.Service.Astronomy.Models;

namespace Sas.Service.Astronomy.Profiles
{
    public class ObservatoryProfile : Profile
    {
        public ObservatoryProfile()
        {
            CreateMap<ObservatoryEntity, ObservatoryDTO>().ReverseMap();
        }
    }
}
