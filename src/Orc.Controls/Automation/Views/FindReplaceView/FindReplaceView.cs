namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[Control(ControlTypeName = nameof(ControlType.Window))]
public class FindReplaceView : Window
{
    public FindReplaceView(AutomationElement element) 
        : base(element)
    {
    }

    private FindReplaceViewMap Map => Map<FindReplaceViewMap>();

    public void Find(string text)
    {
        var map = Map;

        var findEdit = map.FindEdit;
        findEdit.Text = text;

        map.FindNextButton.Click();
    }

    public void Replace(string text, string replacementText)
    {
        var map = Map;

        var findEdit = map.FindReplaceEdit;
        findEdit.Text = text;

        var replaceEdit = map.ReplaceEdit;
        replaceEdit.Text = replacementText;

        map.ReplaceButton.Click();
    }

    public void ReplaceAll(string text, string replacementText)
    {
        var map = Map;

        var findEdit = map.FindReplaceEdit;
        findEdit.Text = text;

        var replaceEdit = map.ReplaceEdit;
        replaceEdit.Text = replacementText;

        map.ReplaceAllButton.Click();
    }

    public bool IsCaseSensitive
    {
        get => Map.CaseSensitiveCheckBox.IsChecked == true;
        set => Map.CaseSensitiveCheckBox.IsChecked = value;
    }

    public bool IsWholeWord
    {
        get => Map.WholeWordCheckBox.IsChecked == true;
        set => Map.WholeWordCheckBox.IsChecked = value;
    }

    public bool IsRegex
    {
        get => Map.RegexCheckBox.IsChecked == true;
        set => Map.RegexCheckBox.IsChecked = value;
    }

    public bool IsWildcards
    {
        get => Map.WildcardsCheckBox.IsChecked == true;
        set => Map.WildcardsCheckBox.IsChecked = value;
    }

    public bool SearchUp
    {
        get => Map.SearchUpCheckBox.IsChecked == true;
        set => Map.SearchUpCheckBox.IsChecked = value;
    }
}
