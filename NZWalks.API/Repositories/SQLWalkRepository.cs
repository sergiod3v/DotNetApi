using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories {
    public class SQLWalkRepository : IWalkRepository {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<Walk?> GetByIdAsync(Guid id) {
            return await dbContext.Walks.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Walk> CreateAsync(Walk walk) {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id) {
            Walk? currentWalk = await GetByIdAsync(id);
            if (currentWalk == null) return null;

            dbContext.Walks.Remove(currentWalk);
            await dbContext.SaveChangesAsync();

            return currentWalk;
        }

        public async Task<List<Walk>> GetAllAsync() {
            return await dbContext.Walks.ToListAsync();
        }

        public Task<Walk?> UpdateAsync(Guid id, Walk walk) {
            throw new NotImplementedException();
        }
    }
}
