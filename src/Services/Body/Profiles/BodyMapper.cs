﻿using AutoMapper;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Models.Entities;

namespace Sas.Body.Service.Profiles
{
    public class BodyMapper : Profile
    {
        public BodyMapper()
        {
            _ = CreateMap<BodyEntity, BodyDto>()
                .ForMember(d => d.Name, e => e.MapFrom(s => s.Name))
                .ForMember(d => d.Mass, e => e.MapFrom(s => s.Mass))
                .ForMember(d => d.Radius, e => e.MapFrom(s => s.Radius))
                .ForMember(d => d.Enabled, e => e.MapFrom(s => s.Enabled))
                .ForMember(d => d.Position, e => e.MapFrom(s => s.Position))
                .ForMember(d => d.Velocity, e => e.MapFrom(s => s.Velocity))
                .ForMember(d => d.Id, e => e.MapFrom(s => s.Id))
                .ReverseMap()
                .ForMember(e => e.PositionId, d => d.Ignore())
                .ForMember(e => e.VelocityId, d => d.Ignore());

            _ = CreateMap<BodyEntity, NewBodyDto>()
                .ForMember(d => d.Name, e => e.MapFrom(s => s.Name))
                .ForMember(d => d.Mass, e => e.MapFrom(s => s.Mass))
                .ForMember(d => d.Radius, e => e.MapFrom(s => s.Radius))
                .ForMember(d => d.Enabled, e => e.MapFrom(s => s.Enabled))
                .ForMember(d => d.Position, e => e.MapFrom(s => s.Position))
                .ForMember(d => d.Velocity, e => e.MapFrom(s => s.Velocity))
                .ReverseMap()
                .ForMember(e => e.PositionId, d => d.Ignore())
                .ForMember(e => e.VelocityId, d => d.Ignore())
                .ForMember(e => e.Id, d => d.Ignore());
        }
    }
}
