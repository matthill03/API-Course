using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            // Fetch data from database
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            // Convert data into DTO object
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);

            // return ok response
            return Ok(walkDifficultyDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            // Get data from data base - as domain model
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            // Check that difficulty exists - if not return not found
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // Return Ok response
            return Ok(walkDifficultyDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // Convert DTO to Domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass walk difficulty to repository to add to database
            var walkDifficltyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            // Convert back to DTO model
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // return created response with DTO back to client
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // Update walk difficulty in database - using repository
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            // if no walk with id then return not found
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert Domain to DTO onject
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // Return ok response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync([FromRoute] Guid id)
        {
            // Delete from database - using repository
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            // check if walk difficulty is found
            if(walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // convert domain into DTO object
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // return ok response with DTO
            return Ok(walkDifficultyDTO);
        }
    }
}
