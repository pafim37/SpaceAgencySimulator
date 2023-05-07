using AutoMapper;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;
using Sas.BodySystem.Service.Documents;

namespace Sas.BodySystem.Service.Profiles
{
    internal class BodyProfile : Profile
    {
        public BodyProfile()
        {
            CreateMap<Body, BodyDocument>().ReverseMap();
            CreateMap<VectorDocument, Vector>().ReverseMap();
        }
    }
}
