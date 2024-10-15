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
            if (walk == null) return NotFound(); return Ok(walk);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto) {
            Walk body = mapper.Map<Walk>(addWalkDto);

            Walk walkModel = await walkRepository.CreateAsync(body);
            WalkDto walk = mapper.Map<WalkDto>(walkModel);
            if (walk == null) return StatusCode(500, "Error creating walk");

            return Ok(walk);
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
