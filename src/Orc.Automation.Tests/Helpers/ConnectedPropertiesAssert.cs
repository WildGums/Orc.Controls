namespace Orc.Automation.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    public static class ConnectedPropertiesAssert
    {
        public static void VerifyIdenticalConnectedProperties(object firstObject, string firstObjectProperty, object secondObject, string secondObjectPropertyName, params object[] values)
        {
            VerifyConnectedProperties(firstObject, firstObjectProperty, secondObject, secondObjectPropertyName, values?.Select(x => new ValueTuple<object, object>(x, x)).ToArray());
        }

        public static void VerifyIdenticalConnectedProperties(object firstObject, object secondObject, string propertiesName, params object[] values)
        {
            VerifyConnectedProperties(firstObject, propertiesName, secondObject, propertiesName, values?.Select(x => new ValueTuple<object, object>(x, x)).ToArray());
        }

        public static void VerifyConnectedProperties(object firstObject, string firstObjectPropertyName, object secondObject, string secondObjectPropertyName, params (object FirstObjectPropertyValue, object SecondObjectPropertyValue)[] testValues)
        {
            var firstObjectProperty = firstObject.GetType().GetProperty(firstObjectPropertyName);
            Assert.That(firstObjectProperty, Is.Not.Null, $"Can't find property '{firstObjectPropertyName}' in '{firstObject}'");

            var secondObjectProperty = secondObject.GetType().GetProperty(secondObjectPropertyName);
            Assert.That(secondObjectProperty, Is.Not.Null, $"Can't find property '{secondObjectPropertyName}' in '{secondObject}'");
            
            var firstObjectPropertySetMethod = firstObjectProperty.GetSetMethod();
            var secondObjectPropertySetMethod = secondObjectProperty.GetSetMethod();

            var testedValuesList = testValues.ToList();

            //Add initial values to test; to return to previous state
            var firstPropertyInitialValue = firstObjectProperty.GetValue(firstObject);
            var secondPropertyInitialValue = secondObjectProperty.GetValue(secondObject);

            testedValuesList.Add(new ValueTuple<object, object>(firstPropertyInitialValue, secondPropertyInitialValue));

            foreach (var (firstObjectPropertyValue, secondObjectPropertyValue) in testedValuesList)
            {
                if (firstObjectPropertySetMethod is not null)
                {
                    //If target changed -> view should have expected value
                    firstObjectPropertySetMethod.Invoke(firstObject, new[] { firstObjectPropertyValue });
                    Assert.That(secondObjectProperty.GetValue(secondObject), Is.EqualTo(secondObjectPropertyValue));
                }

                if (secondObjectPropertySetMethod is not null)
                {
                    //If view changed -> target should have expected value
                    secondObjectPropertySetMethod.Invoke(secondObject, new[] { secondObjectPropertyValue });
                    Assert.That(firstObjectProperty.GetValue(firstObject), Is.EqualTo(firstObjectPropertyValue));
                }
            }
        }
    }
}
