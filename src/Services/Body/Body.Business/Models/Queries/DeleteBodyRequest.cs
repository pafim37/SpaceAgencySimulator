using MediatR;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record DeleteBodyRequest(string Name, string? SenderId) : IRequest<BodyEntity>;
