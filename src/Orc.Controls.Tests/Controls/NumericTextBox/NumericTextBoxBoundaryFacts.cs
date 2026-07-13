namespace Orc.Controls.Tests.UI;

using System.Reflection;
using System.Windows;
using System.Windows.Data;
using NUnit.Framework;

[RequiresThread(System.Threading.ApartmentState.STA)]
[TestFixture]
internal class NumericTextBoxBoundaryFacts
{
    [TestCase(50d, 300d, 400d, 300d)]
    [TestCase(900d, 300d, 400d, 400d)]
    [TestCase(350d, 300d, 400d, 350d)]
    [TestCase(null, 300d, 400d, null)]
    public void CoerceToBoundariesClampsIntoRange(double? value, double min, double max, double? expected)
    {
        var numericTextBox = new NumericTextBox
        {
            MinValue = min,
            MaxValue = max
        };

        Assert.That(InvokeCoerceToBoundaries(numericTextBox, value), Is.EqualTo(expected));
    }

    [Test]
    public void OnLostFocusClampsValueBelowMinimumUpToMinimum()
    {
        var numericTextBox = new NumericTextBox
        {
            MinValue = 300,
            MaxValue = 400
        };

        numericTextBox.Text = "50";
        InvokeOnLostFocus(numericTextBox);

        Assert.That(numericTextBox.Value, Is.EqualTo(300d));
    }

    [Test]
    public void SetValueSafelyKeepsPreviousValueWhenBoundToNonNullableSource()
    {
        var source = new NonNullableValueSource();
        var numericTextBox = new NumericTextBox
        {
            DataContext = source
        };

        BindingOperations.SetBinding(numericTextBox, NumericTextBox.ValueProperty,
            new Binding(nameof(NonNullableValueSource.Number))
            {
                Mode = BindingMode.TwoWay
            });

        Assume.That(numericTextBox.Value, Is.EqualTo(42d));

        InvokeSetValueSafely(numericTextBox, null);

        Assert.That(numericTextBox.Value, Is.EqualTo(42d));
        Assert.That(source.Number, Is.EqualTo(42d));
    }

    [Test]
    public void SetValueSafelyWritesNullWhenBoundToNullableSource()
    {
        var source = new NullableValueSource();
        var numericTextBox = new NumericTextBox
        {
            DataContext = source
        };

        BindingOperations.SetBinding(numericTextBox, NumericTextBox.ValueProperty,
            new Binding(nameof(NullableValueSource.Number))
            {
                Mode = BindingMode.TwoWay
            });

        Assume.That(numericTextBox.Value, Is.EqualTo(42d));

        InvokeSetValueSafely(numericTextBox, null);

        Assert.That(numericTextBox.Value, Is.Null);
        Assert.That(source.Number, Is.Null);
    }

    [Test]
    public void OnLostFocusClearsToEmptyWhenBoundToNullableSource()
    {
        var source = new NullableValueSource();
        var numericTextBox = new NumericTextBox
        {
            IsNullValueAllowed = true,
            DataContext = source
        };

        BindingOperations.SetBinding(numericTextBox, NumericTextBox.ValueProperty,
            new Binding(nameof(NullableValueSource.Number))
            {
                Mode = BindingMode.TwoWay
            });

        numericTextBox.Text = string.Empty;
        InvokeOnLostFocus(numericTextBox);

        Assert.That(numericTextBox.Value, Is.Null);
        Assert.That(source.Number, Is.Null);
    }

    [Test]
    public void OnLostFocusKeepsPreviousValueWhenClearingNonNullableSource()
    {
        var source = new NonNullableValueSource();
        var numericTextBox = new NumericTextBox
        {
            IsNullValueAllowed = true,
            DataContext = source
        };

        BindingOperations.SetBinding(numericTextBox, NumericTextBox.ValueProperty,
            new Binding(nameof(NonNullableValueSource.Number))
            {
                Mode = BindingMode.TwoWay
            });

        numericTextBox.Text = string.Empty;
        InvokeOnLostFocus(numericTextBox);

        Assert.That(numericTextBox.Value, Is.EqualTo(42d));
        Assert.That(source.Number, Is.EqualTo(42d));
    }

    private static double? InvokeCoerceToBoundaries(NumericTextBox numericTextBox, double? value)
    {
        var method = typeof(NumericTextBox).GetMethod("CoerceToBoundaries", BindingFlags.NonPublic | BindingFlags.Instance);
        return (double?)method.Invoke(numericTextBox, new object[] { value });
    }

    private static void InvokeSetValueSafely(NumericTextBox numericTextBox, double? value)
    {
        var method = typeof(NumericTextBox).GetMethod("SetValueSafely", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(numericTextBox, new object[] { value });
    }

    private static void InvokeOnLostFocus(NumericTextBox numericTextBox)
    {
        var method = typeof(NumericTextBox).GetMethod("OnLostFocus", BindingFlags.NonPublic | BindingFlags.Instance,
            null, new[] { typeof(object), typeof(RoutedEventArgs) }, null);
        method.Invoke(numericTextBox, new object[] { numericTextBox, new RoutedEventArgs() });
    }

    private sealed class NonNullableValueSource
    {
        public double Number { get; set; } = 42d;
    }

    private sealed class NullableValueSource
    {
        public double? Number { get; set; } = 42d;
    }
}
