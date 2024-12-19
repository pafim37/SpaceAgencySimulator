using AutoMapper;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;

namespace Sas.BodySystem.Service.Profiles
{
    public class BodyProfile : Profile
    {
        public BodyProfile()
        {
            _ = CreateMap<Body, BodyDTO>()
                .ReverseMap();
            _ = CreateMap<BodyDTO, BodyDocument>()
                .ReverseMap();
            _ = CreateMap<BodyDocument, Body>()
                .ReverseMap();
        }
    }
}
