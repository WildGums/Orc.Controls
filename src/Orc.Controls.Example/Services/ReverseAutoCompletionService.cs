namespace Orc.Controls.Example.Services
{
    using System;
    using System.Collections;
    using System.Linq;
    using Catel.Reflection;
    using Catel.Services;

    public sealed class ReverseAutoCompletionService : IAutoCompletionService
    {
        #region IAutoCompletionService Members
        public string[] GetAutoCompleteValues(string property, string filter, IEnumerable source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var reverseFilter = string.Empty;
            for (var i = filter.Length - 1; i >= 0; i--)
            {
                reverseFilter += filter[i];
            }

            return source.Cast<object>().Select(o => PropertyHelper.GetPropertyValue<string>(o, property, true)).Where(s => s.EndsWith(reverseFilter)).ToArray();
        }
        #endregion
    }
}
