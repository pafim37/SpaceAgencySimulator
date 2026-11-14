using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers
{
    public class GetEnabledBodiesHandler(IBodyRepository repository, IMapper mapper) : IRequestHandler<GetEnabledBodiesRequest, IEnumerable<BodyDto>>
    {
        public async Task<IEnumerable<BodyDto>> Handle(GetEnabledBodiesRequest request, CancellationToken cancellationToken)
            => mapper.Map<IEnumerable<BodyDto>>(await repository.GetEnabledBodiesAsync(cancellationToken).ConfigureAwait(false));
    }
}
