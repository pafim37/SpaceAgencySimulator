using MediatR;
using Sas.Body.Business.Models.Queries;
using AutoMapper;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class GetBodyByNameHandler(IBodyRepository repository, IMapper mapper) : IRequestHandler<GetBodyByName, BodyDto?>
{
    public async Task<BodyDto?> Handle(GetBodyByName request, CancellationToken cancellationToken)
        => mapper.Map<BodyDto?>(await repository.GetBodyByNameAsync(request.Name, cancellationToken));
}
