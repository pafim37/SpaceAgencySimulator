using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Sas.Db.BodyDatabase.Data;
using Sas.Db.BodyDatabase.Documents;

namespace Sas.Db.BodyDatabase.Repositories
{
    public class BodyRepository : IBodyRepository
    {
        private readonly IBodyDatabase _context;

        public BodyRepository(IBodyDatabase context)
        {
            _context = context;
        }

        public async Task<BodyDocument> CreateAsync(BodyDocument bodyDocument)
        {
            await _context.Bodies.InsertOneAsync(bodyDocument);
            return bodyDocument;
        }

        public async Task<BodyDocument> GetAsync(string name)
        {
            var body = await _context.Bodies.Find<BodyDocument>(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            return body;
        }

        public async Task<IEnumerable<BodyDocument>> GetAsync()
        {
            var bodies = await _context.Bodies.Find<BodyDocument>(b => true).ToListAsync();
            return bodies;
        }

        public async Task RemoveAsync(string name)
        {
            await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(name));
        }

        public async Task RemoveAsync(BodyDocument bodyDocument)
        {
            await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(bodyDocument.Name));
        }

        public async Task UpdateAsync(string name, BodyDocument bodyDocument)
        {
            await _context.Bodies.ReplaceOneAsync(b => b.Name.Equals(name), bodyDocument);
        }
    }
}
