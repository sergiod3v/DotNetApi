﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO {
    public class UpdateWalkDto {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

    }

}
