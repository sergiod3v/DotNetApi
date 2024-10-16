using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO {
    public class UpdateRegionDto {

        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters long to update Region")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters long to update Region")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum 100 characters long to update Region")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
