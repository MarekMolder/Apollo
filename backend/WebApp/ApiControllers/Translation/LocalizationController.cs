using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Asp.Versioning;
using Base.Resources;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.Translation;

/// <summary>
/// Controller for providing localized translation resources.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class LocalizationController : ControllerBase
{
    private readonly ILogger<LocalizationController> _logger;
    
    public LocalizationController(ILogger<LocalizationController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retrieves translation key-value pairs for the given language and resource name.
    /// </summary>
    /// <param name="language">Language code (e.g. en, et, ru).</param>
    /// <param name="resource">Name of the resource file.</param>
    /// <returns>Dictionary of translations.</returns>
    [HttpGet]
    public IActionResult Get(string language, string resource)
    {
        try
        {
            _logger.LogInformation("Requesting translations: resource={Resource}, language={Language}", resource, language);

            var culture = new CultureInfo(language);
            var resolved = ResolveResourceInfo(resource);
            
            if (resolved == null)
            {
                _logger.LogWarning("Could not resolve resource '{Resource}'", resource);
                return NotFound(new { error = "Resource not found." });
            }

            var (resourceName, assembly) = resolved.Value;
            _logger.LogInformation("Resolved resource: {ResourceName} in assembly: {Assembly}", resourceName, assembly.GetName().Name);
            
            var manager = new ResourceManager(resourceName, assembly);
            var resourceSet = manager.GetResourceSet(culture, true, true);

            if (resourceSet == null)
            {
                _logger.LogWarning("No ResourceSet found for resource='{Resource}' and culture='{Culture}'", resourceName, culture.Name);
                return NotFound(new { error = "Resource not found." });
            }

            var translations = new Dictionary<string, string>();
            foreach (DictionaryEntry entry in resourceSet)
            {
                translations[entry.Key.ToString()!] = entry.Value?.ToString() ?? "";
            }

            _logger.LogInformation("Loaded {Count} translations from {ResourceName}", translations.Count, resourceName);
            return Ok(translations);
        }
        catch (CultureNotFoundException)
        {
            _logger.LogWarning("Invalid culture code received: {Language}", language);
            return BadRequest(new { error = "Invalid language code." });
        }
        catch (MissingManifestResourceException ex)
        {
            _logger.LogError(ex, "MissingManifestResourceException for resource: {Resource}", resource);
            return NotFound(new { error = "Resource not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while retrieving translations");
            return StatusCode(500, new { error = "Internal error" });
        }
    }

    /// <summary>
    /// Resolves the full resource name and corresponding assembly for a given resource key.
    /// </summary>
    /// <param name="resource">Resource identifier.</param>
    /// <returns>Tuple of resource name and assembly, or null if not found.</returns>
    private (string ResourceName, Assembly Assembly)? ResolveResourceInfo(string resource)
    {
        if (string.IsNullOrWhiteSpace(resource)) return null;

        if (resource.Equals("common", StringComparison.OrdinalIgnoreCase) ||
            resource.StartsWith("Base", StringComparison.OrdinalIgnoreCase))
        {
            return ("Base.Resources.common", typeof(ResourceAnchor).Assembly);
        }

        return ($"App.Resources.Domain.{resource}", typeof(App.Resources.ResourceAnchor).Assembly);
    }
}