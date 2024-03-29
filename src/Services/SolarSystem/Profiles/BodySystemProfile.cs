﻿using AutoMapper;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Orbits;

namespace Sas.BodySystem.Service.Profiles
{
    public class BodySystemProfile : Profile
    {
        public BodySystemProfile()
        {
            _ = CreateMap<Sas.Domain.Models.Bodies.BodySystem, BodySystemOutputData>()
                .ForMember(src => src.Bodies, des => des.MapFrom(src => src.Bodies))
                .ForMember(src => src.Orbits, des => des.MapFrom(src => src.OrbitsDescription))
                .ForMember(src => src.GravitationalConstant, des => des.MapFrom(src => src.G))
                .ReverseMap();

            _ = CreateMap<OrbitHolder, OrbitDTO>()
                .ForMember(src => src.Name, des => des.MapFrom(src => src.Name))
                .ForMember(src => src.OrbitType, des => des.MapFrom(src => (int)(src.Orbit!.OrbitType)))
                .ForMember(src => src.Center, des => des.MapFrom(src => src.Center))
                .ForMember(src => src.SemiMajorAxis, des => des.MapFrom(src => src.Orbit!.SemiMajorAxis))
                .ForMember(src => src.SemiMinorAxis, des => des.MapFrom(src => src.Orbit!.SemiMinorAxis))
                .ForMember(src => src.Radius, des => des.MapFrom(src => src.Orbit!.Radius))
                .ForMember(src => src.Rotation, des => des.MapFrom(src => src.Rotation))
                .ReverseMap();

        }
    }
}
