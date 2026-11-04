using MediatR;
using Sas.Body.Business.Models.Domain.BodySystems;

namespace Sas.Body.Business.Models.Queries
{
    public sealed record CreateBodySystem(bool SkipStaticPoints = false) : IRequest<BodySystem>
    {
    }
}
