using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Sas.BodySystem.Models;
using Sas.Dal.BodyDataAccessLayer.Mapping;
using Sas.Db.BodyDatabase.Data;
using Sas.Db.BodyDatabase.Documents;

namespace Sas.Dal.BodyDataAccessLayer.Repositories
{
    public class BodyRepository : IBodyRepository
    {
        private readonly IBodyDatabase _context;
        private readonly BodyMap _bodyMap;

        public BodyRepository(IBodyDatabase context)
        {
            _bodyMap = new BodyMap();
            _context = context;
        }

        // Create
        public async Task<BodyDocument> CreateAsync(Body body)
        {
            var bodyDocument = _bodyMap.Map(body);
            await _context.Bodies.InsertOneAsync(bodyDocument);
            return bodyDocument;
        }

        // Read
        public async Task<Body> GetAsync(string name)
        {
            var bodyDocument = await _context.Bodies.Find(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            var body = _bodyMap.Map(bodyDocument);
            return body;
        }

        // Read All
        public async Task<IEnumerable<Body>> GetAsync()
        {
            var bodyDocuments = await _context.Bodies.Find(b => true).ToListAsync();
            var bodies = _bodyMap.Map(bodyDocuments);
            return bodies;
        }
        
        // Update
        public async Task UpdateAsync(string name, Body body)
        {
            var bodyDocument = _bodyMap.Map(body);
            await _context.Bodies.ReplaceOneAsync(b => b.Name.Equals(name), bodyDocument);
        }

        // Remove
        public async Task RemoveAsync(string name)
        {
            await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(name));
        }

        // Remove
        public async Task RemoveAsync(Body body)
        {
            var bodyDocument = _bodyMap.Map(body);
            await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(bodyDocument.Name));
        }
    }
}
