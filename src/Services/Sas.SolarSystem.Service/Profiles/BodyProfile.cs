using AutoMapper;
using Sas.Domain.Bodies;
using Sas.Mathematica;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Profiles
{
    internal class BodyProfile : Profile
    {
        public BodyProfile()
        {
            CreateMap<CelestialBody, BodyDocument>().ReverseMap();
            CreateMap<VectorDocument, Vector>().ReverseMap();
        }
    }
}
