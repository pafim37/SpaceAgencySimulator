using MediatR;

namespace Sas.BodySystem.Service.Models.Queries;

public sealed record GetBodySystem(double AtTime) : IRequest<BodySystems.BodySystem>;
