namespace Orc.Automation
{
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        public static void MouseClick(this AutomationElement element, MouseButton mouseButton = MouseButton.Left)
        {
            Argument.IsNotNull(() => element);

            var rect = element.Current.BoundingRectangle;

            MouseInput.MoveTo(rect.GetClickablePoint());
            MouseInput.Click(mouseButton);
        }
    }
}
