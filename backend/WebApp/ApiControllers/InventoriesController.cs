using System.Security.Claims;
using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMapper;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "admin,manager,tootaja")]
public class InventoriesController : ControllerBase
{
    private readonly ILogger<InventoriesController> _logger;
    private readonly IAppBLL _bll;

    private readonly InventoryAPIMapper _mapper = new();
    private readonly EnrichedInventoryApiMapper _enrichedMapper = new();

    public InventoriesController(IAppBLL bll, ILogger<InventoriesController> logger)
    {
        _bll = bll;
        _logger = logger;
    }

    // ----------------------------------------------------------------
    //  HELPER – loe kasutaja rollid JA LOGI need välja
    // ----------------------------------------------------------------
    private List<string> GetCurrentUserRoles()
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        _logger.LogInformation("▶ JWT roles for user {User}: {Roles}",
            User.Identity?.Name ?? "anonymous",
            roles.Count == 0 ? "(none)" : string.Join(", ", roles));

        return roles;
    }

    // ----------------------------------------------------------------
    //  GET /inventories
    // ----------------------------------------------------------------
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Inventory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
    {
        var userRoles = GetCurrentUserRoles();

        var all = (await _bll.InventoryService.AllAsync()).ToList();
        LogInventories("before filter", all);

        var filtered = all
            .Where(i => i.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
            .ToList();

        LogInventories("after  filter", filtered);

        return filtered.Select(i => _mapper.Map(i)!).ToList();
    }

    // ----------------------------------------------------------------
    //  GET /inventories/enrichedInventories
    // ----------------------------------------------------------------
    [HttpGet("enrichedInventories")]
    [ProducesResponseType(typeof(IEnumerable<EnrichedInventory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EnrichedInventory>>> GetEnrichedInventories()
    {
        var userRoles = GetCurrentUserRoles();

        var all = (await _bll.InventoryService.GetEnrichedInventories()).ToList();
        LogInventories("before filter", all);

        var filtered = all
            .Where(i => i!.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
            .ToList();

        LogInventories("after  filter", filtered);

        return Ok(filtered.Select(i => _enrichedMapper.Map(i)!));
    }

    // ----------------------------------------------------------------
    //  Ülejäänud meetodid jäid samaks (lühendatud)
    // ----------------------------------------------------------------
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Inventory>> GetInventory(Guid id)
    {
        var inv = await _bll.InventoryService.FindAsync(id);
        if (inv == null) return NotFound();

        if (inv.AllowedRoles == null ||
            !inv.AllowedRoles.Intersect(GetCurrentUserRoles()).Any())
            return Forbid();     // nähtav vaid lubatud rollidele

        return _mapper.Map(inv)!;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutInventory(Guid id, Inventory dto)
    {
        if (id != dto.Id) return BadRequest();
        await _bll.InventoryService.UpdateAsync(_mapper.Map(dto)!);
        await _bll.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Inventory>> PostInventory(Inventory dto)
    {
        var bllEntity = _mapper.Map(dto);
        _bll.InventoryService.Add(bllEntity);
        await _bll.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInventory),
            new { id = bllEntity.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            dto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInventory(Guid id)
    {
        await _bll.InventoryService.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return NoContent();
    }

    // ----------------------------------------------------------------
    //  Väike util logimiseks
    // ----------------------------------------------------------------
    private void LogInventories(string stage, IEnumerable<dynamic> list)
    {
        var arr = list.ToList();
        _logger.LogInformation("▶ Inventories {Stage}: {Count}", stage, arr.Count);
        foreach (var inv in arr)
        {
        }
    }
}
