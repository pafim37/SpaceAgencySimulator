using MediatR;
using Sas.Body.Service.Models.Domain;

namespace Sas.Body.Service.Models.Queries
{
    public record CreateBodySystem(double G) : IRequest<BodySystem>
    {
    }
}
