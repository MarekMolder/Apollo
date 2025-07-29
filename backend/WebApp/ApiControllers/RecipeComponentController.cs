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
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public class RecipeComponentController : ControllerBase
    {
        private readonly ILogger<RecipeComponentController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly RecipeComponentAPIMapper _mapper = new();

        public RecipeComponentController(IAppBLL bll, ILogger<RecipeComponentController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all persons for current user
        /// </summary>
        /// <returns>List of persons</returns>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<RecipeComponent> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<RecipeComponent>>> GetActions()
        {
            return (await _bll.RecipeComponentService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeComponent>> GetActionEntity(Guid id)
        {
            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id);

            if (recipeComponent == null)
            {
                return NotFound();
            }

            return _mapper.Map(recipeComponent)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, RecipeComponent recipeComponent)
        {
            if (id != recipeComponent.Id)
            {
                return BadRequest();
            }

            await _bll.RecipeComponentService.UpdateAsync(_mapper.Map(recipeComponent)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<RecipeComponent>> PostActionEntity(RecipeComponent recipeComponent)
        {
            var bllEntity = _mapper.Map(recipeComponent);
            _bll.RecipeComponentService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetRecipeComponent", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, recipeComponent);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.RecipeComponentService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
