namespace Orc.Controls.Settings;

using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// File system location provider that converts keys to file paths
/// </summary>
public class FileSystemLocationProvider : ISettingsLocationProvider
{
    private readonly string _basePath;

    public FileSystemLocationProvider(string basePath)
    {
        _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
    }

    public async Task<string> GetLocationAsync(string settingsKey)
    {
        if (string.IsNullOrEmpty(settingsKey))
        {
            return Path.Combine(_basePath, "Unknown.json");
        }

        // Convert "Key1/Key2/Key3" to "BasePath/Key1/Key2/Key3.json"
        var keyParts = settingsKey.Split('/', StringSplitOptions.RemoveEmptyEntries);

        // Sanitize each part
        var sanitizedParts = new string[keyParts.Length];
        for (var i = 0; i < keyParts.Length; i++)
        {
            sanitizedParts[i] = SanitizeFileName(keyParts[i]);
        }

        // Last part becomes the filename with .json extension
        if (sanitizedParts.Length > 0)
        {
            sanitizedParts[^1] += ".json";
        }

        return Path.Combine(_basePath, Path.Combine(sanitizedParts));
    }

    private static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return "Unknown";
        }

        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = fileName;

        foreach (var invalidChar in invalidChars)
        {
            sanitized = sanitized.Replace(invalidChar, '_');
        }

        return sanitized.Replace(':', '_')
            .Replace('*', '_')
            .Replace('?', '_')
            .Replace('"', '_')
            .Replace('<', '_')
            .Replace('>', '_')
            .Replace('|', '_');
    }
}
