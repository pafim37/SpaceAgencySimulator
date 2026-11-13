using AutoMapper;
using MediatR;
using Sas.Body.Business.Models.Queries;
using Sas.Body.Core.DataTransferObject;
using Sas.Body.Core.Repositories;

namespace Sas.Body.Business.Handlers
{
    // TODO: Consider renaming this handler to GetEnabledBodiesHandler for clarity.
    // TODO: SHould it return null?
    public class GetEnabledBodyHandler(IBodyRepository repository, IMapper mapper) : IRequestHandler<GetEnabledBodyRequest, IEnumerable<BodyDto?>>
    {
        public async Task<IEnumerable<BodyDto?>> Handle(GetEnabledBodyRequest request, CancellationToken cancellationToken)
            => mapper.Map<IEnumerable<BodyDto?>>(await repository.GetEnabledBodiesAsync(cancellationToken).ConfigureAwait(false));
    }
}
