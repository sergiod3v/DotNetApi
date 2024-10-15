using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO {
    public class AddRegionDto {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
