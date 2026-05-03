namespace Orc.Controls.Settings;

using System.Threading.Tasks;

/// <summary>
/// Interface for providing embedded resource access
/// </summary>
public interface IEmbeddedResourceProvider
{
    /// <summary>
    /// Gets embedded resource content as string
    /// </summary>
    Task<string?> GetResourceContentAsync(string resourcePath);

    /// <summary>
    /// Checks if an embedded resource exists
    /// </summary>
    Task<bool> ResourceExistsAsync(string resourcePath);
}
