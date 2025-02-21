using MediatR;
using Sas.Body.Service.Models.Domain.BodySystems;

namespace Sas.Body.Service.Models.Queries
{
    public record CreateBodySystem() : IRequest<BodySystem>
    {
    }
}
