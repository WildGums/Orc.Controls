namespace Orc.Controls
{
    using System.Linq;
    using System.Text;
    using Catel.Data;
    using Catel.Text;

    public static class ValidationContextExtensions
    {
        public static string GetViewContents(this ValidationContext context)
        {
            if (context is null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Errors");
            stringBuilder.AppendLine("===============================");

            var errors = context.GetErrors();
            if (errors.Count == 0)
            {
                stringBuilder.AppendLine("[no errors]");
            }
            else
            {
                foreach (var error in errors.OrderBy(x => x.Message))
                {
                    stringBuilder.AppendLine("- {0}", error.Message);
                }
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("Warnings");
            stringBuilder.AppendLine("===============================");

            var warnings = context.GetWarnings();
            if (warnings.Count == 0)
            {
                stringBuilder.AppendLine("[no warnings]");
            }
            else
            {
                foreach (var warning in warnings.OrderBy(x => x.Message))
                {
                    stringBuilder.AppendLine("- {0}", warning.Message);
                }
            }

            var finalString = stringBuilder.ToString();
            return finalString;
        }
    }
}
