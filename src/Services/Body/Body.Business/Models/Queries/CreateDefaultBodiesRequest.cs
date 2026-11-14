using MediatR;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record CreateDefaultBodiesRequest(List<string> Names, string? SenderId) : IRequest<List<BodyEntity>>;
