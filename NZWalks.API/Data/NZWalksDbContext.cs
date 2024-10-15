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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Create data to seed 
            List<Difficulty> difficulties = [
                new Difficulty() { Id = Guid.Parse("9e336c61-faed-4281-82d9-639537cd1d4a"), Name = "Easy" },
                new Difficulty() { Id = Guid.Parse("66f0a310-3f96-4298-a126-adf7fa491c79"), Name = "Medium" },
                new Difficulty() { Id = Guid.Parse("539f569a-1044-44e0-94b1-4c68834f6d1d"), Name = "Hard" },
            ];

            // Seed Difficulty table
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Create data to seed 
            List<Region> regions = [
                new Region() {
                    Id = Guid.Parse("39deb490-e199-48ca-80d5-d6ccdd21ab18"),
                    Code = "AWK", Name = "Auckland", RegionImageUrl = "regionurl1.jpg"
                },
                new Region() {
                    Id = Guid.Parse("ff7397f5-2cb3-41d1-b2e6-1ad96bc0c927"),
                    Code = "NES", Name = "Nessieuwu", RegionImageUrl = "regionurl2.jpg"
                },
                new Region() {
                    Id = Guid.Parse("4e86ea9f-e912-4a82-bf40-af1085bff5a4"),
                    Code = "SAC", Name = "Sergio", RegionImageUrl = "regionurl3.jpg"
                },
            ];

            // Seed Difficulty table
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
