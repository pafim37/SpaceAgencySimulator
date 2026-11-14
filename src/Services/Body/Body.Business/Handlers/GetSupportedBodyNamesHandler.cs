using MediatR;
using Sas.Body.Business.Models.Queries;

namespace Sas.Body.Business.Handlers;

public sealed class GetSupportedBodyNamesHandler : IRequestHandler<GetSupportedBodyNamesRequest, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(GetSupportedBodyNamesRequest request, CancellationToken cancellationToken)
        => Task.FromResult(SupportedBodyNames.All);
}
