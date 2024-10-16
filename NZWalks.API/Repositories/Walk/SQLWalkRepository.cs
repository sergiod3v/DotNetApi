using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public class SQLWalkRepository(NZWalksDbContext dbContext) : IWalkRepository {
        private readonly NZWalksDbContext dbContext = dbContext;

        public async Task<List<Walk>> GetAllAsync() {
            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id) {
            return await dbContext.Walks
                .Include(w => w.Difficulty) // Use lambda expression for clarity
                .Include(w => w.Region)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<Walk?> CreateAsync(Walk walk) {
            // Add the new walk to the context and get the EntityEntry
            var newWalk = await dbContext.Walks.AddAsync(walk);

            // Save changes to persist the new walk in the database
            await dbContext.SaveChangesAsync();

            // Return the complete walk object by fetching it using its ID
            return await GetByIdAsync(newWalk.Entity.Id);
        }


        public async Task<Walk?> UpdateAsync(Guid id, Walk updatedWalk) {
            Walk? currentWalk = await GetByIdAsync(id);
            if (currentWalk == null) return null;

            currentWalk.Name = updatedWalk.Name;
            currentWalk.Description = updatedWalk.Description;
            currentWalk.LengthInKm = updatedWalk.LengthInKm;
            currentWalk.WalkImageUrl = updatedWalk.WalkImageUrl;
            currentWalk.DifficultyId = updatedWalk.DifficultyId;
            currentWalk.RegionId = updatedWalk.RegionId;

            await dbContext.SaveChangesAsync();

            return await GetByIdAsync(currentWalk.Id);
        }

        public async Task<Walk?> DeleteAsync(Guid id) {
            Walk? currentWalk = await GetByIdAsync(id);
            if (currentWalk == null) return null;

            dbContext.Walks.Remove(currentWalk);
            await dbContext.SaveChangesAsync();

            return currentWalk;
        }
    }
}
