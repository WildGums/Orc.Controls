namespace Orc.Controls.Automation
{
    using System.Windows;

    public class RaiseMouseDownEventCall : IAutomationCommandCall
    {
        #region Constants
        public const string EventCommandNamePrefix = "RaiseEvent:";
        #endregion

        #region IAutomationCommandCall Members
        public bool IsMatch(FrameworkElement owner, AutomationCommand command)
        {
            var commandName = command?.CommandName;
            return commandName?.StartsWith(EventCommandNamePrefix) ?? false;
        }

        public bool TryInvoke(FrameworkElement owner, AutomationCommand command, out AutomationCommandResult result)
        {
            result = null;

            //var eventName = GetEventNameFromCommandName(command.CommandName);

            //if (eventName == "MouseDownEvent")
            //{
            //    var point = (PointData)command.Data.ExtractValue();

            //    var screenPoint = owner.PointToScreen(new Point(point.X, point.Y));

            //    MouseInput.MoveTo(new System.Drawing.Point((int)screenPoint.X, (int)screenPoint.Y));
            //    MouseInput.Click(MouseButton.Left);


            //  //  MouseOperations.SetCursorPosition((int)screenPoint.X, (int)screenPoint.Y);

            //   // MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            // //   MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);

            //    return true;
            //}

            return false;
        }
        #endregion

        #region Methods
        public static string ConvertPropertyToCommandName(string eventName)
        {
            return string.IsNullOrWhiteSpace(eventName) ? null : $"{EventCommandNamePrefix}{eventName}";
        }

        public static string GetEventNameFromCommandName(string commandName)
        {
            return string.IsNullOrWhiteSpace(commandName) ? null : commandName.Replace(EventCommandNamePrefix, string.Empty);
        }
        #endregion
    }
}
