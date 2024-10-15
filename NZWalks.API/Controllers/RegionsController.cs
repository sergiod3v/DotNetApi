using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase {
        private readonly NZWalksDbContext dbContext; // property
        private readonly IRegionRepository regionRepository;

        // Service & dbContext will be loaded will be loaded
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository) {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // Fetch the list of regions from the database
            var regionsModel = await regionRepository.GetAllAsync();

            // Create a list of RegionDTOs to return
            var regionsDto = new List<RegionDto>();

            // Iterate over the regionsModel (not regionsDto)
            foreach (var region in regionsModel) {
                // Add each region as a RegionDTO to the DTO list
                regionsDto.Add(new RegionDto(region));
            }

            // Return the list of DTOs, or NotFound if it's empty
            if (regionsDto == null || regionsDto.Count == 0) {
                return NotFound();
            }

            return Ok(regionsDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            var regionModel = await regionRepository.GetByIdAsync(id);
            if (regionModel == null) return NotFound();

            RegionDto regionDto = new(regionModel);
            if (regionDto == null) return NotFound(); return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDto addRegionDto) {
            // Validate the incoming DTO
            if (!ModelState.IsValid) return BadRequest(ModelState);


            // Create raw region based on DTO
            Region region = new() {
                Code = addRegionDto.Code,
                Name = addRegionDto.Name,
                RegionImageUrl = addRegionDto.RegionImageUrl,
            };

            // Call repository to insert the region into the database
            var createdRegion = await regionRepository.CreateAsync(region);
            if (createdRegion == null)
                return StatusCode(500, "An error occurred while creating the region.");

            // Create & return DTO to avoid exposing domain model
            RegionDto regionDto = new(createdRegion);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto) {
            // Validate the incoming DTO
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create the Region object to update
            Region body = new() {
                Name = updateRegionDto.Name,
                Code = updateRegionDto.Code,
                RegionImageUrl = updateRegionDto.RegionImageUrl,
            };

            // Attempt to update the region in the repository
            var updatedRegion = await regionRepository.UpdateAsync(id, body);
            if (updatedRegion == null) return NotFound();

            // Convert the updated model to DTO for the response
            RegionDto regionDto = new (updatedRegion);
            return Ok(regionDto);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            var region = await regionRepository.DeleteAsync(id);
            if (region == null) return NotFound();

            // DTO created & sent to the user (not the just updated model)
            RegionDto regionDto = new(region);
            if (regionDto == null) return NotFound(); return Ok(regionDto);
        }
    }

}
