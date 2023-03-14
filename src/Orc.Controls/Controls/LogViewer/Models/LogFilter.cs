namespace Orc.Controls;

using System;
using Catel;
using Catel.Data;
using Catel.Logging;

public class LogFilter : ModelBase
{
    public string? Name { get; set; }

    public LogFilterExpressionType ExpressionType { get; set; } = LogFilterExpressionType.Contains;

    public string? ExpressionValue { get; set; }

    public LogFilterAction Action { get; set; } = LogFilterAction.Include;

    public LogFilterTarget Target { get; set; } = LogFilterTarget.TypeName;

    public bool Pass(LogEntry logEntry)
    {
        if (logEntry is null)
        {
            return false;
        }

        var result = false;

        var expression = string.Empty;

        switch (Target)
        {
            case LogFilterTarget.AssemblyName:
                expression = logEntry.Log.TargetType.Assembly.GetName().Name;
                break;

            case LogFilterTarget.TypeName:
                expression = logEntry.Log.TargetType.FullName;
                break;

            case LogFilterTarget.LogMessage:
                expression = logEntry.Message;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Target), $"Target '{Target}' is not yet supported");
        }

        if (string.IsNullOrWhiteSpace(expression))
        {
            return Action != LogFilterAction.Include;
        }

        switch (ExpressionType)
        {
            case LogFilterExpressionType.Contains:
                result = expression.ContainsIgnoreCase(ExpressionValue);
                break;

            case LogFilterExpressionType.NotContains:
                result = !expression.ContainsIgnoreCase(ExpressionValue);
                break;

            case LogFilterExpressionType.Equals:
                result = expression.EqualsIgnoreCase(ExpressionValue);
                break;

            case LogFilterExpressionType.NotEquals:
                result = !expression.EqualsIgnoreCase(ExpressionValue);
                break;

            case LogFilterExpressionType.StartsWith:
                result = expression.StartsWithIgnoreCase(ExpressionValue);
                break;

            case LogFilterExpressionType.NotStartsWith:
                result = !expression.StartsWithIgnoreCase(ExpressionValue);
                break;
        }

        return Action == LogFilterAction.Include ? result : !result;
    }
}
