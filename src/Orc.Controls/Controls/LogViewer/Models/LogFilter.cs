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
        var result = false;

        string? expression;

        switch (Target)
        {
            case LogFilterTarget.AssemblyName:
            case LogFilterTarget.TypeName:
                expression = logEntry.Category;
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

        var expressionValue = ExpressionValue ?? string.Empty;
        switch (ExpressionType)
        {
            case LogFilterExpressionType.Contains:
                result = expression.ContainsIgnoreCase(expressionValue);
                break;

            case LogFilterExpressionType.NotContains:
                result = !expression.ContainsIgnoreCase(expressionValue);
                break;

            case LogFilterExpressionType.Equals:
                result = expression.EqualsIgnoreCase(expressionValue);
                break;

            case LogFilterExpressionType.NotEquals:
                result = !expression.EqualsIgnoreCase(expressionValue);
                break;

            case LogFilterExpressionType.StartsWith:
                result = expression.StartsWithIgnoreCase(expressionValue);
                break;

            case LogFilterExpressionType.NotStartsWith:
                result = !expression.StartsWithIgnoreCase(expressionValue);
                break;
        }

        return Action == LogFilterAction.Include ? result : !result;
    }
}
