using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class GetBodyNamesHandler(IBodyRepository repository) : IRequestHandler<GetBodyNamesRequest, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetBodyNamesRequest request, CancellationToken cancellationToken)
        => repository.GetAllBodiesNamesAsync(cancellationToken);
}
