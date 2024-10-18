using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data {
    public class NZWalksAuthDbContext : IdentityDbContext {
        public NZWalksAuthDbContext(
            DbContextOptions<NZWalksAuthDbContext> options
        ) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            /*
                25334fc5-e142-4a0c-8897-d5fc8edce75d
                0a0a9b69-3ec3-4a25-a734-e296e5e7954c
                0cbea234-eff1-412e-bd74-357c81c04de3
                2525c5f3-cac6-4b5c-be4b-0a31ad596629
            */

            string readerId = "513d003e-ec3c-41df-8d6c-d13d4a40d6f5";
            string writerId = "1bdb8167-86ab-4fd0-a095-7a154c52bd6e";

            List<IdentityRole> roles = [
                new () {
                    Id = readerId,
                    ConcurrencyStamp = readerId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new() {
                    Id = writerId,
                    ConcurrencyStamp = writerId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            ];

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}