using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper) {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // Fetch the list of regions from the database
            List<Region> regionsModel = await regionRepository.GetAllAsync();
            List<RegionDto> regions = mapper.Map<List<RegionDto>>(regionsModel);

            // Return the list of DTOs, or NotFound if it's empty
            if (regions == null || regions.Count == 0) return NotFound();

            return Ok(regions);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            Region? regionModel = await regionRepository.GetByIdAsync(id);
            if (regionModel == null) return NotFound();

            RegionDto region = mapper.Map<RegionDto>(regionModel);
            if (region == null) return NotFound(); return Ok(region);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDto addRegionDto) {
            // Validate the incoming DTO
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create raw region based on DTO
            Region regionModel = mapper.Map<Region>(addRegionDto);

            // Call repository to insert the region into the database
            Region? createdRegion = await regionRepository.CreateAsync(regionModel);
            if (createdRegion == null) { return StatusCode(500, "An error occurred while creating the region."); }

            // Create & return DTO to avoid exposing domain model
            RegionDto region = mapper.Map<RegionDto>(createdRegion);
            return CreatedAtAction(nameof(GetById), new { id = region.Id }, region);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto) {
            // Validate the incoming DTO
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create the Region object to update
            Region body = mapper.Map<Region>(updateRegionDto);

            // Attempt to update the region in the repository
            Region? updatedRegion = await regionRepository.UpdateAsync(id, body);
            if (updatedRegion == null) return NotFound();

            RegionDto region = mapper.Map<RegionDto>(updatedRegion);
            return Ok(region);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {

            // null if no region<id> was found 
            Region? deletedRegion = await regionRepository.DeleteAsync(id);
            if (deletedRegion == null) return NotFound();

            // DTO created & sent to the user (not the just updated model)
            RegionDto region = mapper.Map<RegionDto>(deletedRegion);
            if (region == null) return NotFound(); return Ok(region);
        }
    }

}
