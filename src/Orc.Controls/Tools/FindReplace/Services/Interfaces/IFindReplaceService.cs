namespace Orc.Controls.Services
{
    public interface IFindReplaceService
    {
        string GetInitialFindText();
        bool FindNext(string textToFind, FindReplaceSettings settings);
        bool Replace(string textToFind, string textToReplace, FindReplaceSettings settings);
        void ReplaceAll(string textToFind, string textToReplace, FindReplaceSettings settings);
    }
}
