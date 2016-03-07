// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultGroup.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel.Data;

    public class ValidationResultGroup : ModelBase
    {
        public ValidationResultGroup(ValidationResultType resultType)
        {
            ResultType = resultType;
        }

        public ValidationResultType ResultType { get; set; }
        public IList<IValidationResult> ValidationResults { get; set; }
    }
}