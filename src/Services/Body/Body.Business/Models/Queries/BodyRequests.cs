using MediatR;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodies : IRequest<IEnumerable<BodyDto>>;
public sealed record GetBodyNames : IRequest<IEnumerable<string>>;
public sealed record GetBodyByName(string Name) : IRequest<BodyDto?>;
public sealed record GetSupportedBodyNames : IRequest<IReadOnlyList<string>>;
public sealed record CreateBody(NewBodyDto Body, string? SenderId) : IRequest<BodyEntity>;
public sealed record CloneBody(string Name, string? SenderId) : IRequest<BodyEntity?>;
public sealed record UpdateBody(BodyDto Body, string? SenderId) : IRequest<BodyEntity>;
public sealed record ChangeBodyState(string Name, bool Enabled, string? SenderId) : IRequest<BodyEntity>;
public sealed record DeleteBody(string Name, string? SenderId) : IRequest<BodyEntity>;
public sealed record CreateDefaultBodies(List<string> Names, string? SenderId) : IRequest<List<BodyEntity>>;
