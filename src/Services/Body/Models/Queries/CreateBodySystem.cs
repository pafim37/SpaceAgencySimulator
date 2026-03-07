using MediatR;
using Sas.Body.Service.Models.Domain.BodySystems;

namespace Sas.Body.Service.Models.Queries
{
    public sealed record CreateBodySystem(bool SkipStaticPoints = false) : IRequest<BodySystem>
    {
    }
}
