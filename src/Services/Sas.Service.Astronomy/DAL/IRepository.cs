using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Service.Astronomy.DAL
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string name);
        Task<IEnumerable<T>> GetAsync();
    }
}
