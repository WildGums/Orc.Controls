namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Automation;
    using Catel.Data;
    using NUnit.Framework;

    public static class ValidationTreeAssert
    {
        public static void Match(IEnumerable<ValidationContextTagTreeItem> tagNodes, ValidationContext context)
        {
            var validationResults = new Dictionary<ValidationResultType, IList<IValidationResult>>
            {
                { ValidationResultType.Warning, context.GetWarnings() },
                { ValidationResultType.Error, context.GetErrors() }
            };

            foreach (var tagItem in tagNodes)
            {
                tagItem.IsExpanded = true;
                var tag = tagItem.Tag;

                foreach (var typeItem in tagItem.TypeNodes)
                {
                    typeItem.IsExpanded = true;
                    var validationResultType = typeItem.Type;

                    var validationContextResults = validationResults[validationResultType];
                    foreach (var resultItem in typeItem.ResultNodes)
                    {
                        var resultItemMessage = resultItem.Message;
                        var foundContextResult = validationContextResults.FirstOrDefault(x => x.Message.Equals(resultItemMessage) && Equals(x.Tag, tag));
                        
                        Assert.IsNotNull(foundContextResult);
                        
                        validationContextResults.Remove(foundContextResult);
                    }
                }
            }

            foreach (var validationResult in validationResults.Values)
            {
                CollectionAssert.IsEmpty(validationResult);
            }
        }
    }
}
