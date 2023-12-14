namespace Orc.Controls;

using System.Linq;

public static class NumberFormatHelper
{
    public static string GetFormat(int digits)
    {
        return new string(Enumerable.Repeat('0', digits).ToArray());
    }
}
