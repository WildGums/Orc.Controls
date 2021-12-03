namespace Orc.Automation
{
    using Controls;

    public static class FrameworkElementExtensions
    {
        public static bool IsVisible(this FrameworkElement element)
        {
            //Automation can't find element if it's not visible, so no checks for null
            if (element is null)
            {
                return false;
            }

            return element.Element.IsVisible();
        }
    }
}
