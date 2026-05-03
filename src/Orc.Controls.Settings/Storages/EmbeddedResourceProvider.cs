namespace Orc.Controls.Settings;

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// Default implementation of embedded resource provider
/// </summary>
public class EmbeddedResourceProvider : IEmbeddedResourceProvider
{
    private readonly Assembly _assembly;

    public EmbeddedResourceProvider(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        _assembly = assembly;
    }

    public async Task<string?> GetResourceContentAsync(string resourcePath)
    {
        try
        {
            await using var stream = _assembly.GetManifestResourceStream(resourcePath);
            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ResourceExistsAsync(string resourcePath)
    {
        var resourceNames = _assembly.GetManifestResourceNames();

        return await Task.FromResult(Array.Exists(resourceNames, name => name.Equals(resourcePath, StringComparison.OrdinalIgnoreCase)));
    }
}
