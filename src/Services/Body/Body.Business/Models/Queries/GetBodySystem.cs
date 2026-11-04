using MediatR;
using Sas.Body.Business.Models.Domain.BodySystems;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodySystem(double? AtTime = null) : IRequest<BodySystem>;
