using AutoMapper;
using MongoDB.Driver;
using Sas.SolarSystem.Service.Data;
using Sas.SolarSystem.Service.Documents;
using Sas.SolarSystem.Service.DTOs;

namespace Sas.SolarSystem.Service.DAL
{
    public class BodyRepository : IBodyRepository
    {
        private readonly IBodyDatabase _context;

        public BodyRepository(IBodyDatabase context)
        {
            _context = context;
        }

        // Create
        //public async Task<BodyDocument> CreateAsync(Body body)
        //{
        //    var bodyDocument = _mapper.Map(body);
        //    await _context.Bodies.InsertOneAsync(bodyDocument);
        //    return bodyDocument;
        //}

        // Read
        public async Task<BodyDocument> GetAsync(string name)
        {
            var body = await _context.Bodies.Find(b => b.Name.Equals(name)).FirstOrDefaultAsync();
            return body;
        }

        // Read All
        public async Task<IEnumerable<BodyDocument>> GetAsync()
        {
            var bodies = await _context.Bodies.Find(b => true).ToListAsync();
            return bodies;
        }

        //// Update
        //public async Task UpdateAsync(string name, BodyDTO body)
        //{
        //    var bodyDocument = _mapper.Map(body);
        //    await _context.Bodies.ReplaceOneAsync(b => b.Name.Equals(name), bodyDocument);
        //}

        //// Remove
        //public async Task RemoveAsync(string name)
        //{
        //    await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(name));
        //}

        //// Remove
        //public async Task RemoveAsync(Body body)
        //{
        //    var bodyDocument = _mapper.Map(body);
        //    await _context.Bodies.DeleteOneAsync(b => b.Name.Equals(bodyDocument.Name));
        //}
    }
}
