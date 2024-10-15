using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO {
    public class RegionDto {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

        // Constructor that initializes the DTO from the Region object
        public RegionDto(Region? region) {
            Id = region.Id;
            Code = region.Code;
            Name = region.Name;
            RegionImageUrl = region.RegionImageUrl;
        }

        public RegionDto() {
        }
    }
}