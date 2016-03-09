// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeNodeBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Collections;
    using Catel.Data;

    public abstract class ValidationContextTreeNodeBase : ModelBase
    {
        protected ValidationContextTreeNodeBase()
        {
            Children = new FastObservableCollection<ValidationContextTreeNodeBase>();

            IsExpanded = true;
        }        

        public FastObservableCollection<ValidationContextTreeNodeBase> Children { get; }

        public string DisplayName { get; protected set; }

        public bool IsExpanded { get; set; }

        public ValidationResultType? ResultType { get; set; }
    }
}