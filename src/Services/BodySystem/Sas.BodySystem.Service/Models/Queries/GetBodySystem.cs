using MediatR;

namespace Sas.BodySystem.Process.Models.Queries;

public sealed record GetBodySystem() : IRequest<BodySystems.BodySystem>;
