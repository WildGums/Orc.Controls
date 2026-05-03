namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orc.FileSystem;

public class FileBasedSettingsFolderProvider : ISettingsFolderProvider
{
    private readonly string _settingsFilePath;
    private readonly ISpecialFoldersPathResolver _specialFoldersPathResolver;
    private SettingsFoldersCollection? _cachedSettings;
    private DateTime _lastFileWrite = DateTime.MinValue;
    private readonly ISettingsSerializer<SettingsFoldersCollection> _serializer;
    private readonly IFileService _fileService;
    private readonly IDirectoryService _directoryService;

    public FileBasedSettingsFolderProvider(string settingsFilePath,
        ISpecialFoldersPathResolver specialFoldersPathResolver,
        ISettingsSerializer<SettingsFoldersCollection> serializer,
        IFileService fileService,
        IDirectoryService directoryService)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settingsFilePath);
        ArgumentNullException.ThrowIfNull(specialFoldersPathResolver);
        ArgumentNullException.ThrowIfNull(serializer);
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(directoryService);

        _settingsFilePath = specialFoldersPathResolver.GetResolvedPath(settingsFilePath);
        _specialFoldersPathResolver = specialFoldersPathResolver;
        _serializer = serializer;
        _fileService = fileService;
        _directoryService = directoryService;
    }

    public async Task<bool> SettingsFileExistsAsync()
    {
        return await Task.FromResult(_fileService.Exists(_settingsFilePath));
    }

    public async Task<SettingsFoldersCollection> GetSettingsFoldersAsync()
    {
        // Check if we need to reload from file
        if (_cachedSettings is null || HasFileChanged())
        {
            await LoadFromFileAsync();
        }

        // Ensure resolved paths are updated
        if (_cachedSettings?.SettingsFolders is not null)
        {
            foreach (var folder in _cachedSettings.SettingsFolders)
            {
                folder.ResolvedPath = folder.Path is null ? string.Empty : _specialFoldersPathResolver.GetResolvedPath(folder.Path);
            }
        }

        return _cachedSettings ?? new SettingsFoldersCollection();
    }

    public async Task SaveSettingsFoldersAsync(SettingsFoldersCollection settingsFolders)
    {
        _cachedSettings = settingsFolders ?? new SettingsFoldersCollection();
        await SaveToFileAsync();
    }

    public async Task<SettingsFolderItem?> GetSettingsFolderAsync(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var collection = await GetSettingsFoldersAsync();
        var folder = collection.SettingsFolders.FirstOrDefault(f =>
            string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));

        if (folder is not null)
        {
            folder.ResolvedPath = folder.Path is null ? string.Empty : _specialFoldersPathResolver.GetResolvedPath(folder.Path);
        }

        return folder;
    }

    public async Task<bool> SettingsFolderExistsAsync(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var folder = await GetSettingsFolderAsync(name);
        return folder is not null;
    }

    public async Task AddOrUpdateSettingsFolderAsync(SettingsFolderItem folder)
    {
        ArgumentNullException.ThrowIfNull(folder);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder.Name);

        var collection = await GetSettingsFoldersAsync();
        collection.SettingsFolders ??= [];

        var existingIndex = collection.SettingsFolders.FindIndex(f =>
            string.Equals(f.Name, folder.Name, StringComparison.OrdinalIgnoreCase));

        if (existingIndex >= 0)
        {
            // Update existing
            collection.SettingsFolders[existingIndex] = folder;
        }
        else
        {
            // Add new
            collection.SettingsFolders.Add(folder);
        }

        await SaveSettingsFoldersAsync(collection);
    }

    public async Task RemoveSettingsFolderAsync(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var collection = await GetSettingsFoldersAsync();
        if (collection.SettingsFolders is null)
        {
            return;
        }

        var folder = collection.SettingsFolders.FirstOrDefault(f =>
            string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));

        if (folder is { CanRemove: true })
        {
            collection.SettingsFolders.Remove(folder);
            await SaveSettingsFoldersAsync(collection);
        }
    }

    public async Task<IEnumerable<string>> GetSettingsFolderNamesAsync()
    {
        var collection = await GetSettingsFoldersAsync();

        var names = new List<string>();
        foreach (var folder in collection.SettingsFolders)
        {
            if (folder.Name is not null)
            {
                names.Add(folder.Name);
            }
        }

        return names;
    }

    public async Task InitializeWithDefaultsAsync(SettingsFoldersCollection defaultSettings, bool overrideExisting = false)
    {
        ArgumentNullException.ThrowIfNull(defaultSettings);

        var fileExists = await SettingsFileExistsAsync();

        if (!fileExists || overrideExisting)
        {
            // File doesn't exist or we want to override - save defaults
            await SaveSettingsFoldersAsync(defaultSettings);
            return;
        }

        // File exists and we don't want to override - merge with existing
        var existingCollection = await GetSettingsFoldersAsync();

        // Add any default folders that don't already exist
        foreach (var defaultFolder in defaultSettings.SettingsFolders ?? [])
        {
            var exists = existingCollection.SettingsFolders?.Any(f =>
                string.Equals(f.Name, defaultFolder.Name, StringComparison.OrdinalIgnoreCase)) ?? false;

            if (exists)
            {
                continue;
            }

            existingCollection.SettingsFolders ??= [];
            existingCollection.SettingsFolders.Add(defaultFolder);
        }

        // Update collection-level properties if they're not set
        if (existingCollection.CanAddFolder == default)
        {
            existingCollection.CanAddFolder = defaultSettings.CanAddFolder;
        }

        await SaveSettingsFoldersAsync(existingCollection);
    }

    private async Task LoadFromFileAsync()
    {
        if (!_fileService.Exists(_settingsFilePath))
        {
            _cachedSettings = new();
            return;
        }

        try
        {
            var json = await _fileService.ReadAllTextAsync(_settingsFilePath);
            _cachedSettings = await _serializer.DeserializeAsync(json) ?? new SettingsFoldersCollection();
            _lastFileWrite = File.GetLastWriteTime(_settingsFilePath);
        }
        catch (Exception)
        {
            // If we can't read the file, start with empty collection
            _cachedSettings = new();
        }
    }

    private async Task SaveToFileAsync()
    {
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_settingsFilePath);
            if (!string.IsNullOrEmpty(directory) && !_directoryService.Exists(directory))
            {
                _directoryService.Create(directory);
            }

            var json = await _serializer.SerializeAsync(_cachedSettings ?? new SettingsFoldersCollection());
            await _fileService.WriteAllTextAsync(_settingsFilePath, json);
            _lastFileWrite = DateTime.Now;
        }
        catch (Exception)
        {
            // Handle file write errors appropriately for your application
            throw;
        }
    }

    private bool HasFileChanged()
    {
        if (!_fileService.Exists(_settingsFilePath))
        {
            return false;
        }

        var lastWrite = File.GetLastWriteTime(_settingsFilePath);
        return lastWrite > _lastFileWrite;
    }
}
