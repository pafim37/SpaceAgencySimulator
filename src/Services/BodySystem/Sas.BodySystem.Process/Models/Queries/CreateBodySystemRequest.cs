using MediatR;
using Sas.BodySystem.Process.Models.DataTransferObject;
using Sas.Mathematica.Service;

namespace Sas.BodySystem.Process.Models.Queries
{
    public sealed record CreateBodySystemRequest(float Scale = 500, double? AtTime = 0, bool IsMovementRequest = false, double GravitationalConstant = Constants.G) : IRequest<BodySystemDto>;
}
