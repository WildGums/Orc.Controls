// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationRule.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;

    public class ValidationRule : ModelBase
    {
        
        public ValidationRule(string name)
        {
            DisplayName = name;
            ResultGroups = new List<ValidationResultGroup>();
        }

        public string DisplayName { get; private set; }

        public IList<ValidationResultGroup> ResultGroups { get; private set; }

        public string Summary { get; private set; }

        public void SetSummary()
        {
            var errorCount = ResultGroups.Where(x => x.ResultType == ValidationResultType.Error).SelectMany(x => x.ValidationResults).Count();
            var warningCount = ResultGroups.Where(x => x.ResultType == ValidationResultType.Warning).SelectMany(x => x.ValidationResults).Count();
            Summary = $"{DisplayName} (Errors: {errorCount}, Warnings: {warningCount})";
        }
    }
}