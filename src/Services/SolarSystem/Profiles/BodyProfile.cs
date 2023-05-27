using AutoMapper;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Service.Profiles
{
    internal class BodyProfile : Profile
    {
        public BodyProfile()
        {
            CreateMap<BodyDocument, BodyDTO>().ReverseMap();
            CreateMap<VectorDocument, VectorDTO>().ReverseMap();
            CreateMap<Body, BodyDocument>().ReverseMap();
            CreateMap<VectorDocument, Vector>().ReverseMap();
            CreateMap<Body, BodyDTO>().ReverseMap();
            CreateMap<Vector, VectorDTO>().ReverseMap();
        }
    }
}
