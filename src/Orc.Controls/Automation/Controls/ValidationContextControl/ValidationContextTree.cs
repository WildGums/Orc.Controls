namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Catel.Data;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.ValidationContextTree))]
    public class ValidationContextTree : FrameworkElement<ValidationContextTreeModel, ValidationContextTreeMap>
    {
        public ValidationContextTree(AutomationElement element)
            : base(element)
        {
        }

        public IReadOnlyList<string> GetValidationItems(string tag, ValidationResultType type)
        {
            var innerTree = Map.InnerTree;

            var firstItem = innerTree.ChildItems.FirstOrDefault(x => x.Header?.StartsWith(tag) ?? false);
            if (firstItem is null)
            {
                return Array.Empty<string>();
            }

            var typeItem = firstItem.ChildItems.FirstOrDefault(x => x.Header?.StartsWith(type.ToString()) ?? false);
            if (typeItem is null)
            {
                return Array.Empty<string>();
            }

            return typeItem.ChildItems.Select(x => x.Header).ToList();
        }

        public string GetContents()
        {
            var estimatedValidationContext = new ValidationContext();

            var innerTree = Map.InnerTree;

            foreach (var tagItem in innerTree.ChildItems)
            {
                var tag = tagItem.Header;
                foreach (var typeItem in tagItem.ChildItems)
                {
                    var validationResultTypeString = typeItem.Header;
                    var validationResult = validationResultTypeString.StartsWith(ValidationResultType.Error.ToString()) 
                        ? ValidationResultType.Error
                        : ValidationResultType.Warning;

                    foreach (var resultItem in typeItem.ChildItems)
                    {
                        //we create business rule validation result each time
                        //because we need only string
                        estimatedValidationContext.Add(new BusinessRuleValidationResult(validationResult, resultItem.Header));
                    }
                }
            }

            return estimatedValidationContext.GetViewContents();
        }
    }
}
