using AutoMapper;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Service.Profiles
{
    public class VectorProfile : Profile
    {
        public VectorProfile()
        {
            _ = CreateMap<Vector, VectorDTO>()
                .ReverseMap();
            _ = CreateMap<VectorDTO, VectorDocument>()
                .ReverseMap();
            _ = CreateMap<Vector, VectorDocument>()
                .ReverseMap();
        }
    }
}
