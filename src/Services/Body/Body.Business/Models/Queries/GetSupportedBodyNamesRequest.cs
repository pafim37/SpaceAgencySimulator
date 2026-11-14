using MediatR;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetSupportedBodyNamesRequest : IRequest<IReadOnlyList<string>>;
