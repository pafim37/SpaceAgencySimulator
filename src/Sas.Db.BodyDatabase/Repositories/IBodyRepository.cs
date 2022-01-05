using Sas.Db.BodyDatabase.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.BodyDatabase.Repositories
{
    internal interface IBodyRepository
    {
        Task<BodyDocument> GetAsync(string name);
        Task<IEnumerable<BodyDocument>> GetAsync();
        Task<BodyDocument> CreateAsync(BodyDocument bodyDocument);
        Task UpdateAsync(string name, BodyDocument bodyDocument);
        Task RemoveAsync(string name);
    }
}
