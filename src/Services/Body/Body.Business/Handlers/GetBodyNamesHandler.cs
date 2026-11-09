using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class GetBodyNamesHandler(IBodyRepository repository) : IRequestHandler<GetBodyNames, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetBodyNames request, CancellationToken cancellationToken)
        => repository.GetAllBodiesNamesAsync(cancellationToken);
}
