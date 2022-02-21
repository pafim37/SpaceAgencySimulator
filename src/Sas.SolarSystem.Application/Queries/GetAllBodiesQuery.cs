using MediatR;
using Sas.Domain.Bodies;
using Sas.SolarSystem.Service.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.SolarSystem.Application.Queries
{
    public class GetAllBodiesQuery 
        : IRequest<IEnumerable<BodyDocument>>
    {
    }
}
