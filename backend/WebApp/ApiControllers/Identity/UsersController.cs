using App.Domain.Identity;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Controller for managing application users.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "admin,juhataja")]
public class UsersController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UsersController> _logger;
    
    public UsersController(UserManager<AppUser> userManager, ILogger<UsersController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Gets all users with basic profile information.
    /// </summary>
    /// <returns>List of all users.</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Fetching all users");
        
        var users = await _userManager.Users
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.UserName,
                u.Email
            })
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} users", users.Count);
        return Ok(users);
    }
    
    /// <summary>
    /// Gets a single user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>User info or NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        _logger.LogInformation("Fetching user with ID {UserId}", id);

        var user = await _userManager.Users
            .Where(u => u.Id == id)
            .Select(u => new {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.UserName
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", id);
            return NotFound();
        }

        _logger.LogInformation("User with ID {UserId} retrieved successfully", id);
        return Ok(user);
    }
}