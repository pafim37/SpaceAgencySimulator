using MediatR;
using Sas.Mathematica.Service;

namespace Sas.BodySystem.Process.Models.Queries
{
    public sealed record CreateBodySystemRequest(float Scale = 1000, double GravitationalConstant = Constants.G) : IRequest<BodySystems.BodySystem>;
}
