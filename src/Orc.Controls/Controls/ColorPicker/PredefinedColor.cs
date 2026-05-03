namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

/// <summary>
/// The predefined color.
/// </summary>
public class PredefinedColor : IEquatable<PredefinedColor>
{
    /// <summary>
    /// The lock singleton.
    /// </summary>
    private static readonly object LockSingleton = new();

    /// <summary>
    /// The dictionary colors.
    /// </summary>
    private static Dictionary<string, PredefinedColor>? DictionaryColors;

    // Add a static property to store custom theme colors
    private static List<PredefinedColor>? _customThemeColors;

    /// <summary>
    /// The list colors.
    /// </summary>
    private static List<PredefinedColor>? ListColors;

    /// <summary>
    /// The list theme colors.
    /// </summary>
    private static List<PredefinedColor>? ListThemeColors;

    public PredefinedColor(Color color, string name)
    {
        Value = color;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PredefinedColor" /> class.
    /// </summary>
    /// <param name="a">The a.</param>
    /// <param name="r">The r.</param>
    /// <param name="g">The g.</param>
    /// <param name="b">The b.</param>
    /// <param name="name">The name.</param>
    public PredefinedColor(byte a, byte r, byte g, byte b, string name)
    {
        Value = Color.FromArgb(a, r, g, b);
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PredefinedColor" /> class.
    /// </summary>
    /// <param name="rgb">The rgb.</param>
    /// <param name="name">The name.</param>
    public PredefinedColor(string rgb, string name)
    {
        Value = Color.FromArgb(
            0xff,
            byte.Parse(rgb.Substring(0, 2), NumberStyles.HexNumber),
            byte.Parse(rgb.Substring(2, 2), NumberStyles.HexNumber),
            byte.Parse(rgb.Substring(4, 2), NumberStyles.HexNumber));
        Name = name;
    }

    /// <summary>
    /// Gets the all.
    /// </summary>
    public static List<PredefinedColor> All
    {
        get
        {
            Initialize();

            return ListColors;
        }
    }

    /// <summary>
    /// Gets the all theme colors.
    /// </summary>
    public static List<PredefinedColor> AllThemeColors
    {
        get
        {
            if (_customThemeColors is not null)
            {
                return _customThemeColors;
            }

            InitializeThemeColors();
            return ListThemeColors;
        }
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public Color Value { get; private set; }

    public static void SetCustomThemeColors(IEnumerable<PredefinedColor>? colors)
    {
        if (colors is null)
        {
            _customThemeColors = null;
            return;
        }

        _customThemeColors = [..colors];
    }


    /// <summary>
    /// The get color name.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The <see cref="string" />.</returns>
    public static string GetColorName(Color color)
    {
        Initialize();

        var code = color.ToString();
        return DictionaryColors.ContainsKey(code) ? DictionaryColors[code].Name : code;
    }

    /// <summary>
    /// The get predefined color.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The <see cref="PredefinedColor" />.</returns>
    public static PredefinedColor? GetPredefinedColor(Color color)
    {
        Initialize();

        var code = color.ToString();
        return DictionaryColors.ContainsKey(code) ? DictionaryColors[code] : null;
    }

    /// <summary>
    /// The is predefined.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The <see cref="bool" />.</returns>
    public static bool IsPredefined(Color color)
    {
        Initialize();

        var code = color.ToString();
        return DictionaryColors.ContainsKey(code);
    }

    /// <summary>
    /// The initialize.
    /// </summary>
    [MemberNotNull(nameof(DictionaryColors),
        nameof(ListColors))]
    private static void Initialize()
    {
        lock (LockSingleton)
        {
            if (ListColors is not null && DictionaryColors is not null)
            {
                return;
            }

            var listColors = new List<PredefinedColor>();

            listColors.AddRange(InitializeAlphabet());
            listColors.AddRange(InitializeWindows());

            ListColors = listColors.Distinct().OrderBy(x => x.Name).ToList();

            DictionaryColors = new Dictionary<string, PredefinedColor>();
            foreach (var item in ListColors)
            {
                var code = item.Value.ToString();
                if (DictionaryColors.ContainsKey(code))
                {
                    continue;
                }

                DictionaryColors.Add(code, item);
            }
        }
    }

    [MemberNotNull(nameof(ListThemeColors))]
    private static void InitializeThemeColors()
    {
        lock (LockSingleton)
        {
            if (ListThemeColors is not null)
            {
                return;
            }

            ListThemeColors = new()
            {
                // Basic colors
                new ("ffffff", "#ffffff"),   // White
                new ("000000", "#000000"),   // Black
                new ("f2f2f2", "#f2f2f2"),   // Light gray
                new ("7f7f7f", "#7f7f7f"),   // Medium gray
                new ("262626", "#262626"),   // Dark gray

                // Blues
                new ("4f81bd", "#4f81bd"),   // Medium blue
                new ("c6d9f0", "#c6d9f0"),   // Light blue
                new ("17365d", "#17365d"),   // Dark blue
                new ("4bacc6", "#4bacc6"),   // Cyan
                new ("dbeef3", "#dbeef3"),   // Light cyan

                // Reds
                new ("c0504d", "#c0504d"),   // Medium red
                new ("f2dcdb", "#f2dcdb"),   // Light red
                new ("632423", "#632423"),   // Dark red

                // Greens
                new ("9bbb59", "#9bbb59"),   // Medium green
                new ("ebf1dd", "#ebf1dd"),   // Light green
                new ("4f6128", "#4f6128"),   // Dark green

                // Yellows/Oranges
                new ("f79646", "#f79646"),   // Orange
                new ("fdeada", "#fdeada"),   // Light orange

                // Purples
                new ("8064a2", "#8064a2"),   // Medium purple
                new ("e5e0ec", "#e5e0ec"),   // Light purple

                // Browns/Earth tones
                new ("ddd9c3", "#ddd9c3"),   // Beige
                new ("c4bd97", "#c4bd97"),   // Khaki
                new ("938953", "#938953"),   // Bronze

                // Other useful colors
                new ("e36c09", "#e36c09"),   // Bright orange
                new ("548dd4", "#548dd4")    // Bright blue
            };
        }
    }

    /// <summary>
    /// The initialize_ alphabet.
    /// </summary>
    private static List<PredefinedColor> InitializeAlphabet()
    {
        var listColors = new List<PredefinedColor>
        {
            new (255, 240, 248, 255, "Alice Blue"),
            new (255, 250, 235, 215, "Antique White"),
            new (255, 0, 255, 255, "Aqua"),
            new (255, 127, 255, 212, "Aquamarine"),
            new (255, 240, 255, 255, "Azure"),
            new (255, 245, 245, 220, "Beige"),
            new (255, 255, 228, 196, "Bisque"),
            new (255, 0, 0, 0, "Black"),
            new (255, 255, 235, 205, "Blanched Almond"),
            new (255, 0, 0, 255, "Blue"),
            new (255, 138, 43, 226, "Blue Violet"),
            new (255, 165, 42, 42, "Brown"),
            new (255, 222, 184, 135, "Burly Wood"),
            new (255, 95, 158, 160, "Cadet Blue"),
            new (255, 127, 255, 0, "Chartreuse"),
            new (255, 210, 105, 30, "Chocolate"),
            new (255, 255, 127, 80, "Coral"),
            new (255, 100, 149, 237, "Cornflower Blue"),
            new (255, 255, 248, 220, "Cornsilk"),
            new (255, 220, 20, 60, "Crimson"),
            new (255, 0, 255, 255, "Cyan"),
            new (255, 0, 0, 139, "Dark Blue"),
            new (255, 0, 139, 139, "Dark Cyan"),
            new (255, 184, 134, 11, "Dark Goldenrod"),
            new (255, 169, 169, 169, "Dark Gray"),
            new (255, 0, 100, 0, "Dark Green"),
            new (255, 189, 183, 107, "Dark Khaki"),
            new (255, 139, 0, 139, "Dark Magenta"),
            new (255, 85, 107, 47, "Dark Olive Green"),
            new (255, 255, 140, 0, "Dark Orange"),
            new (255, 153, 50, 204, "Dark Orchid"),
            new (255, 139, 0, 0, "Dark Red"),
            new (255, 233, 150, 122, "Dark Salmon"),
            new (255, 143, 188, 139, "Dark Sea Green"),
            new (255, 72, 61, 139, "Dark Slate Blue"),
            new (255, 47, 79, 79, "Dark Slate Gray"),
            new (255, 0, 206, 209, "Dark Turquoise"),
            new (255, 148, 0, 211, "Dark Violet"),
            new (255, 255, 20, 147, "Deep Pink"),
            new (255, 0, 191, 255, "Deep SkyBlue"),
            new (255, 105, 105, 105, "Dim Gray"),
            new (255, 30, 144, 255, "Dodger Blue"),
            new (255, 178, 34, 34, "Firebrick"),
            new (255, 255, 250, 240, "Floral White"),
            new (255, 34, 139, 34, "Forest Green"),
            new (255, 255, 0, 255, "Fuchsia"),
            new (255, 220, 220, 220, "Gainsboro"),
            new (255, 248, 248, 255, "Ghost White"),
            new (255, 255, 215, 0, "Gold"),
            new (255, 218, 165, 32, "Goldenrod"),
            new (255, 128, 128, 128, "Gray"),
            new (255, 0, 128, 0, "Green"),
            new (255, 173, 255, 47, "Green Yellow"),
            new (255, 240, 255, 240, "Honeydew"),
            new (255, 255, 105, 180, "Hot Pink"),
            new (255, 205, 92, 92, "Indian Red"),
            new (255, 75, 0, 130, "Indigo"),
            new (255, 255, 255, 240, "Ivory"),
            new (255, 240, 230, 140, "Khaki"),
            new (255, 230, 230, 250, "Lavender"),
            new (255, 255, 240, 245, "Lavender Blush"),
            new (255, 124, 252, 0, "Lawn Green"),
            new (255, 255, 250, 205, "Lemon Chiffon"),
            new (255, 173, 216, 230, "Light Blue"),
            new (255, 240, 128, 128, "Light Coral"),
            new (255, 224, 255, 255, "Light Cyan"),
            new (255, 250, 250, 210, "Light Goldenrod Yellow"),
            new (255, 211, 211, 211, "Light Gray"),
            new (255, 144, 238, 144, "Light Green"),
            new (255, 255, 182, 193, "Light Pink"),
            new (255, 255, 160, 122, "Light Salmon"),
            new (255, 32, 178, 170, "Light Sea Green"),
            new (255, 135, 206, 250, "Light Sky Blue"),
            new (255, 119, 136, 153, "Light Slate Gray"),
            new (255, 176, 196, 222, "Light Steel Blue"),
            new (255, 255, 255, 224, "Light Yellow"),
            new (255, 0, 255, 0, "Lime"),
            new (255, 50, 205, 50, "Lime Green"),
            new (255, 250, 240, 230, "Linen"),
            new (255, 255, 0, 255, "Magenta"),
            new (255, 128, 0, 0, "Maroon"),
            new (255, 102, 205, 170, "Medium Aquamarine"),
            new (255, 0, 0, 205, "Medium Blue"),
            new (255, 186, 85, 211, "Medium Orchid"),
            new (255, 147, 112, 219, "Medium Purple"),
            new (255, 60, 179, 113, "Medium Sea Green"),
            new (255, 123, 104, 238, "Medium Slate Blue"),
            new (255, 0, 250, 154, "Medium Spring Green"),
            new (255, 72, 209, 204, "Medium Turquoise"),
            new (255, 199, 21, 133, "Medium Violet Red"),
            new (255, 25, 25, 112, "Midnight Blue"),
            new (255, 245, 255, 250, "Mint Cream"),
            new (255, 255, 228, 225, "Misty Rose"),
            new (255, 255, 228, 181, "Moccasin"),
            new (255, 255, 222, 173, "Navajo White"),
            new (255, 0, 0, 128, "Navy"),
            new (255, 253, 245, 230, "Old Lace"),
            new (255, 128, 128, 0, "Olive"),
            new (255, 107, 142, 35, "Olive Drab"),
            new (255, 255, 165, 0, "Orange"),
            new (255, 255, 69, 0, "Orange Red"),
            new (255, 218, 112, 214, "Orchid"),
            new (255, 238, 232, 170, "Pale Goldenrod"),
            new (255, 152, 251, 152, "Pale Green"),
            new (255, 175, 238, 238, "Pale Turquoise"),
            new (255, 219, 112, 147, "Pale VioletRed"),
            new (255, 255, 239, 213, "Papaya Whip"),
            new (255, 255, 218, 185, "Peach Puff"),
            new (255, 205, 133, 63, "Peru"),
            new (255, 255, 192, 203, "Pink"),
            new (255, 221, 160, 221, "Plum"),
            new (255, 176, 224, 230, "Powder Blue"),
            new (255, 128, 0, 128, "Purple"),
            new (255, 255, 0, 0, "Red"),
            new (255, 188, 143, 143, "Rosy Brown"),
            new (255, 65, 105, 225, "Royal Blue"),
            new (255, 139, 69, 19, "Saddle Brown"),
            new (255, 250, 128, 114, "Salmon"),
            new (255, 244, 164, 96, "Sandy Brown"),
            new (255, 46, 139, 87, "Sea Green"),
            new (255, 255, 245, 238, "Sea Shell"),
            new (255, 160, 82, 45, "Sienna"),
            new (255, 192, 192, 192, "Silver"),
            new (255, 135, 206, 235, "Sky Blue"),
            new (255, 106, 90, 205, "Slate Blue"),
            new (255, 112, 128, 144, "Slate Gray"),
            new (255, 255, 250, 250, "Snow"),
            new (255, 0, 255, 127, "Spring Green"),
            new (255, 70, 130, 180, "Steel Blue"),
            new (255, 210, 180, 140, "Tan"),
            new (255, 0, 128, 128, "Teal"),
            new (255, 216, 191, 216, "Thistle"),
            new (255, 255, 99, 71, "Tomato"),
            new (0, 255, 255, 255, "Transparent"),
            new (255, 64, 224, 208, "Turquoise"),
            new (255, 238, 130, 238, "Violet"),
            new (255, 245, 222, 179, "Wheat"),
            new (255, 255, 255, 255, "White"),
            new (255, 245, 245, 245, "White Smoke"),
            new (255, 255, 255, 0, "Yellow"),
            new (255, 154, 205, 50, "Yellow Green")
        };

        return listColors;
    }

    /// <summary>
    /// The initialize_ windows.
    /// </summary>
    private static List<PredefinedColor> InitializeWindows()
    {
        var listColors = new List<PredefinedColor>
        {
            new (0, 255, 255, 255, "Transparent"),
            new (255, 0, 0, 0, "Black"),
            new (255, 255, 255, 255, "White"),
            new (255, 105, 105, 105, "Dim Gray"),
            new (255, 128, 128, 128, "Gray"),
            new (255, 169, 169, 169, "Dark Gray"),
            new (255, 192, 192, 192, "Silver"),
            new (255, 211, 211, 211, "Light Gray"),
            new (255, 220, 220, 220, "Gainsboro"),
            new (255, 245, 245, 245, "White Smoke"),
            new (255, 128, 0, 0, "Maroon"),
            new (255, 139, 0, 0, "Dark Red"),
            new (255, 255, 0, 0, "Red"),
            new (255, 165, 42, 42, "Brown"),
            new (255, 178, 34, 34, "Firebrick"),
            new (255, 205, 92, 92, "Indian Red"),
            new (255, 255, 250, 250, "Snow"),
            new (255, 240, 128, 128, "Light Coral"),
            new (255, 188, 143, 143, "Rosy Brown"),
            new (255, 255, 228, 225, "Misty Rose"),
            new (255, 250, 128, 114, "Salmon"),
            new (255, 255, 99, 71, "Tomato"),
            new (255, 233, 150, 122, "Dark Salmon"),
            new (255, 255, 127, 80, "Coral"),
            new (255, 255, 69, 0, "Orange Red"),
            new (255, 255, 160, 122, "Light Salmon"),
            new (255, 160, 82, 45, "Sienna"),
            new (255, 255, 245, 238, "Sea Shell"),
            new (255, 210, 105, 30, "Chocolate"),
            new (255, 139, 69, 19, "Saddle Brown"),
            new (255, 244, 164, 96, "Sandy Brown"),
            new (255, 255, 218, 185, "Peach Puff"),
            new (255, 205, 133, 63, "Peru"),
            new (255, 250, 240, 230, "Linen"),
            new (255, 255, 228, 196, "Bisque"),
            new (255, 255, 140, 0, "Dark Orange"),
            new (255, 222, 184, 135, "Burly Wood"),
            new (255, 210, 180, 140, "Tan"),
            new (255, 250, 235, 215, "Antique White"),
            new (255, 255, 222, 173, "Navajo White"),
            new (255, 255, 235, 205, "Blanched Almond"),
            new (255, 255, 239, 213, "Papaya Whip"),
            new (255, 255, 228, 181, "Moccasin"),
            new (255, 255, 165, 0, "Orange"),
            new (255, 245, 222, 179, "Wheat"),
            new (255, 253, 245, 230, "Old Lace"),
            new (255, 255, 250, 240, "Floral White"),
            new (255, 184, 134, 11, "Dark Goldenrod"),
            new (255, 218, 165, 32, "Goldenrod"),
            new (255, 255, 248, 220, "Cornsilk"),
            new (255, 255, 215, 0, "Gold"),
            new (255, 240, 230, 140, "Khaki"),
            new (255, 255, 250, 205, "Lemon Chiffon"),
            new (255, 238, 232, 170, "Pale Goldenrod"),
            new (255, 189, 183, 107, "Dark Khaki"),
            new (255, 245, 245, 220, "Beige"),
            new (255, 250, 250, 210, "Light Goldenrod Yellow"),
            new (255, 128, 128, 0, "Olive"),
            new (255, 255, 255, 0, "Yellow"),
            new (255, 255, 255, 224, "Light Yellow"),
            new (255, 255, 255, 240, "Ivory"),
            new (255, 107, 142, 35, "Olive Drab"),
            new (255, 154, 205, 50, "Yellow Green"),
            new (255, 85, 107, 47, "Dark Olive Green"),
            new (255, 173, 255, 47, "Green Yellow"),
            new (255, 127, 255, 0, "Chartreuse"),
            new (255, 124, 252, 0, "Lawn Green"),
            new (255, 143, 188, 139, "Dark Sea Green"),
            new (255, 144, 238, 144, "Light Green"),
            new (255, 34, 139, 34, "Forest Green"),
            new (255, 50, 205, 50, "Lime Green"),
            new (255, 152, 251, 152, "Pale Green"),
            new (255, 0, 100, 0, "Dark Green"),
            new (255, 0, 128, 0, "Green"),
            new (255, 0, 255, 0, "Lime"),
            new (255, 240, 255, 240, "Honeydew"),
            new (255, 46, 139, 87, "Sea Green"),
            new (255, 60, 179, 113, "Medium Sea Green"),
            new (255, 0, 255, 127, "Spring Green"),
            new (255, 245, 255, 250, "Mint Cream"),
            new (255, 0, 250, 154, "Medium Spring Green"),
            new (255, 102, 205, 170, "Medium Aquamarine"),
            new (255, 127, 255, 212, "Aquamarine"),
            new (255, 64, 224, 208, "Turquoise"),
            new (255, 32, 178, 170, "Light Sea Green"),
            new (255, 72, 209, 204, "Medium Turquoise"),
            new (255, 47, 79, 79, "Dark SlateGray"),
            new (255, 175, 238, 238, "Pale Turquoise"),
            new (255, 0, 128, 128, "Teal"),
            new (255, 0, 139, 139, "Dark Cyan"),
            new (255, 0, 255, 255, "Cyan"),
            new (255, 0, 255, 255, "Aqua"),
            new (255, 224, 255, 255, "Light Cyan"),
            new (255, 240, 255, 255, "Azure"),
            new (255, 0, 206, 209, "Dark Turquoise"),
            new (255, 95, 158, 160, "Cadet Blue"),
            new (255, 176, 224, 230, "Powder Blue"),
            new (255, 173, 216, 230, "Light Blue"),
            new (255, 0, 191, 255, "Deep Sky Blue"),
            new (255, 135, 206, 235, "Sky Blue"),
            new (255, 135, 206, 250, "Light Sky Blue"),
            new (255, 70, 130, 180, "Steel Blue"),
            new (255, 240, 248, 255, "Alice Blue"),
            new (255, 30, 144, 255, "Dodger Blue"),
            new (255, 112, 128, 144, "Slate Gray"),
            new (255, 119, 136, 153, "Light Slate Gray"),
            new (255, 176, 196, 222, "Light Steel Blue"),
            new (255, 100, 149, 237, "Cornflower Blue"),
            new (255, 65, 105, 225, "Royal Blue"),
            new (255, 25, 25, 112, "Midnight Blue"),
            new (255, 230, 230, 250, "Lavender"),
            new (255, 0, 0, 128, "Navy"),
            new (255, 0, 0, 139, "Dark Blue"),
            new (255, 0, 0, 205, "Medium Blue"),
            new (255, 0, 0, 255, "Blue"),
            new (255, 248, 248, 255, "Ghost White"),
            new (255, 106, 90, 205, "Slate Blue"),
            new (255, 72, 61, 139, "Dark Slate Blue"),
            new (255, 123, 104, 238, "Medium Slate Blue"),
            new (255, 147, 112, 219, "Medium Purple"),
            new (255, 138, 43, 226, "Blue Violet"),
            new (255, 75, 0, 130, "Indigo"),
            new (255, 153, 50, 204, "Dark Orchid"),
            new (255, 148, 0, 211, "Dark Violet"),
            new (255, 186, 85, 211, "Medium Orchid"),
            new (255, 216, 191, 216, "Thistle"),
            new (255, 221, 160, 221, "Plum"),
            new (255, 238, 130, 238, "Violet"),
            new (255, 128, 0, 128, "Purple"),
            new (255, 139, 0, 139, "Dark Magenta"),
            new (255, 255, 0, 255, "Fuchsia"),
            new (255, 255, 0, 255, "Magenta"),
            new (255, 218, 112, 214, "Orchid"),
            new (255, 199, 21, 133, "Medium Violet Red"),
            new (255, 255, 20, 147, "Deep Pink"),
            new (255, 255, 105, 180, "Hot Pink"),
            new (255, 255, 240, 245, "Lavender Blush"),
            new (255, 219, 112, 147, "Pale Violet Red"),
            new (255, 220, 20, 60, "Crimson"),
            new (255, 255, 192, 203, "Pink"),
            new (255, 255, 182, 193, "Light Pink")
        };

        return listColors;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PredefinedColor);
    }

    public bool Equals(PredefinedColor? other)
    {
        return other is not null &&
               Name == other.Name &&
               Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = -244751520;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(Value);
            return hashCode;
        }
    }

    public static bool operator ==(PredefinedColor color1, PredefinedColor color2)
    {
        return EqualityComparer<PredefinedColor>.Default.Equals(color1, color2);
    }

    public static bool operator !=(PredefinedColor color1, PredefinedColor color2)
    {
        return !(color1 == color2);
    }
}
