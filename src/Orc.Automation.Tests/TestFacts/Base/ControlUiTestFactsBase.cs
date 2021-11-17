namespace Orc.Automation.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Automation;
    using Catel.IoC;
    using Catel.Reflection;
    using Controls;
    using NUnit.Framework;
    using Orc.Controls.Tests;

    public abstract class ControlUiTestFactsBase<TControl> : UiTestFactsBase
        where TControl : FrameworkElement
    {
        protected override string ExecutablePath => @"C:\Source\Orc.Controls\output\Debug\Orc.Automation.Host\net5.0-windows\Orc.Automation.Host.exe";
        protected override string MainWindowAutomationId => "AutomationHost";

        [SetUp]
        public virtual void SetUpTest()
        {
            var window = Setup.MainWindow;

            var testHost = window.Find<TestHostAutomationElement>(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                Assert.Fail("Can't find Test host");
            }

            if (!testHost.TryLoadControl(typeof(TControl), out var testedControlAutomationId))
            {
                Assert.Fail($"Error Message: {testedControlAutomationId}");
            }

            Thread.Sleep(1000);

            var target = testHost.Find(id: testedControlAutomationId);
            if (target is null)
            {
                Assert.Fail("Can't find target control");
            }

            ResolveTargetProperty(target);
            ResolvePartProperties(target);
        }
        
        protected virtual void ResolveTargetProperty(AutomationElement targetElement)
        {
            var targetControlProperty = GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TestTargetAttribute)));
            if (targetControlProperty is null)
            {
                Assert.Fail("Can't find target control property");
            }

            InitializePropertyWithValue(targetControlProperty, targetElement);
        }

        protected virtual void ResolvePartProperties(AutomationElement targetElement)
        {
            var controlPartProperties = GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ControlPartAttribute)))
                .Select(x => new
                {
                    PropertyInfo = x,
                    Attribute = x.GetAttribute<ControlPartAttribute>()
                })
                .ToList();

            foreach (var controlPartProperty in controlPartProperties)
            {
                var property = controlPartProperty.PropertyInfo;
                var attribute = controlPartProperty.Attribute;

                ControlType controlType = null;
                if (!string.IsNullOrWhiteSpace(attribute.ControlType))
                {
                    controlType = typeof(ControlType).GetField(attribute.ControlType)?.GetValue(null) as ControlType;
                }

                var part = targetElement.Find(id: attribute.AutomationId, name: attribute.Name, className: attribute.ClassName, controlType: controlType);

                InitializePropertyWithValue(property, part);
            }
        }

        private void InitializePropertyWithValue(PropertyInfo property, AutomationElement element)
        {
            var propertyType = property.PropertyType;
            if (typeof(AutomationElementBase).IsAssignableFrom(propertyType))
            {
                var typeFactory = this.GetTypeFactory();

                property.SetValue(this, typeFactory.CreateInstanceWithParametersAndAutoCompletion(propertyType, element));

                return;
            }

            if (propertyType == typeof(AutomationElement))
            {
                property.SetValue(this, element);

                return;
            }

            Assert.Fail("Can't find target control property is not assignable to AutomationElementBase or AutomationElement");
        }
    }
}
