using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories {
    public class SQLRegionRepository : IRegionRepository {
        private readonly NZWalksDbContext dbContext;
        private readonly NZWalksAuthDbContext authDbContext;

        public SQLRegionRepository(
            NZWalksDbContext dbContext,
            NZWalksAuthDbContext authDbContext
        ) {
            this.dbContext = dbContext;
            this.authDbContext = authDbContext;
        }

        public async Task<List<Region>> GetAllAsync() {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id) {
            return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> CreateAsync(Region region) {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region? region) {
            Region? currentRegion = await GetByIdAsync(id);
            if (currentRegion == null) return null;

            currentRegion.Name = region.Name;
            currentRegion.Code = region.Code;
            currentRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return await GetByIdAsync(currentRegion.Id);
        }

        public async Task<Region?> DeleteAsync(Guid id) {
            Region? currentRegion = await GetByIdAsync(id);
            if (currentRegion == null) return null;

            dbContext.Regions.Remove(currentRegion);
            await dbContext.SaveChangesAsync();

            return currentRegion;
        }
    }
}
