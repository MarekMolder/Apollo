using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing reasons.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager,tootaja")]
    public class ReasonsController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ReasonsController> _logger;
        
        private readonly ReasonApiMapper _mapper = new();

        public ReasonsController(IAppBll bll, ILogger<ReasonsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all reasons
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<Reason> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Reason>>> GetReasons()
        {
            _logger.LogInformation("Fetching all reasons");
            var result = (await _bll.ReasonService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} reasons", result.Count);
            return result;        }

        /// <summary>
        /// Get reason by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Reason>> GetReason(Guid id)
        {
            _logger.LogInformation("Fetching reason with ID {Id}", id);
            var reason = await _bll.ReasonService.FindAsync(id);

            if (reason == null)
            {
                _logger.LogWarning("Reason with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(reason)!;
        }

        /// <summary>
        /// Update reason
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReason(Guid id, Reason reason)
        {
            if (id != reason.Id)
            {
                _logger.LogWarning("PutReason failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, reason.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating reason with ID {Id}", id);
            await _bll.ReasonService.UpdateAsync(_mapper.Map(reason)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new reason
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Reason>> PostReason(Reason reason)
        {
            _logger.LogInformation("Creating new reason");
            var bllEntity = _mapper.Map(reason);
            _bll.ReasonService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created reason with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetAddress", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, reason);
        }

        /// <summary>
        /// Delete reason by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReason(Guid id)
        {
            _logger.LogInformation("Deleting reason with ID {Id}", id);
            await _bll.ReasonService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted reason with ID {Id}", id);
            return NoContent();
        }
    }
}
