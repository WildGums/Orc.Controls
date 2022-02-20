namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using Catel.Data;
    using NUnit.Framework;

    public partial class ValidationContextControlTestFacts
    {
        private static IEnumerable<TestCaseData> FilterTestCases()
        {
            var filteredValidationContext = ValidationContextBuilder.Start()
                .Tag("Test_Tag")
                    .Warnings()
                        .Business("This is business warning")
                        .Field("TestProperty", "This is field warning")
                    .Errors()
                        .Business("This is business error")
                        .Field("TestProperty", "This is field error")
                        .Field("TestProperty2", "This is field error 2")
                .Result();

            yield return new TestCaseData(filteredValidationContext, "error",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Errors()
                            .Business("This is business error")
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
                    .GetViewContents()
            );

            yield return new TestCaseData(filteredValidationContext, "warning",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")
                    .Result()
                    .GetViewContents()
            );

            yield return new TestCaseData(filteredValidationContext, "field",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Warnings()
                            .Field("TestProperty", "This is field warning")
                        .Errors()
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
                    .GetViewContents()
            );

            yield return new TestCaseData(filteredValidationContext, "This is field error 2",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Errors()
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
                    .GetViewContents()
            );
        }

        [Test]
        [TestCaseSource(nameof(FilterTestCases))]
        public void CorrectlyFilter(ValidationContext validationContext, string filter,
            string expectedTreeContents)
        {
            var target = Target;
            var model = target.Current;

            model.ValidationContext = validationContext;

            target.Filter = filter;

            var treeContents = target.GetContents();
            
            Assert.That(treeContents, Is.EqualTo(expectedTreeContents));
        }
    }
}
