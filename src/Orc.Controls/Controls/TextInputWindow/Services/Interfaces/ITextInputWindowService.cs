namespace Orc.Controls.Services;

using System.Threading.Tasks;

public interface ITextInputWindowService
{
    Task<TextInputDialogResult> ShowDialogAsync(string title, string initialText);
}