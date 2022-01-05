using MongoDB.Driver;
using Sas.Db.BodyDatabase.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.BodyDatabase.Data
{
    public interface IBodyDatabase
    {
         IMongoCollection<BodyDocument> Bodies { get; set; }
    }
}
