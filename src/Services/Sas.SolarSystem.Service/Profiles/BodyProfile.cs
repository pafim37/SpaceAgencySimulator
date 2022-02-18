using AutoMapper;
using Sas.Mathematica;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.DTOs;

namespace Sas.SolarSystem.Service.Profiles
{
    public class BodyProfile : Profile
    {
        public BodyProfile()
        {
            CreateMap<BodyDocument, BodyDTO>().ReverseMap();
            CreateMap<VectorDocument, Vector>().ReverseMap();
        }
    }
}
