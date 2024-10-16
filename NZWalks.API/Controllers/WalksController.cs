using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase {
        private const string ID = "{id:Guid}";
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // Fetch the list of regions from the database
            List<Walk> walkModels = await walkRepository.GetAllAsync();
            List<WalkDto> walks = mapper.Map<List<WalkDto>>(walkModels);

            // Return the list of DTOs, or NotFound if it's empty
            if (walks == null || walks.Count == 0) return NotFound();

            return Ok(walks);
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            Walk? walkModel = await walkRepository.GetByIdAsync(id);
            if (walkModel == null) return NotFound();

            WalkDto walk = mapper.Map<WalkDto>(walkModel);
            if (walk == null) return StatusCode(500, "Error retreiving walk"); return Ok(walk);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto) {
            if (addWalkDto == null) return BadRequest("Invalid payload");

            // Map the DTO to the domain model
            Walk walkModel = mapper.Map<Walk>(addWalkDto);

            // Create the walk in the repository
            Walk? createdWalk = await walkRepository.CreateAsync(walkModel);

            // Map the created walk back to the DTO for the response
            WalkDto walkDto = mapper.Map<WalkDto>(createdWalk);

            if (walkDto == null) return StatusCode(500, "Error creating walk");

            return Ok(walkDto);
        }


        [HttpPut]
        [Route(ID)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateWalkDto updateWalkDto
        ) {
            // Validate the incoming DTO
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create the Region object to update
            Walk body = mapper.Map<Walk>(updateWalkDto);

            // Attempt to update the region in the repository
            Walk? updatedWalk = await walkRepository.UpdateAsync(id, body);
            if (updatedWalk == null) return NotFound();

            WalkDto region = mapper.Map<WalkDto>(updatedWalk);
            return Ok(region);
        }

        [HttpDelete]
        [Route(ID)]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            Walk? deletedWalk = await walkRepository.DeleteAsync(id);
            if (deletedWalk == null) return NotFound();

            WalkDto walk = mapper.Map<WalkDto>(deletedWalk);
            if (walk == null) return StatusCode(500, "Error deleting walk");

            return Ok(walk);
        }
    }
}
