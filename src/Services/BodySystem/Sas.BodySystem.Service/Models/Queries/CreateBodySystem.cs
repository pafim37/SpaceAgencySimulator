using MediatR;

namespace Sas.BodySystem.Service.Models.Queries
{
    public sealed record CreateBodySystem(bool SkipStaticPoints = false) : IRequest<BodySystems.BodySystem>;
}
