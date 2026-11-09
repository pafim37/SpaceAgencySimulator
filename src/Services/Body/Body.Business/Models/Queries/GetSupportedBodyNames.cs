using MediatR;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetSupportedBodyNames : IRequest<IReadOnlyList<string>>;
