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
    [Authorize(Roles = "admin,manager,tootaja")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly ProductAPIMapper _mapper = new();
        
        private readonly EnrichedProductApiMapper _enrichedProductApiMapper = new();

        public ProductsController(IAppBLL bll, ILogger<ProductsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<Product> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Product>>> GetActions()
        {
            return (await _bll.ProductService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetActionEntity(Guid id)
        {
            var product = await _bll.ProductService.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map(product)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _bll.ProductService.UpdateAsync(_mapper.Map(product)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Product>> PostActionEntity(Product product)
        {
            var bllEntity = _mapper.Map(product);
            _bll.ProductService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, product);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.ProductService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpGet("enrichedProducts/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedProduct>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedProduct>>> GetEnrichedProduct()
        {
            var data = await _bll.ProductService.GetEnrichedProducts();

            var res = data.Select(u => _enrichedProductApiMapper.Map(u)!).ToList();
            return Ok(res);
        }
        
    }
}
