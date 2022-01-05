using MongoDB.Driver;
using Sas.Db.BodyDatabase.Documents;
using Sas.Db.BodyDatabase.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Db.BodyDatabase.Data
{
    public class BodyDatabase : IBodyDatabase
    {
        public IMongoCollection<BodyDocument> Bodies { get; set; }

        public BodyDatabase(IBodyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Bodies = database.GetCollection<BodyDocument>(settings.CollectionName);
        }
    }
}
