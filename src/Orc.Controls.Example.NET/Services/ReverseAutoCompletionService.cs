// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls.Example.Services
{
    using System.Collections;
    using System.Linq;
    using Catel;
    using Catel.Reflection;
    using Catel.Services;

    public sealed class ReverseAutoCompletionService : IAutoCompletionService
    {
        public string[] GetAutoCompleteValues(string property, string filter, IEnumerable source)
        {
            Argument.IsNotNull(() => source);

            var reverseFilter = string.Empty;
            for (var i = filter.Length - 1; i >= 0; i--)
            {
                reverseFilter += filter[i];
            }

            return source.Cast<object>().Select(o => PropertyHelper.GetPropertyValue<string>(o, property, true)).Where(s => s.EndsWith(reverseFilter)).ToArray();
        }
    }

}