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
    /// Api controller for managing monthly statistics.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,juhataja")]
    public class MonthlyStatisticsController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<MonthlyStatisticsController> _logger;
        
        private readonly MonthlyStatisticsApiMapper _mapper = new();
        
        private readonly EnrichedMonthlyStatisticsApiMapper _enrichedMonthlyStatisticsApiMapper = new();

        public MonthlyStatisticsController(IAppBll bll, ILogger<MonthlyStatisticsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all monthly statistics
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<MonthlyStatistics> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<MonthlyStatistics>>> GetMonthlyStatistics()
        {
            _logger.LogInformation("Fetching all monthly statistics");
            
            var stats = (await _bll.MonthlyStatisticsService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} monthly statistics", stats.Count);
            return stats;
        }

        /// <summary>
        /// Get monthly statistics by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MonthlyStatistics>> GetMonthlyStatistics(Guid id)
        {
            _logger.LogInformation("Fetching monthly statistics with ID {Id}", id);
            
            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id);

            if (monthlyStatistics == null)
            {
                _logger.LogWarning("Monthly statistics with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(monthlyStatistics)!;
        }

        /// <summary>
        /// Update monthly statistics
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonthlyStatistics(Guid id, MonthlyStatistics monthlyStatistics)
        {
            if (id != monthlyStatistics.Id)
            {
                _logger.LogWarning("PutMonthlyStatistics failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, monthlyStatistics.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating monthly statistics with ID {Id}", id);
            await _bll.MonthlyStatisticsService.UpdateAsync(_mapper.Map(monthlyStatistics)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new monthly statistics
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MonthlyStatistics>> PostMonthlyStatistics(MonthlyStatistics monthlyStatistics)
        {
            _logger.LogInformation("Creating new monthly statistics");
            
            var bllEntity = _mapper.Map(monthlyStatistics);
            _bll.MonthlyStatisticsService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created monthly statistics with ID {Id}", bllEntity.Id);
            
            return CreatedAtAction("GetMonthlyStatistics", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, monthlyStatistics);
        }

        /// <summary>
        /// Delete monthly statistics by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            _logger.LogInformation("Deleting monthly statistics with ID {Id}", id);

            await _bll.MonthlyStatisticsService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted monthly statistics with ID {Id}", id);
            return NoContent();
        }
        
        /// <summary>
        /// Get monthly statistics by storage room ID
        /// </summary>
        [HttpGet("bystorageroom/{id}")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedMonthlyStatistics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedMonthlyStatistics>>> GetByStorageRoom(Guid id)
        {
            _logger.LogInformation("Fetching monthly statistics by storage room ID {Id}", id);
            var stocks = await _bll.MonthlyStatisticsService.GetByStorageRoomIdAsync(id);
            var res = stocks.Select(x => _enrichedMonthlyStatisticsApiMapper.Map(x)!);
            return Ok(res);
        }
        
        /// <summary>
        /// Get enriched monthly statistics
        /// </summary>
        [HttpGet("enrichedMonthlyStatistics/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedMonthlyStatistics>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedMonthlyStatistics>>> GetEnrichedMonthlyStatistics()
        {
            _logger.LogInformation("Fetching enriched monthly statistics");
            var data = await _bll.MonthlyStatisticsService.GetEnrichedMonthlyStatistics();
            var res = data.Select(u => _enrichedMonthlyStatisticsApiMapper.Map(u)!).ToList();
            
            _logger.LogInformation("Returned {Count} enriched monthly statistics", res.Count);
            return Ok(res);
        }
        
        /// <summary>
        /// Get converted removed quantity for a MonthlyStatistics record.
        /// </summary>
        [HttpGet("converted/{monthlyStatisticsId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConvertedQuantity(Guid monthlyStatisticsId, [FromQuery] string targetUnit)
        {
            var stats = await _bll.MonthlyStatisticsService.FindAsync(monthlyStatisticsId);
            if (stats == null) return NotFound();

            var product = await _bll.ProductService.FindAsync(stats.ProductId);
            if (product == null) return NotFound();

            try
            {
                var converted = ConvertUnits(stats.TotalRemovedQuantity, product.Unit, targetUnit);
                return Ok($"{converted:F2} {targetUnit} (from {product.Unit})");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Get converted removed volume for a MonthlyStatistics record.
        /// </summary>
        [HttpGet("converted-volume/{monthlyStatisticsId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetConvertedRemovedVolume(Guid monthlyStatisticsId, [FromQuery] string targetUnit)
        {
            var stats = await _bll.MonthlyStatisticsService.FindAsync(monthlyStatisticsId);
            if (stats == null) return NotFound("MonthlyStatistics not found");

            var product = await _bll.ProductService.FindAsync(stats.ProductId);
            if (product == null) return NotFound("Product not found");
            
            if (!string.Equals(product.Unit, "tk", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Removed volume conversion is only applicable when product unit is 'tk'.");

            if (product.Volume == null || product.Volume <= 0 || string.IsNullOrWhiteSpace(product.VolumeUnit))
                return BadRequest("Product volume and volume unit must be set for 'tk' products.");

            var baseValue = stats.TotalRemovedQuantity * product.Volume;
            var fromUnit = product.VolumeUnit!;

            try
            {
                decimal result;
                if (string.Equals(fromUnit, targetUnit, StringComparison.OrdinalIgnoreCase))
                {
                    result = baseValue;
                }
                else
                {
                    result = ConvertUnits(baseValue, fromUnit, targetUnit);
                }

                return Ok($"{result:F2} {targetUnit} (from {fromUnit})");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Unit conversion helper function.
        /// </summary>
        private decimal ConvertUnits(decimal value, string from, string to)
        {
            var conversionRates = new Dictionary<(string from, string to), decimal>
            {
                // Massi teisendused
                { ("g", "kg"), 0.001m },
                { ("kg", "g"), 1000m },
                { ("g", "mg"), 1000m },
                { ("mg", "g"), 0.001m },

                // Mahu teisendused
                { ("ml", "l"), 0.001m },
                { ("l", "ml"), 1000m },
                { ("ml", "cl"), 0.1m },
                { ("l", "cl"), 100m },

                // Mass -> maht
                { ("g", "ml"), 1m },
                { ("g", "l"), 0.001m },
                { ("kg", "ml"), 1000m },
                { ("kg", "l"), 1m },
                
                // Maht -> mass (eeldame tihedus 1 g/ml)
                { ("ml", "g"), 1m },
                { ("l", "g"), 1000m },
                { ("ml", "kg"), 0.001m },
                { ("l", "kg"), 1m }
            };

            if (from == to) return value;

            if (conversionRates.TryGetValue((from, to), out var rate))
                return value * rate;

            throw new Exception($"Unsupported conversion from {from} to {to}");
        }
    }
}
