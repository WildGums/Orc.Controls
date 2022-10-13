namespace Orc.Controls
{
    using System.Windows;

    internal static class BooleanExtensions
    {
        public static Visibility ToVisibility(this bool value, Visibility hiddenState = Visibility.Collapsed)
        {
            return value ? Visibility.Visible : hiddenState;
        }
    }
}
