using AutoMapper;
using Sas.Mathematica.Service.Vectors;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.Models;

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
