using MediatR;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodyNamesRequest : IRequest<IEnumerable<string>>;
