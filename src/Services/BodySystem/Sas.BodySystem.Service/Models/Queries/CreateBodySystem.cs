using MediatR;
using Sas.Mathematica.Service;

namespace Sas.BodySystem.Process.Models.Queries
{
    public sealed record CreateBodySystem(double GravitationalConstant = Constants.G) : IRequest<BodySystems.BodySystem>;
}
