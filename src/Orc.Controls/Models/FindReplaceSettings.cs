namespace Orc.Controls;

using Catel.Data;

public class FindReplaceSettings : ModelBase
{
    public bool CaseSensitive { get; set; } = true;
    public bool WholeWord { get; set; } = true;
    public bool UseRegex { get; set; } = false;
    public bool UseWildcards { get; set; } = false;
    public bool IsSearchUp { get; set; } = false;
}
