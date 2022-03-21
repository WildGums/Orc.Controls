namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using Catel.Data;
    using NUnit.Framework;

    public partial class ValidationContextControlTestFacts
    {
        [Test]
        [TestCaseSource(nameof(FilterTestCases))]
        public void CorrectlyFilter(ValidationContext validationContext, string filter,
            ValidationContext expectedTreeContents)
        {
            var target = Target;
            var model = target.Current;

            model.ValidationContext = validationContext;

            target.Filter = filter;

            ValidationTreeAssert.Match(target, expectedTreeContents);
        }

        [Test]
        [TestCaseSource(nameof(FilterByTypeTestCases))]
        public void CorrectlyFilterByType(ValidationContext validationContext, bool isWarningVisible, bool isErrorsVisible, ValidationContext expectedTreeContents)
        {
            var target = Target;
            var model = target.Current;

            model.ValidationContext = validationContext;

            target.IsWarningsVisible = isWarningVisible;
            target.IsErrorsVisible = isErrorsVisible;

            ValidationTreeAssert.Match(target, expectedTreeContents);
        }

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
            );

            yield return new TestCaseData(filteredValidationContext, "warning",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")
                    .Result()
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
            );

            yield return new TestCaseData(filteredValidationContext, "This is field error 2",
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Errors()
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
            );
        }
            
        private static IEnumerable<TestCaseData> FilterByTypeTestCases()
        {
            var testValidationContext = ValidationContextBuilder.Start()
                .Tag("Test_Tag")
                    .Warnings()
                        .Business("This is business warning")
                        .Field("TestProperty", "This is field warning")
                    .Errors()
                        .Business("This is business error")
                        .Field("TestProperty", "This is field error")
                        .Field("TestProperty2", "This is field error 2")

                .Tag("Test_Tag2")
                    .Warnings()
                        .Business("This is business warning")
                        .Field("TestProperty", "This is field warning")
                    .Errors()
                        .Business("This is business error")
                        .Field("TestProperty", "This is field error")
                        .Field("TestProperty2", "This is field error 2")
                .Result();

            yield return new TestCaseData(testValidationContext, 
                true, //is warnings visible
                false, //is errors visible
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")

                    .Tag("Test_Tag2")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")
                    .Result()
            );

            yield return new TestCaseData(ValidationContextBuilder.Start()
                .Tag("Test_Tag")
                    .Warnings()
                        .Business("This is business warning")
                        .Field("TestProperty", "This is field warning")

                .Tag("Test_Tag2")
                    .Warnings()
                        .Business("This is business warning")
                        .Field("TestProperty", "This is field warning")
                .Result(),
                true, //is warnings visible
                false, //is errors visible
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")

                    .Tag("Test_Tag2")
                        .Warnings()
                            .Business("This is business warning")
                            .Field("TestProperty", "This is field warning")
                    .Result()
            );

            yield return new TestCaseData(testValidationContext,
                false, //is warnings visible
                true, //is errors visible
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Errors()
                            .Business("This is business error")
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")

                    .Tag("Test_Tag2")
                        .Errors()
                            .Business("This is business error")
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
            );

            yield return new TestCaseData(
                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                    .Errors()
                    .Business("This is business error")
                    .Field("TestProperty", "This is field error")
                    .Field("TestProperty2", "This is field error 2")

                    .Tag("Test_Tag2")
                    .Errors()
                    .Business("This is business error")
                    .Field("TestProperty", "This is field error")
                    .Field("TestProperty2", "This is field error 2")
                    .Result(),

                false, //is warnings visible
                true, //is errors visible

                ValidationContextBuilder.Start()
                    .Tag("Test_Tag")
                        .Errors()
                            .Business("This is business error")
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")

                    .Tag("Test_Tag2")
                        .Errors()
                            .Business("This is business error")
                            .Field("TestProperty", "This is field error")
                            .Field("TestProperty2", "This is field error 2")
                    .Result()
            );

            yield return new TestCaseData(testValidationContext,
                false, //is warnings visible
                false, //is errors visible
                new ValidationContext()
            );

            yield return new TestCaseData(testValidationContext,
                true, //is warnings visible
                true, //is errors visible
                testValidationContext
            );
        }
    }
}
