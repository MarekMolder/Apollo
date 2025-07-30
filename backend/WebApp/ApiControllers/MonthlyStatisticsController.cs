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
    public class MonthlyStatisticsController : ControllerBase
    {
        private readonly ILogger<MonthlyStatisticsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly MonthlyStatisticsAPIMapper _mapper = new();
        
        private readonly EnrichedMonthlyStatisticsAPIMapper _enrichedMonthlyStatisticsAPIMapper = new();

        public MonthlyStatisticsController(IAppBLL bll, ILogger<MonthlyStatisticsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<MonthlyStatistics> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<MonthlyStatistics>>> GetActions()
        {
            return (await _bll.MonthlyStatisticsService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MonthlyStatistics>> GetActionEntity(Guid id)
        {
            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id);

            if (monthlyStatistics == null)
            {
                return NotFound();
            }

            return _mapper.Map(monthlyStatistics)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, MonthlyStatistics monthlyStatistics)
        {
            if (id != monthlyStatistics.Id)
            {
                return BadRequest();
            }

            await _bll.MonthlyStatisticsService.UpdateAsync(_mapper.Map(monthlyStatistics)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MonthlyStatistics>> PostActionEntity(MonthlyStatistics monthlyStatistics)
        {
            var bllEntity = _mapper.Map(monthlyStatistics);
            _bll.MonthlyStatisticsService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetMonthlyStatistics", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, monthlyStatistics);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.MonthlyStatisticsService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpGet("bystorageroom/{id}")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedMonthlyStatistics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedMonthlyStatistics>>> GetByStorageRoom(Guid id)
        {
            var stocks = await _bll.MonthlyStatisticsService.GetByStorageRoomIdAsync(id);
            var res = stocks.Select(x => _mapper.Map(x)!);
            return Ok(res);
        }
        
        [HttpGet("enrichedMonthlyStatistics/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedMonthlyStatistics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedMonthlyStatistics>>> GetEnrichedMonthlyStatistics()
        {
            var data = await _bll.MonthlyStatisticsService.GetEnrichedMonthlyStatistics();

            var res = data.Select(u => _enrichedMonthlyStatisticsAPIMapper.Map(u)!).ToList();
            return Ok(res);
        }
    }
}
