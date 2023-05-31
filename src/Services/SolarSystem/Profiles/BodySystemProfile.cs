using AutoMapper;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Orbits;

namespace Sas.BodySystem.Service.Profiles
{
    internal class BodySystemProfile : Profile
    {
        public BodySystemProfile()
        {
            CreateMap<Sas.Domain.Models.Bodies.BodySystem, BodySystemDTO>()
                .ForMember(src => src.Bodies, des => des.MapFrom(src => src.Bodies))
                .ForMember(src => src.Orbits, des => des.MapFrom(src => src.OrbitsDescription))
                .ReverseMap();

            CreateMap<OrbitHolder, OrbitDTO>()
                .ForMember(src => src.Name, des => des.MapFrom(src => src.Name))
                .ForMember(src => src.OrbitType, des => des.MapFrom(src => (int)(src.Orbit.OrbitType)))
                .ForMember(src => src.Center, des => des.MapFrom(src => src.Center))
                .ForMember(src => src.SemiMajorAxis, des => des.MapFrom(src => src.Orbit.SemiMajorAxis))
                .ForMember(src => src.SemiMinorAxis, des => des.MapFrom(src => src.Orbit.SemiMinorAxis))
                .ForMember(src => src.Rotation, des => des.MapFrom(src => src.Rotation))
                .ReverseMap();

            //CreateMap<Orbit, OrbitDTO>()
            //    .ForMember(src => src.SemiMajorAxis, des => des.MapFrom(src => src.SemiMajorAxis))
            //    .ForMember(src => src.SemiMinorAxis, des => des.MapFrom(src => src.SemiMinorAxis))
            //    .ReverseMap();
        }
    }
}
