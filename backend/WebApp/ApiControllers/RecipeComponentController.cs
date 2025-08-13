using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMapper;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing recipe components.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public class RecipeComponentController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<RecipeComponentController> _logger;
        
        private readonly RecipeComponentApiMapper _mapper = new();
        
        private readonly EnrichedRecipeComponentApiMapper _enrichedRecipeComponentApiMapper = new();

        public RecipeComponentController(IAppBll bll, ILogger<RecipeComponentController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all recipe components.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<RecipeComponent> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<RecipeComponent>>> GetRecipeComponents()
        {
            _logger.LogInformation("Fetching all recipe components");
            var result = (await _bll.RecipeComponentService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} recipe components", result.Count);
            return result;        }

        /// <summary>
        /// Get recipe component by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeComponent>> GetRecipeComponent(Guid id)
        {
            _logger.LogInformation("Fetching recipe component with ID {Id}", id);
            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id);

            if (recipeComponent == null)
            {
                _logger.LogWarning("Recipe component with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(recipeComponent)!;
        }

        /// <summary>
        /// Update recipe component.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipeComponent(Guid id, RecipeComponent recipeComponent)
        {
            if (id != recipeComponent.Id)
            {
                _logger.LogWarning("PutRecipeComponent failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, recipeComponent.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating recipe component with ID {Id}", id);
            await _bll.RecipeComponentService.UpdateAsync(_mapper.Map(recipeComponent)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new recipe component.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RecipeComponent>> PostRecipeComponent(RecipeComponent recipeComponent)
        {
            _logger.LogInformation("Creating new recipe component");
            var bllEntity = _mapper.Map(recipeComponent);
            _bll.RecipeComponentService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            _logger.LogInformation("Created recipe component with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetRecipeComponent", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, recipeComponent);
        }

        /// <summary>
        /// Delete recipe component by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeComponent(Guid id)
        {
            _logger.LogInformation("Deleting recipe component with ID {Id}", id);
            await _bll.RecipeComponentService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted recipe component with ID {Id}", id);
            return NoContent();
        }
        
        /// <summary>
        /// Get enriched recipe components
        /// </summary>
        [HttpGet("enrichedRecipeComponents/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedRecipeComponent>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedRecipeComponent>>> GetEnrichedRecipeComponents()
        {
            _logger.LogInformation("Fetching enriched recipe components");
            var data = await _bll.RecipeComponentService.GetEnrichedRecipeComponents();
            var res = data.Select(u => _enrichedRecipeComponentApiMapper.Map(u)!).ToList();
            
            _logger.LogInformation("Returned {Count} enriched recipeComponents", res.Count);
            return Ok(res);
        }
    }
}
