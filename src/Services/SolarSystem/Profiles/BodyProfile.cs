using AutoMapper;
using Sas.Domain.Bodies;
using Sas.Mathematica.Service.Vectors;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Profiles
{
    internal class BodyProfile : Profile
    {
        public BodyProfile()
        {
            CreateMap<Body, CelestialBodyDocument>().ReverseMap();
            CreateMap<VectorDocument, Vector>().ReverseMap();
        }
    }
}
