using AutoMapper;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Models.Entities;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Profiles
{
    public class VectorMapper : Profile
    {
        public VectorMapper()
        {
            _ = CreateMap<VectorEntity, VectorDto>()
                .ForMember(d => d.X, e => e.MapFrom(s => s.X))
                .ForMember(d => d.Y, e => e.MapFrom(s => s.Y))
                .ForMember(d => d.Z, e => e.MapFrom(s => s.Z))
                .ReverseMap()
                .ForMember(e => e.Id, e => e.Ignore());

            _ = CreateMap<Vector, VectorDto>()
                .ForMember(d => d.X, e => e.MapFrom(s => s.X))
                .ForMember(d => d.Y, e => e.MapFrom(s => s.Y))
                .ForMember(d => d.Z, e => e.MapFrom(s => s.Z));
        }
    }
}
