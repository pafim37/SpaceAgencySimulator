using MediatR;
using Sas.Body.Business.Models.Queries;

namespace Sas.Body.Business.Handlers;

public sealed class GetSupportedBodyNamesHandler : IRequestHandler<GetSupportedBodyNames, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(GetSupportedBodyNames request, CancellationToken cancellationToken)
        => Task.FromResult(SupportedBodyNames.All);
}
