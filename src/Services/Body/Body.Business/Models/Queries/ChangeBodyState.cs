using MediatR;
using Sas.Body.Core.Models.Entities;

namespace Sas.Body.Business.Models.Queries;

public sealed record ChangeBodyState(string Name, bool Enabled, string? SenderId) : IRequest<BodyEntity>;
