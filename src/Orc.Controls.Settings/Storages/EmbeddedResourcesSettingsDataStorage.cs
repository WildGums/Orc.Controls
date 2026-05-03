namespace Orc.Controls.Settings;

using System;
using System.Threading.Tasks;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class EmbeddedResourcesSettingsDataStorage : ISettingsDataStorage
{
    private readonly ILogger<EmbeddedResourcesSettingsDataStorage> _logger;
    private readonly IEmbeddedResourceProvider _embeddedResourceProvider;
    private readonly ISettingsLocationProvider _locationProvider;

    public EmbeddedResourcesSettingsDataStorage(
        ILogger<EmbeddedResourcesSettingsDataStorage> logger,
        IEmbeddedResourceProvider embeddedResourceProvider,
        ISettingsLocationProvider locationProvider)
    {
        _logger = logger;
        _embeddedResourceProvider = embeddedResourceProvider ?? throw new ArgumentNullException(nameof(embeddedResourceProvider));
        _locationProvider = locationProvider ?? throw new ArgumentNullException(nameof(locationProvider));
    }

    public async Task<bool> IsReadOnlyAsync(string settingsKey)
    {
        return true;
    }

    /// <summary>
    /// Determines if this storage can handle the given settings key
    /// </summary>
    public async Task<bool> CanHandleAsync(string settingsKey)
    {
        try
        {
            var location = await _locationProvider.GetLocationAsync(settingsKey);

            // Handle locations that look like resource paths (contain dots but no directory separators)
            return !string.IsNullOrEmpty(location) &&
                   location.Contains('.') &&
                   !location.Contains('\\') &&
                   !location.Contains('/') &&
                   !location.Contains(':'); // No drive letters
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> LoadStringAsync(string settingsKey)
    {
        if (!await CanHandleAsync(settingsKey))
        {
            return null;
        }

        try
        {
            var location = await _locationProvider.GetLocationAsync(settingsKey);
            var content = await _embeddedResourceProvider.GetResourceContentAsync(location);

            if (!string.IsNullOrEmpty(content))
            {
                _logger.LogDebug($"Loaded settings from embedded resource for key: {settingsKey} (location: {location})");
            }

            return content;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Error loading embedded resource for key: {settingsKey}");
            return null;
        }
    }

    public async Task<bool> ExistsAsync(string settingsKey)
    {
        if (!await CanHandleAsync(settingsKey))
        {
            return false;
        }

        try
        {
            var location = await _locationProvider.GetLocationAsync(settingsKey);
            return await _embeddedResourceProvider.ResourceExistsAsync(location);
        }
        catch
        {
            return false;
        }
    }

    // Write operations are not supported for embedded resources
    public Task SaveStringAsync(string settingsKey, string data)
    {
        throw _logger.LogErrorAndCreateException<NotSupportedException>("Embedded resources are read-only. Use a different storage provider for write operations.");
    }

    public Task DeleteAsync(string settingsKey)
    {
        throw _logger.LogErrorAndCreateException<NotSupportedException>("Embedded resources are read-only. Use a different storage provider for write operations.");
    }

    public Task RenameAsync(string oldKey, string newKey)
    {
        throw _logger.LogErrorAndCreateException<NotSupportedException>("Embedded resources are read-only. Use a different storage provider for write operations.");
    }
}
