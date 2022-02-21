using MediatR;
using Sas.Domain.Bodies;
using Sas.SolarSystem.Application.Queries;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.SolarSystem.Application.Handlers
{
    public class GetAllBodiesHandler
        : IRequestHandler<GetAllBodiesQuery, IEnumerable<BodyDocument>>
    {
        private readonly IBodyRepository _repository;

        public GetAllBodiesHandler(IBodyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BodyDocument>> Handle(GetAllBodiesQuery request, CancellationToken cancellationToken)
        {
            var bodies = await _repository.GetAsync();
            return bodies;
        }
    }
}
