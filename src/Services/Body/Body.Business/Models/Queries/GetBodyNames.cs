using MediatR;

namespace Sas.Body.Business.Models.Queries;

public sealed record GetBodyNames : IRequest<IEnumerable<string>>;
