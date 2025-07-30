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
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly Random _random = new();
    private readonly AppDbContext _context;

    private const string UserPassProblem = "User/Password problem";
    private const int RandomDelayMin = 500;
    private const int RandomDelayMax = 5000;

    private const string SettingsJWTPrefix = "JWTSecurity";
    private const string SettingsJWTKey = SettingsJWTPrefix + ":Key";
    private const string SettingsJWTIssuer = SettingsJWTPrefix + ":Issuer";
    private const string SettingsJWTAudience = SettingsJWTPrefix + ":Audience";
    private const string SettingsJWTExpiresInSeconds = SettingsJWTPrefix + ":ExpiresInSeconds";
    private const string SettingsJWTRefreshTokenExpiresInSeconds = SettingsJWTPrefix + ":RefreshTokenExpiresInSeconds";

    public AccountController(IConfiguration configuration, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, ILogger<AccountController> logger, AppDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponse>> Login(LoginInfo loginInfo, int? jwtExpiresInSeconds, int? refreshTokenExpiresInSeconds)
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));
            return NotFound(new Message(UserPassProblem));
        }

        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password {} for email {} was wrong", loginInfo.Password, loginInfo.Email);
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
        
        //var claims = await BuildClaims(appUser);

        if (!_context.Database.ProviderName!.Contains("InMemory"))
        {
            var deletedRows = await _context.RefreshTokens.Where(t => t.UserId == appUser.Id && t.Expiration < DateTime.UtcNow).ExecuteDeleteAsync();
            _logger.LogInformation("Deleted {} refresh tokens", deletedRows);
        }

        var refreshToken = new AppRefreshToken
        {
            UserId = appUser.Id,
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        return Ok(new JwtResponse { Jwt = jwt, RefreshToken = refreshToken.RefreshToken, Email = appUser.Email, Roles = roles, UserId = appUser.Id, FirstName = appUser.FirstName, LastName = appUser.LastName, Username = appUser.UserName });
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<ActionResult<JwtResponse>> Register(Register registerModel, int? jwtExpiresInSeconds, int? refreshTokenExpiresInSeconds)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User {User} already registered", registerModel.Email);
            return BadRequest(new Message("User already registered"));
        }

        var refreshToken = new AppRefreshToken
        {
            Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
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
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new Message { Messages = errors });
        }
        
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
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        return Ok(new JwtResponse { Jwt = jwt, RefreshToken = refreshToken.RefreshToken, Email = appUser.Email, Roles = roles, UserId = appUser.Id, FirstName = appUser.FirstName, LastName = appUser.LastName, Username = appUser.UserName });
    }

    /// <summary>
    /// Renew JWT using refresh token
    /// </summary>
    /// <param name="refreshTokenModel">Data for renewal</param>
    /// <param name="jwtExpiresInSeconds">Optional custom expiration for jwt</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional custom expiration for refresh token</param>
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
            return BadRequest(new Message($"Cant parse the token, {e.Message}"));
        }

        if (!IdentityExtensions.ValidateJwt(
            refreshTokenModel.Jwt,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!
        ))
        {
            return BadRequest("JWT validation fail");
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null) return BadRequest(new Message("No email in jwt"));

        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null) return NotFound($"User with email {userEmail} not found");

        var validRefreshTokens = await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == refreshTokenModel.RefreshToken && x.Expiration > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == refreshTokenModel.RefreshToken && x.PreviousExpiration > DateTime.UtcNow)
            )
            .ToListAsync();

        if (validRefreshTokens == null || validRefreshTokens.Count == 0)
        {
            _logger.LogWarning("RenewRefreshToken: RefreshTokens collection is empty, no valid refresh tokens found");
            return BadRequest(new Message("Refresh token invalid or expired"));
        }

        if (validRefreshTokens.Count != 1)
        {
            _logger.LogWarning("RenewRefreshToken: More than one valid refresh token found");
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
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        var refreshToken = validRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpiration = refreshToken.Expiration;
            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.Expiration = GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds);
            await _context.SaveChangesAsync();
        }

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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<ActionResult> Logout([FromBody] LogoutInfo logout)
    {
        var appUser = await _context.Users.Where(u => u.Id == User.GetUserId()).SingleOrDefaultAsync();
        if (appUser == null) return NotFound(new Message(UserPassProblem));

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
        return Ok(new { TokenDeleteCount = deleteCount });
    }
    
        /// <summary>
        /// Updates user fields.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="updateInfo">Updated user information.</param>
        [HttpPut]
        [Route("/api/v{version:apiVersion}/account/updateuserfields")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUserFields([FromBody] UserFieldUpdate updateInfo)
        {
            if (string.IsNullOrEmpty(updateInfo.Email))
                return BadRequest("Email is required");

            var user = await _userManager.FindByEmailAsync(updateInfo.Email);
            if (user == null)
                return BadRequest("User not found");

            UpdateUserFieldIfChanged(user, updateInfo.FirstName, (u, v) => u.FirstName = v);
            UpdateUserFieldIfChanged(user, updateInfo.LastName, (u, v) => u.LastName = v);
            UpdateUserFieldIfChanged(user, updateInfo.Username, (u, v) => u.UserName = v);
            UpdateUserFieldIfChanged(user, updateInfo.Email, (u, v) => u.Email = v);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return NoContent();

            return BadRequest(result.Errors);
        }

        private void UpdateUserFieldIfChanged<T>(AppUser user, T newValue, Action<AppUser, T> setter)
        {
            if (!EqualityComparer<T>.Default.Equals(newValue, default(T)) && newValue is string stringValue && !string.IsNullOrEmpty(stringValue))
            {
                setter(user, newValue);
            }
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("/api/v{version:apiVersion}/account/changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || 
                string.IsNullOrWhiteSpace(model.CurrentPassword) || 
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return BadRequest(new { Error = "All fields are required" });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new { Error = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }

            return NoContent(); // 204 kui edukas
        }

    private DateTime GetExpirationDateTime(int? expiresInSeconds, string settingsKey)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;
        expiresInSeconds = expiresInSeconds < _configuration.GetValue<int>(settingsKey)
            ? expiresInSeconds
            : _configuration.GetValue<int>(settingsKey);

        return DateTime.UtcNow.AddSeconds(expiresInSeconds ?? 60);
    }
    
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsersWithRoles()
    {
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
    
    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null)
        {
            return NotFound("Role assignment not found");
        }

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Role removed from user" });
    }
}