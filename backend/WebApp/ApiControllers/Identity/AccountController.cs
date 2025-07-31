using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain.Identity;
using App.DTO.v1;
using App.DTO.v1.Identity;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// User account controller - login, register, etc.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;
    
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    
    private readonly Random _random = new();
    
    private readonly AppDbContext _context;

    private const string UserPassProblem = "User/Password problem";
    private const int RandomDelayMin = 500;
    private const int RandomDelayMax = 5000;

    private const string SettingsJwtPrefix = "JWTSecurity";
    private const string SettingsJwtKey = SettingsJwtPrefix + ":Key";
    private const string SettingsJwtIssuer = SettingsJwtPrefix + ":Issuer";
    private const string SettingsJwtAudience = SettingsJwtPrefix + ":Audience";
    private const string SettingsJwtExpiresInSeconds = SettingsJwtPrefix + ":ExpiresInSeconds";
    private const string SettingsJwtRefreshTokenExpiresInSeconds = SettingsJwtPrefix + ":RefreshTokenExpiresInSeconds";

    public AccountController(IConfiguration configuration, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, ILogger<AccountController> logger, AppDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Logs in a user and returns a JWT and refresh token.
    /// </summary>
    /// <param name="loginInfo">User login information (email and password).</param>
    /// <param name="jwtExpiresInSeconds">Optional override for JWT expiry time in seconds.</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional override for refresh token expiry time in seconds.</param>
    /// <returns>JWT response with tokens and user information.</returns>
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Login(LoginInfo loginInfo, int? jwtExpiresInSeconds, int? refreshTokenExpiresInSeconds)
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        if (appUser == null)
        {
            _logger.LogWarning("Login failed: email {Email} not found", loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));
            return NotFound(new Message(UserPassProblem));
        }

        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed: incorrect password for {Email}", loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));
            return NotFound(new Message(UserPassProblem));
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var identity = (ClaimsIdentity)claimsPrincipal.Identity!;

        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        if (!_context.Database.ProviderName!.Contains("InMemory"))
        {
            var deletedRows = await _context.RefreshTokens.Where(t => t.UserId == appUser.Id && t.Expiration < DateTime.UtcNow).ExecuteDeleteAsync();
            _logger.LogInformation("Login cleanup: deleted {Count} old refresh tokens for user {UserId}", deletedRows, appUser.Id);
        }

        var refreshToken = new AppRefreshToken
        {
            UserId = appUser.Id,
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJwtRefreshTokenExpiresInSeconds)
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Login successful for {Email}", loginInfo.Email);

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJwtKey)!,
            _configuration.GetValue<string>(SettingsJwtIssuer)!,
            _configuration.GetValue<string>(SettingsJwtAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJwtExpiresInSeconds)
        );

        return Ok(new JwtResponse
        {
            Jwt = jwt, 
            RefreshToken = refreshToken.RefreshToken, 
            Email = appUser.Email,
            Roles = roles, 
            UserId = appUser.Id, 
            FirstName = appUser.FirstName, 
            LastName = appUser.LastName, 
            Username = appUser.UserName
        });
    }

    /// <summary>
    /// Registers a new user and returns a JWT and refresh token.
    /// Only accessible to users with "admin" or "manager" role.
    /// </summary>
    /// <param name="registerModel">User registration data.</param>
    /// <param name="jwtExpiresInSeconds">Optional override for JWT expiry time.</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional override for refresh token expiry time.</param>
    /// <returns>JWT response with tokens and user information.</returns>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<ActionResult<JwtResponse>> Register(Register registerModel, int? jwtExpiresInSeconds, int? refreshTokenExpiresInSeconds)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Register attempt failed: user {Email} already exists", registerModel.Email);
            return BadRequest(new Message("User already registered"));
        }

        _logger.LogInformation("Registering new user: {Email}", registerModel.Email);
        
        var refreshToken = new AppRefreshToken
        {
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJwtRefreshTokenExpiresInSeconds)
        };

        var appUser = new AppUser
        {
            Email = registerModel.Email,
            UserName = registerModel.Email,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            RefreshTokens = new List<AppRefreshToken> { refreshToken }
        };

        var result = await _userManager.CreateAsync(appUser, registerModel.Password);
        if (!result.Succeeded)
        {
            _logger.LogError("User creation failed for {Email}: {Errors}", registerModel.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new Message { Messages = errors });
        }
        
        _logger.LogInformation("User created: {Email}", appUser.Email);
        
        await _userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.GivenName, appUser.FirstName));
        await _userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.Surname, appUser.LastName));
        await _userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.Name, appUser.UserName));
        
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var roles = await _userManager.GetRolesAsync(appUser);
        var identity = (ClaimsIdentity)claimsPrincipal.Identity!;
        
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJwtKey)!,
            _configuration.GetValue<string>(SettingsJwtIssuer)!,
            _configuration.GetValue<string>(SettingsJwtAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJwtExpiresInSeconds)
        );

        return Ok(new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            Email = appUser.Email,
            Roles = roles,
            UserId = appUser.Id,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            Username = appUser.UserName
        });
    }

    /// <summary>
    /// Renews the JWT using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenModel">Refresh token model with current JWT and refresh token.</param>
    /// <param name="jwtExpiresInSeconds">Optional override for JWT expiry.</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional override for refresh token expiry.</param>
    /// <returns>New JWT response if refresh token is valid.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JwtResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Message), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> RenewRefreshToken(RefreshTokenModel refreshTokenModel, int? jwtExpiresInSeconds, int? refreshTokenExpiresInSeconds)
    {
        JwtSecurityToken jwtToken;
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Failed to parse JWT: {Error}", e.Message);
            return BadRequest(new Message($"Cant parse the token, {e.Message}"));
        }

        if (!IdentityExtensions.ValidateJwt(
            refreshTokenModel.Jwt,
            _configuration.GetValue<string>(SettingsJwtKey)!,
            _configuration.GetValue<string>(SettingsJwtIssuer)!,
            _configuration.GetValue<string>(SettingsJwtAudience)!
        ))
        {
            _logger.LogWarning("JWT validation failed for renew attempt.");
            return BadRequest("JWT validation fail");
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            _logger.LogWarning("JWT missing email claim during refresh.");
            return BadRequest(new Message("No email in jwt"));
        }

        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            _logger.LogWarning("Refresh token request failed: user not found for {Email}", userEmail);
            return NotFound($"User with email {userEmail} not found");
        }

        var validRefreshTokens = await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == refreshTokenModel.RefreshToken && x.Expiration > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == refreshTokenModel.RefreshToken && x.PreviousExpiration > DateTime.UtcNow)
            )
            .ToListAsync();

        if (validRefreshTokens.Count == 0)
        {
            _logger.LogWarning("Refresh token invalid or expired for user {UserId}", appUser.Id);
            return BadRequest(new Message("Refresh token invalid or expired"));
        }

        if (validRefreshTokens.Count != 1)
        {
            _logger.LogError("Multiple valid refresh tokens found for user {UserId}", appUser.Id);
            return Problem("More than one valid refresh token found.");
        }
        
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        
        var roles = await _userManager.GetRolesAsync(appUser);
        var identity = (ClaimsIdentity)claimsPrincipal.Identity!;
        
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJwtKey)!,
            _configuration.GetValue<string>(SettingsJwtIssuer)!,
            _configuration.GetValue<string>(SettingsJwtAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJwtExpiresInSeconds)
        );

        var refreshToken = validRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpiration = refreshToken.Expiration;
            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJwtRefreshTokenExpiresInSeconds);
            await _context.SaveChangesAsync();
        }
        
        _logger.LogInformation("JWT successfully renewed for {Email}", appUser.Email);

        var response = new JwtResponse
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken,
            UserId = appUser.Id,
            Email = appUser.Email,
            Roles = roles,
            FirstName = appUser.FirstName, 
            LastName = appUser.LastName, 
            Username = appUser.UserName
        };

        return Ok(response);
    }

    /// <summary>
    /// Logs out the user by deleting the given refresh token.
    /// </summary>
    /// <param name="logout">Logout info containing refresh token to revoke.</param>
    /// <returns>Number of deleted tokens.</returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult> Logout([FromBody] LogoutInfo logout)
    {
        var appUser = await _context.Users.Where(u => u.Id == User.GetUserId()).SingleOrDefaultAsync();
        if (appUser == null)
        {
            _logger.LogWarning("Logout failed: user not found with ID {UserId}", User.GetUserId());
            return NotFound(new Message(UserPassProblem));
        }

        _logger.LogInformation("Logging out user {UserId}", appUser.Id);

        await _context.Entry(appUser)
            .Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x => x.RefreshToken == logout.RefreshToken || x.PreviousRefreshToken == logout.RefreshToken)
            .ToListAsync();

        foreach (var token in appUser.RefreshTokens!)
        {
            _context.RefreshTokens.Remove(token);
        }

        var deleteCount = await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted {Count} refresh tokens for user {UserId}", deleteCount, appUser.Id);
        return Ok(new { TokenDeleteCount = deleteCount });
    }
    
    /// <summary>
    /// Updates user profile fields like first name, last name, username, and email.
    /// </summary>
    /// <param name="updateInfo">User field update model.</param>
    /// <returns>No content on success, or error details.</returns>
    [HttpPut]
    [Route("/api/v{version:apiVersion}/account/updateuserfields")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateUserFields([FromBody] UserFieldUpdate updateInfo)
    {
        if (string.IsNullOrEmpty(updateInfo.Email))
        {
            _logger.LogWarning("UpdateUserFields failed: email was empty");
            return BadRequest("Email is required");
        }

        var user = await _userManager.FindByEmailAsync(updateInfo.Email);
        if (user == null)
        {
            _logger.LogWarning("UpdateUserFields failed: user with email {Email} not found", updateInfo.Email);
            return BadRequest("User not found");
        }
        
        _logger.LogInformation("Updating fields for user {Email}", updateInfo.Email);

        UpdateUserFieldIfChanged(user, updateInfo.FirstName, (u, v) => u.FirstName = v);
        UpdateUserFieldIfChanged(user, updateInfo.LastName, (u, v) => u.LastName = v);
        UpdateUserFieldIfChanged(user, updateInfo.Username, (u, v) => u.UserName = v);
        UpdateUserFieldIfChanged(user, updateInfo.Email, (u, v) => u.Email = v);

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            _logger.LogInformation("User fields updated successfully for {Email}", updateInfo.Email);
            return NoContent();
        }

        _logger.LogWarning("User update failed for {Email}: {Errors}", updateInfo.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
        return BadRequest(result.Errors);
    }

    private void UpdateUserFieldIfChanged<T>(AppUser user, T newValue, Action<AppUser, T> setter)
    {
        if (!EqualityComparer<T>.Default.Equals(newValue, default(T)) && newValue is string stringValue && !string.IsNullOrEmpty(stringValue))
        {
            setter(user, newValue);
        }
    }
    
    /// <summary>
    /// Changes the user's password.
    /// </summary>
    /// <param name="model">Change password DTO with current and new password.</param>
    /// <returns>No content on success, error message on failure.</returns>    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Route("/api/v{version:apiVersion}/account/changepassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || 
            string.IsNullOrWhiteSpace(model.CurrentPassword) || 
            string.IsNullOrWhiteSpace(model.NewPassword))
        {
            _logger.LogWarning("ChangePassword failed: one or more required fields were empty");
            return BadRequest(new { Error = "All fields are required" });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            _logger.LogWarning("ChangePassword failed: user with email {Email} not found", model.Email);
            return NotFound(new { Error = "User not found" });
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            _logger.LogWarning("ChangePassword failed for user {Email}: {Errors}", model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }
        
        _logger.LogInformation("Password changed successfully for user {Email}", model.Email);
        return NoContent();
    }
    
    /// <summary>
    /// Gets all users along with their assigned roles.
    /// Only accessible by admins or managers.
    /// </summary>
    /// <returns>List of users with role information.</returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsersWithRoles()
    {
        _logger.LogInformation("Fetching all users with their roles");
        
        var users = await _context.Users
            .Include(u => u.UserRoles)!
            .ThenInclude(ur => ur.Role)
            .ToListAsync();

        var result = users.Select(user => new UserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Roles = user.UserRoles!.Select(ur => ur.Role!.Name).ToList()
        });

        return Ok(result);
    }
    
    /// <summary>
    /// Removes a specific role from a user.
    /// Only accessible by admins or managers.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="roleId">The ID of the role to remove.</param>
    /// <returns>Confirmation message on success.</returns>
    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null)
        {
            _logger.LogWarning("RemoveRoleFromUser failed: no role assignment found for user {UserId} and role {RoleId}", userId, roleId);
            return NotFound("Role assignment not found");
        }

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Removed role {RoleId} from user {UserId}", roleId, userId);
        return Ok(new { Message = "Role removed from user" });
    }
    
    private DateTime GetExpirationDateTime(int? expiresInSeconds, string settingsKey)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>(settingsKey)
            ? expiresInSeconds
            : _configuration.GetValue<int>(settingsKey);

        return DateTime.UtcNow.AddSeconds(expiresInSeconds ?? 60);
    }
}