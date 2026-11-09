using MediatR;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record CreateDefaultBodies(List<string> Names, string? SenderId) : IRequest<List<BodyEntity>>;
