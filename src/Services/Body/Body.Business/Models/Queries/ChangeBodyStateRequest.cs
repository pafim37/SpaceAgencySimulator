using MediatR;
using Sas.Body.Core.DataTransferObject;

namespace Sas.Body.Business.Models.Queries;

public sealed record ChangeBodyStateRequest(string Name, bool Enabled, string? SenderId) : IRequest<BodyDto>;
