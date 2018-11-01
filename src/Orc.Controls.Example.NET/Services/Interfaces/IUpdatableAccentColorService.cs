namespace Orc.Controls.Example.Services
{
    using System.Windows.Media;
    using Orc.Controls.Services;

    public interface IUpdatableAccentColorService : IAccentColorService
    {
        void SetAccentColor(Color color);
    }
}
