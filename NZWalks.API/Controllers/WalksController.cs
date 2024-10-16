﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Filters;
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
        [Authorize(Roles = "Admin,Reader")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? ascending,
            [FromQuery] int? page,
            [FromQuery] int? limit
        ) {
            // Fetch the list of regions from the database
            List<Walk> walkModels = await walkRepository.GetAllAsync(
                filterOn,
                filterQuery,
                sortBy,
                ascending ?? true, // if null pass true
                page ?? 1,
                limit ?? 100

            );
            List<WalkDto> walks = mapper.Map<List<WalkDto>>(walkModels);

            // Return the list of DTOs, or NotFound if it's empty
            if (walks == null || walks.Count == 0) return NotFound();

            return Ok(walks);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            Walk? walkModel = await walkRepository.GetByIdAsync(id);
            if (walkModel == null) return BadRequest(ModelState);

            WalkDto walk = mapper.Map<WalkDto>(walkModel);
            if (walk == null) return StatusCode(500, "Error retreiving walk"); return Ok(walk);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto) {

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
        [Authorize(Roles = "Admin,Writer")]
        [Route(ID)]
        [ValidateModel]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateWalkDto updateWalkDto
        ) {
            Walk body = mapper.Map<Walk>(updateWalkDto);

            Walk? updatedWalk = await walkRepository.UpdateAsync(id, body);

            if (updatedWalk == null) return NotFound();

            WalkDto region = mapper.Map<WalkDto>(updatedWalk);
            return Ok(region);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Writer")]
        [Route(ID)]
        [ValidateModel]
        public async Task<IActionResult> Delete([FromRoute] Guid id) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Walk? deletedWalk = await walkRepository.DeleteAsync(id);
            if (deletedWalk == null) return NotFound();

            WalkDto walk = mapper.Map<WalkDto>(deletedWalk);
            if (walk == null) return StatusCode(500, "Error deleting walk");

            return Ok(walk);
        }
    }
}
