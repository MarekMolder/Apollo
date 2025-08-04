using App.DAL.EF;
using App.Domain.Identity;
using App.DTO.v1.Identity;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// Controller for managing roles.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(AppDbContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Gets all roles in the system.
        /// </summary>
        /// <returns>List of all roles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            _logger.LogInformation("Fetching all roles.");
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <returns>Status message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                _logger.LogWarning("CreateRole failed: role name is missing");
                return BadRequest("Role name is required");
            }

            if (await _context.Roles.AnyAsync(r => r.Name == roleName))
            {
                _logger.LogWarning("CreateRole failed: role '{RoleName}' already exists", roleName);
                return BadRequest("Role already exists");
            }

            var role = new AppRole
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Role '{RoleName}' created successfully", roleName);
            return Ok(new { Message = $"Role '{roleName}' created successfully" });
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="dto">AssignRoleDto containing user and role IDs.</param>
        /// <returns>Status message.</returns>
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                _logger.LogWarning("AssignRole failed: user with ID {UserId} not found", dto.UserId);
                return NotFound("User not found");
            }

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
            {
                _logger.LogWarning("AssignRole failed: role with ID {RoleId} not found", dto.RoleId);
                return NotFound("Role not found");
            }

            var alreadyAssigned =
                await _context.UserRoles.AnyAsync(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
            if (alreadyAssigned)
            {
                _logger.LogWarning("AssignRole failed: user {UserId} already has role {RoleId}", dto.UserId, dto.RoleId);
                return BadRequest("User already has this role");
            }

            var userRole = new AppUserRole
            {
                UserId = dto.UserId,
                RoleId = dto.RoleId
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Role {RoleId} assigned to user {UserId}", dto.RoleId, dto.UserId);
            return Ok(new { Message = "Role assigned to user" });
        }
    }
}