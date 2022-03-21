namespace Orc.Controls.Tests.UI
{
    using Catel.Data;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(ValidationContextView))]
    [Category("UI Tests")]
    public partial class ValidationContextControlTestFacts : StyledControlTestFacts<ValidationContextView>
    {
        [Target]
        public Automation.ValidationContextView Target { get; set; }

        private static ValidationContext CreateTestValidationContext() =>
            ValidationContextBuilder.Start()
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

        //[Test]
        //public void CorrectlyTestTree()
        //{
        //    var target = Target;
        //    var model = target.Current;

        //    model.ValidationContext = validationContext;

        //    const ValidationResultType validationResultType = ValidationResultType.Warning;

        //    var items = target.GetValidationItems(tag, validationResultType);
        //    var validationResults = validationContext.GetValidations(tag)
        //        .Where(x => x.ValidationResultType == validationResultType);

        //    CollectionAssert.AreEquivalent(validationResults.Select(x => x.Message), items);
        //}

        [Test]
        public void VerifyApi()
        {
            var target = Target;
            var model = target.Current;

            var testContext = CreateTestValidationContext();

            model.ValidationContext = testContext;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsFilterVisible),
                model, nameof(model.ShowFilterBox), true,
                true, false);

            ValidationTreeAssert.Match(target, testContext);
        }
    }
} 
