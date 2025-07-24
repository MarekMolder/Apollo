using App.BLL.Contracts;
using App.DTO.v1;
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
    [Authorize(Roles = "admin,manager,tootaja")]
    public class ReasonsController : ControllerBase
    {
        private readonly ILogger<ReasonsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly ReasonAPIMapper _mapper = new();

        public ReasonsController(IAppBLL bll, ILogger<ReasonsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<Reason> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Reason>>> GetActions()
        {
            return (await _bll.ReasonService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Reason>> GetActionEntity(Guid id)
        {
            var reason = await _bll.ReasonService.FindAsync(id);

            if (reason == null)
            {
                return NotFound();
            }

            return _mapper.Map(reason)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, Reason reason)
        {
            if (id != reason.Id)
            {
                return BadRequest();
            }

            await _bll.ReasonService.UpdateAsync(_mapper.Map(reason)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Reason>> PostActionEntity(Reason reason)
        {
            var bllEntity = _mapper.Map(reason);
            _bll.ReasonService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, reason);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.ReasonService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
