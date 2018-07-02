// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Models
{
    using Catel.Data;
    using Catel.Logging;

    public class LogFilter : ModelBase
    {
        public string Name { get; set; }

        public ExpressionType ExpressionType { get; set; } = ExpressionType.Contains;

        public string ExpressionValue { get; set; }

        public Action Action { get; set; } = Action.Include;

        public Target Target { get; set; } = Target.TypeName;

        public bool Pass(LogEntry logEntry)
        {
            var result = false;
            var expression = Target == Target.TypeName ? logEntry.Log.TargetType.FullName : logEntry.Log.TargetType.Assembly.GetName().Name;
            if (!string.IsNullOrWhiteSpace(expression))
            {
                switch (ExpressionType)
                {
                    case ExpressionType.Contains:
                        result = expression.Contains(ExpressionValue);
                        break;
                    case ExpressionType.NotContains:
                        result = !expression.Contains(ExpressionValue);
                        break;
                    case ExpressionType.Equals:
                        result = expression.Equals(ExpressionValue);
                        break;
                    case ExpressionType.NotEquals:
                        result = !expression.Equals(ExpressionValue);
                        break;
                    case ExpressionType.StartsWith:
                        result = expression.StartsWith(ExpressionValue);
                        break;
                    case ExpressionType.NotStartsWith:
                        result = !expression.StartsWith(ExpressionValue);
                        break;
                }
            }

            return Action == Action.Include ? result : !result;
        }
    }
}
