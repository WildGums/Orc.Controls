namespace Orc.Controls.Settings;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Orc.FileSystem;

/// <summary>
/// File system settings storage using location provider and serializer
/// </summary>
public class FileSystemSettingsDataStorage : ISettingsDataStorage
{
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly ISettingsLocationProvider _locationProvider;

    public FileSystemSettingsDataStorage(
        IFileService fileService,
        IDirectoryService directoryService,
        ISettingsLocationProvider locationProvider)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _directoryService = directoryService ?? throw new ArgumentNullException(nameof(directoryService));
        _locationProvider = locationProvider ?? throw new ArgumentNullException(nameof(locationProvider));
    }

    public async Task<bool> IsReadOnlyAsync(string settingsKey)
    {
        return false;
    }

    /// <summary>
    /// Async version of CanHandle for better performance when possible
    /// </summary>
    public async Task<bool> CanHandleAsync(string settingsKey)
    {
        try
        {
            var location = await _locationProvider.GetLocationAsync(settingsKey);
            if (location == "Invalid")
            {
                return false;
            }

            // Handle locations that look like file paths (contain directory separators or drive letters)
            return !string.IsNullOrEmpty(location) &&
                   (location.Contains('\\') ||
                    location.Contains('/') ||
                    location.Contains(':') || // Drive letters like C:
                    Path.IsPathRooted(location));
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

            if (!_fileService.Exists(location))
            {
                return null;
            }

            return await _fileService.ReadAllTextAsync(location);
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveStringAsync(string settingsKey, string data)
    {
        if (!await CanHandleAsync(settingsKey))
        {
            throw new InvalidOperationException($"This storage cannot handle settings key: {settingsKey}");
        }

        var location = await _locationProvider.GetLocationAsync(settingsKey);
        var directory = Path.GetDirectoryName(location);

        if (directory is not null && !_directoryService.Exists(directory))
        {
            _directoryService.Create(directory);
        }

        await _fileService.WriteAllTextAsync(location, data);
    }

    public async Task DeleteAsync(string settingsKey)
    {
        if (!await CanHandleAsync(settingsKey))
        {
            return; // Silently ignore if we can't handle it
        }

        var location = await _locationProvider.GetLocationAsync(settingsKey);

        if (_fileService.Exists(location))
        {
            _fileService.Delete(location);
        }
    }

    public async Task RenameAsync(string oldKey, string newKey)
    {
        var canHandleOld = await CanHandleAsync(oldKey);
        var canHandleNew = await CanHandleAsync(newKey);

        if (!canHandleOld || !canHandleNew)
        {
            throw new InvalidOperationException(
                $"This storage cannot handle one or both settings keys: {oldKey}, {newKey}");
        }

        var oldLocation = await _locationProvider.GetLocationAsync(oldKey);
        var newLocation = await _locationProvider.GetLocationAsync(newKey);

        if (_fileService.Exists(oldLocation))
        {
            var newDirectory = Path.GetDirectoryName(newLocation);
            if (newDirectory is not null && !_directoryService.Exists(newDirectory))
            {
                _directoryService.Create(newDirectory);
            }

            _fileService.Move(oldLocation, newLocation);
        }
    }

    public async Task<bool> ExistsAsync(string settingsKey)
    {
        if (!await CanHandleAsync(settingsKey))
        {
            return false;
        }

        var location = await _locationProvider.GetLocationAsync(settingsKey);
        return _fileService.Exists(location);
    }
}
