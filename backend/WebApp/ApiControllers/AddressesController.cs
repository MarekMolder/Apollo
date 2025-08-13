using App.BLL.Contracts;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMapper;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Address = App.DTO.v1.Address;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing addresses.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public class AddressesController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<AddressesController> _logger;
        
        private readonly AddressApiMapper _mapper = new();
        
        private readonly EnrichedAddressApiMapper _enrichedAddressApiMapper = new();


        public AddressesController(IAppBll bll, ILogger<AddressesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all addresses.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<Address> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            _logger.LogInformation("Fetching all addresses");

            var addresses = (await _bll.AddressService.AllAsync())
                .Select(x => _mapper.Map(x)!)
                .ToList();

            _logger.LogInformation("Returned {Count} addresses", addresses.Count);
            return addresses;        }

        /// <summary>
        /// Get address by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(Guid id)
        {
            _logger.LogInformation("Fetching address with ID {Id}", id);

            var address = await _bll.AddressService.FindAsync(id);

            if (address == null)
            {
                _logger.LogWarning("Address with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(address)!;
        }

        /// <summary>
        /// Update address by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(Guid id, Address address)
        {
            if (id != address.Id)
            {
                _logger.LogWarning("PutAddress failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, address.Id);
                return BadRequest();
            }
            
            _logger.LogInformation("Updating address with ID {Id}", id);
            await _bll.AddressService.UpdateAsync(_mapper.Map(address)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a new address.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _logger.LogInformation("Creating new address");

            var bllEntity = _mapper.Map(address);
            _bll.AddressService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created address with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetAddress", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, address);
        }

        /// <summary>
        /// Delete address by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            _logger.LogInformation("Deleting address with ID {Id}", id);

            await _bll.AddressService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted address with ID {Id}", id);
            return NoContent();
        }
        
        /// <summary>
        /// Get enriched storage rooms allowed for current user.
        /// </summary>
        [HttpGet("enrichedAddresses")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedAddress>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedAddress>>> GetEnrichedAddresses()
        {
            _logger.LogInformation("Fetching enriched address data");
            var data = await _bll.AddressService.GetEnrichedAddresses();
            var res = data.Select(u => _enrichedAddressApiMapper.Map(u)!).ToList();
            
            _logger.LogInformation("Returned {Count} enriched addresses", res.Count);
            return Ok(res);
        }
    }
}
