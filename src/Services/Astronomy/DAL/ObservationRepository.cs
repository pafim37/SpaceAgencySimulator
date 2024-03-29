﻿using Sas.Astronomy.Service.Data;
using Sas.Astronomy.Service.Models;
using System.Data.Entity;

namespace Sas.Astronomy.Service.DAL
{
    public class ObservationRepository
    {
        private readonly AstronomyContext _context;
        public ObservationRepository(AstronomyContext context)
        {
            _context = context;
        }

        // Read All
        public async Task<IEnumerable<ObservationEntity>> GetAsync()
        {
            List<ObservationEntity> observations = await _context.Set<ObservationEntity>().ToListAsync();
            foreach (ObservationEntity? observation in observations)
            {
                ObservatoryEntity observatory = await _context.Set<ObservatoryEntity>().Where(x => x.Id == observation.ObservatoryId).FirstOrDefaultAsync();
                observation.Observatory = observatory;
            }
            return observations;
        }

        // Read by id
        public async Task<ObservationEntity> GetAsync(int id)
        {
            ObservationEntity observation = await _context.Set<ObservationEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            observation.Observatory = await _context.Set<ObservatoryEntity>().Where(x => x.Id == observation.ObservatoryId).FirstOrDefaultAsync();
            return observation;
        }

        // Read by object name
        public async Task<IEnumerable<ObservationEntity>> GetAsync(string objectName)
        {
            List<ObservationEntity> observations = await _context.Set<ObservationEntity>().Where(x => x.ObjectName.Equals(objectName)).ToListAsync();
            foreach (ObservationEntity? observation in observations)
            {
                observation.Observatory = await _context.Set<ObservatoryEntity>().Where(x => x.Id == observation.ObservatoryId).FirstOrDefaultAsync();
            }
            return observations;
        }

        // Create 
        public async Task<ObservationEntity> CreateAsync(ObservationEntity observation)
        {
            _context.Set<ObservationEntity>().Add(observation);
            await _context.SaveChangesAsync();
            return observation;
        }
    }
}
