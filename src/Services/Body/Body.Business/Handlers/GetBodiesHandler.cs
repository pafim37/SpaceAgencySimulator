using MediatR;
using Sas.Body.Business.Models.Queries;
using AutoMapper;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers;

public sealed class GetBodiesHandler(IBodyRepository repository, IMapper mapper) : IRequestHandler<GetBodiesRequest, IEnumerable<BodyDto>>
{
    public async Task<IEnumerable<BodyDto>> Handle(GetBodiesRequest request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<BodyDto>>(await repository.GetAllBodiesAsync(cancellationToken));
}
