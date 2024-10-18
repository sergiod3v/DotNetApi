using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public class SQLWalkRepository(NZWalksDbContext dbContext) : IWalkRepository {
        private readonly NZWalksDbContext dbContext = dbContext;

        public async Task<List<Walk>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int limit = 100
        ) {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery)) {
                if (filterOn.ToLower().Equals("name")) {
                    walks = walks.Where(
                        x => x.Name.Contains(filterQuery)
                    );
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy)) {
                if (sortBy.ToLower().Equals("name")) {
                    walks = ascending ?
                    walks.OrderBy(x => x.Name) :
                    walks.OrderByDescending(x => x.Name);
                } else if (sortBy.ToLower().Equals("length")) {
                    walks = ascending ?
                    walks.OrderBy(x => x.LengthInKm) :
                    walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (page - 1) * limit;

            return await walks.Skip(skipResults).Take(limit).ToListAsync(); //this is the async part, not the rest of the methods
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
