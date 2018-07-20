// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Data;
    using Catel.Logging;

    public class LogFilter : ModelBase
    {
        public string Name { get; set; }

        public LogFilterExpressionType ExpressionType { get; set; } = LogFilterExpressionType.Contains;

        public string ExpressionValue { get; set; }

        public LogFilterAction Action { get; set; } = LogFilterAction.Include;

        public LogFilterTarget Target { get; set; } = LogFilterTarget.TypeName;

        public bool Pass(LogEntry logEntry)
        {
            var result = false;

            var expression = Target == LogFilterTarget.TypeName ? logEntry.Log.TargetType.FullName : logEntry.Log.TargetType.Assembly.GetName().Name;
            if (!string.IsNullOrWhiteSpace(expression))
            {
                switch (ExpressionType)
                {
                    case LogFilterExpressionType.Contains:
                        result = expression.Contains(ExpressionValue);
                        break;

                    case LogFilterExpressionType.NotContains:
                        result = !expression.Contains(ExpressionValue);
                        break;

                    case LogFilterExpressionType.Equals:
                        result = expression.Equals(ExpressionValue);
                        break;

                    case LogFilterExpressionType.NotEquals:
                        result = !expression.Equals(ExpressionValue);
                        break;

                    case LogFilterExpressionType.StartsWith:
                        result = expression.StartsWith(ExpressionValue);
                        break;

                    case LogFilterExpressionType.NotStartsWith:
                        result = !expression.StartsWith(ExpressionValue);
                        break;
                }
            }

            return Action == LogFilterAction.Include ? result : !result;
        }
    }
}
