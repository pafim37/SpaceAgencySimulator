using Sas.BodySystem.Models;
using Sas.Db.BodyDatabase.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Dal.BodyDataAccessLayer.Repositories
{
    internal interface IBodyRepository
    {
        Task<Body> GetAsync(string name);
        Task<IEnumerable<Body>> GetAsync();
        Task<BodyDocument> CreateAsync(Body body);
        Task UpdateAsync(string name, Body body);
        Task RemoveAsync(string name);
    }
}
