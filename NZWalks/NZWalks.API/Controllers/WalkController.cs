using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            // Convert domain to walks DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            // return ok result with also the data
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // collect data from database - domain model
            var walkDomain = await walkRepository.GetAsync(id);

            // check if walk exists - if not return not found
            if (walkDomain == null)
            {
                return NotFound();
            }

            // if does exist convert into DTO model
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // return ok status
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass domain object to repository to persist this
            walkDomain = await walkRepository.AddAsync(walkDomain);

            // Convert the domain object back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // send dto response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };

            // Update Walk in database - using repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // If no walk with id then return not found
            if(walkDomain == null)
            {
                return NotFound();
            }

            // Convert back to DTO from Domain
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Return Ok with DTO walk
            return Ok(walkDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            // Get walk from database and delete it
            var walkDomain = await walkRepository.DeleteAsync(id);

            // Check if walk is null if so return not found
            if(walkDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO object
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // return ok response with DTO object
            return Ok(walkDTO);
        }

    }
}
