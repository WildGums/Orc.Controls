namespace Orc.Controls;

using System;
using System.ComponentModel;
using Converters;

[TypeConverter(typeof(NumberTypeConverter))]
public readonly struct Number
{
    public Number(object value)
        : this(value, value.GetType())
    {

    }

    public Number(object value, Type? type)
    {
        var range = type?.TryGetNumberRange();
        if (range is null)
        {
            Type = null;
            Value = null;
            DValue = 0d;
            MinValue = 0d;
            MaxValue = 0d;
            IsValid = false;

            return;
        }

        Type = type;

        var dValue = Convert.ToDouble(value);
        var tValue = value.GetType() == type ? value : type?.ChangeTypeSafe(dValue);

        DValue = Convert.ToDouble(tValue);
        Value = tValue;
        MinValue = range.Value.Min;
        MaxValue = range.Value.Max;
        IsValid = DValue >= MinValue && DValue <= MaxValue;
    }


    public readonly bool IsValid;
    public readonly double DValue;
    public readonly object? Value;
    public readonly Type? Type;
    public readonly double MinValue;
    public readonly double MaxValue;

    #region Operators
    public static bool operator >(Number left, Number right)
    {
        return left.DValue > right.DValue;
    }

    public static bool operator <(Number left, Number right)
    {
        return left.DValue < right.DValue;
    }

    public static bool operator >(double left, Number right)
    {
        return left > right.DValue;
    }

    public static bool operator <(double left, Number right)
    {
        return left < right.DValue;
    }

    public static bool operator >(Number left, double right)
    {
        return left.DValue > right;
    }

    public static bool operator <(Number left, double right)
    {
        return left.DValue < right;
    }

    public static bool operator >=(Number left, Number right)
    {
        return left.DValue > right.DValue;
    }

    public static bool operator <=(Number left, Number right)
    {
        return left.DValue < right.DValue;
    }

    public static bool operator >=(double left, Number right)
    {
        return left > right.DValue;
    }

    public static bool operator <=(double left, Number right)
    {
        return left < right.DValue;
    }

    public static bool operator >=(Number left, double right)
    {
        return left.DValue > right;
    }

    public static bool operator <=(Number left, double right)
    {
        return left.DValue < right;
    }

    public static Number operator +(Number left, double right)
    {
        return new Number(left.DValue + right, left.Type);
    }

    public static Number operator +(double left, Number right)
    {
        return new Number(right.DValue + left, right.Type);
    }

    public static Number operator -(Number left, double right)
    {
        return new Number(left.DValue - right, left.Type);
    }

    public static Number operator -(double left, Number right)
    {
        return new Number(right.DValue - left, right.Type);
    }
    #endregion

    #region Converters
    public static implicit operator Number(byte value)
    {
        return new Number(value);
    }

    public static implicit operator byte(Number number)
    {
        return number.ConvertTo<byte>();
    }

    public static implicit operator Number(sbyte value)
    {
        return new Number(value);
    }

    public static implicit operator sbyte(Number number)
    {
        return number.ConvertTo<sbyte>();
    }

    public static implicit operator Number(short value)
    {
        return new Number(value);
    }

    public static implicit operator short(Number number)
    {
        return number.ConvertTo<short>();
    }

    public static implicit operator Number(ushort value)
    {
        return new Number(value);
    }

    public static implicit operator ushort(Number number)
    {
        return number.ConvertTo<ushort>();
    }

    public static implicit operator Number(int value)
    {
        return new Number(value);
    }

    public static implicit operator int(Number number)
    {
        return number.ConvertTo<int>();
    }

    public static implicit operator Number(uint value)
    {
        return new Number(value);
    }

    public static implicit operator uint(Number number)
    {
        return number.ConvertTo<uint>();
    }

    public static implicit operator Number(long value)
    {
        return new Number(value);
    }

    public static implicit operator long(Number number)
    {
        return number.ConvertTo<long>();
    }

    public static implicit operator Number(ulong value)
    {
        return new Number(value);
    }

    public static implicit operator ulong(Number number)
    {
        return number.ConvertTo<ulong>();
    }

    public static implicit operator Number(decimal value)
    {
        return new Number(value);
    }

    public static implicit operator decimal(Number number)
    {
        return number.ConvertTo<decimal>();
    }

    public static implicit operator Number(float value)
    {
        return new Number(value);
    }

    public static implicit operator float(Number number)
    {
        return number.ConvertTo<float>();
    }

    public static implicit operator Number(double value)
    {
        return new Number(value);
    }

    public static implicit operator double(Number number)
    {
        return number.DValue;
    }
    #endregion
}
