﻿namespace Orc.Controls;

using System.Collections.Generic;
using Catel.Data;

public interface IValidationContextTreeNode
{
    string? DisplayName { get; }
    bool IsExpanded { get; set; }
    bool IsVisible { get; set; }

    ValidationResultType? ResultType { get; }
    IEnumerable<IValidationContextTreeNode> Children { get; }
}
