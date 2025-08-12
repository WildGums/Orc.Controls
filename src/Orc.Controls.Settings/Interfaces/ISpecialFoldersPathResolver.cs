namespace Orc.Controls.Settings;

public interface ISpecialFoldersPathResolver
{
    string ResolveSpecialFolder(string name);
    string GetResolvedPath(string path);
}
