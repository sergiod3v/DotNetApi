using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data {
    public class NZWalksDbContext : DbContext {
        // DbContextOptions -> params like connection_string, provider and more
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {

        }

        // Entity Set -> Entities mapped to EF db handler
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
