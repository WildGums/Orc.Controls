namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// The predefined color.
    /// </summary>
    public class PredefinedColor : IEquatable<PredefinedColor>
    {
        #region Static Fields
        /// <summary>
        /// The lock singleton.
        /// </summary>
        private static readonly object LockSingleton = new object();

        /// <summary>
        /// The dictionary colors.
        /// </summary>
        private static Dictionary<string, PredefinedColor> _dictionaryColors;

        /// <summary>
        /// The list colors.
        /// </summary>
        private static List<PredefinedColor> _listColors;

        /// <summary>
        /// The list theme colors.
        /// </summary>
        private static List<PredefinedColor> _listThemeColors;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor" /> class.
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="name">The name.</param>
        private PredefinedColor(byte a, byte r, byte g, byte b, string name)
        {
            Value = Color.FromArgb(a, r, g, b);
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor" /> class.
        /// </summary>
        /// <param name="rgb">The rgb.</param>
        /// <param name="name">The name.</param>
        private PredefinedColor(string rgb, string name)
        {
            Value = Color.FromArgb(
                0xff,
                byte.Parse(rgb.Substring(0, 2), NumberStyles.HexNumber),
                byte.Parse(rgb.Substring(2, 2), NumberStyles.HexNumber),
                byte.Parse(rgb.Substring(4, 2), NumberStyles.HexNumber));
            Name = name;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the all.
        /// </summary>
        public static List<PredefinedColor> All
        {
            get
            {
                Initialize();

                return _listColors;
            }
        }

        /// <summary>
        /// Gets the all theme colors.
        /// </summary>
        public static List<PredefinedColor> AllThemeColors
        {
            get
            {
                InitializeThemeColors();

                return _listThemeColors;
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
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The get color name.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="string" />.</returns>
        public static string GetColorName(Color color)
        {
            Initialize();

            var code = color.ToString();
            return _dictionaryColors.ContainsKey(code) ? _dictionaryColors[code].Name : code;
        }

        /// <summary>
        /// The get predefined color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="PredefinedColor" />.</returns>
        public static PredefinedColor GetPredefinedColor(Color color)
        {
            Initialize();

            var code = color.ToString();
            return _dictionaryColors.ContainsKey(code) ? _dictionaryColors[code] : null;
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
            return _dictionaryColors.ContainsKey(code);
        }
        #endregion

        #region Methods
        /// <summary>
        /// The initialize.
        /// </summary>
        private static void Initialize()
        {
            lock (LockSingleton)
            {
                if (_listColors is not null)
                {
                    return;
                }

                var listColors = new List<PredefinedColor>();

                listColors.AddRange(InitializeAlphabet());
                listColors.AddRange(InitializeWindows());

                _listColors = listColors.Distinct().OrderBy(x => x.Name).ToList();

                _dictionaryColors = new Dictionary<string, PredefinedColor>();
                foreach (var item in _listColors)
                {
                    var code = item.Value.ToString();
                    if (_dictionaryColors.ContainsKey(code))
                    {
                        continue;
                    }

                    _dictionaryColors.Add(code, item);
                }
            }
        }

        /// <summary>
        /// The initialize theme colors.
        /// </summary>
        private static void InitializeThemeColors()
        {
            lock (LockSingleton)
            {
                if (_listThemeColors is not null)
                {
                    return;
                }

                _listThemeColors = new List<PredefinedColor>
                {
                    new PredefinedColor("ffffff", "#ffffff"),
                    new PredefinedColor("000000", "#000000"),
                    new PredefinedColor("eeece1", "#eeece1"),
                    new PredefinedColor("1f497d", "#1f497d"),
                    new PredefinedColor("4f81bd", "#4f81bd"),
                    new PredefinedColor("c0504d", "#c0504d"),
                    new PredefinedColor("9bbb59", "#9bbb59"),
                    new PredefinedColor("8064a2", "#8064a2"),
                    new PredefinedColor("4bacc6", "#4bacc6"),
                    new PredefinedColor("f79646", "#f79646"),
                    new PredefinedColor("f2f2f2", "#f2f2f2"),
                    new PredefinedColor("7f7f7f", "#7f7f7f"),
                    new PredefinedColor("ddd9c3", "#ddd9c3"),
                    new PredefinedColor("c6d9f0", "#c6d9f0"),
                    new PredefinedColor("dbe5f1", "#dbe5f1"),
                    new PredefinedColor("f2dcdb", "#f2dcdb"),
                    new PredefinedColor("ebf1dd", "#ebf1dd"),
                    new PredefinedColor("e5e0ec", "#e5e0ec"),
                    new PredefinedColor("dbeef3", "#dbeef3"),
                    new PredefinedColor("fdeada", "#fdeada"),
                    new PredefinedColor("d8d8d8", "#d8d8d8"),
                    new PredefinedColor("595959", "#595959"),
                    new PredefinedColor("c4bd97", "#c4bd97"),
                    new PredefinedColor("8db3e2", "#8db3e2"),
                    new PredefinedColor("b8cce4", "#b8cce4"),
                    new PredefinedColor("e5b9b7", "#e5b9b7"),
                    new PredefinedColor("d7e3bc", "#d7e3bc"),
                    new PredefinedColor("ccc1d9", "#ccc1d9"),
                    new PredefinedColor("b7dde8", "#b7dde8"),
                    new PredefinedColor("fbd5b5", "#fbd5b5"),
                    new PredefinedColor("bfbfbf", "#bfbfbf"),
                    new PredefinedColor("3f3f3f", "#3f3f3f"),
                    new PredefinedColor("938953", "#938953"),
                    new PredefinedColor("548dd4", "#548dd4"),
                    new PredefinedColor("95b3d7", "#95b3d7"),
                    new PredefinedColor("d99694", "#d99694"),
                    new PredefinedColor("c3d69b", "#c3d69b"),
                    new PredefinedColor("b2a2c7", "#b2a2c7"),
                    new PredefinedColor("92cddc", "#92cddc"),
                    new PredefinedColor("fac08f", "#fac08f"),
                    new PredefinedColor("a5a5a5", "#a5a5a5"),
                    new PredefinedColor("262626", "#262626"),
                    new PredefinedColor("494429", "#494429"),
                    new PredefinedColor("17365d", "#17365d"),
                    new PredefinedColor("366092", "#366092"),
                    new PredefinedColor("953734", "#953734"),
                    new PredefinedColor("76923c", "#76923c"),
                    new PredefinedColor("5f497a", "#5f497a"),
                    new PredefinedColor("31859b", "#31859b"),
                    new PredefinedColor("e36c09", "#e36c09"),
                    new PredefinedColor("7f7f7f", "#7f7f7f"),
                    new PredefinedColor("0c0c0c", "#0c0c0c"),
                    new PredefinedColor("1d1b10", "#1d1b10"),
                    new PredefinedColor("0f243e", "#0f243e"),
                    new PredefinedColor("244061", "#244061"),
                    new PredefinedColor("632423", "#632423"),
                    new PredefinedColor("4f6128", "#4f6128"),
                    new PredefinedColor("3f3151", "#3f3151"),
                    new PredefinedColor("205867", "#205867"),
                    new PredefinedColor("974806", "#974806")
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
                new PredefinedColor(255, 240, 248, 255, "Alice Blue"),
                new PredefinedColor(255, 250, 235, 215, "Antique White"),
                new PredefinedColor(255, 0, 255, 255, "Aqua"),
                new PredefinedColor(255, 127, 255, 212, "Aquamarine"),
                new PredefinedColor(255, 240, 255, 255, "Azure"),
                new PredefinedColor(255, 245, 245, 220, "Beige"),
                new PredefinedColor(255, 255, 228, 196, "Bisque"),
                new PredefinedColor(255, 0, 0, 0, "Black"),
                new PredefinedColor(255, 255, 235, 205, "Blanched Almond"),
                new PredefinedColor(255, 0, 0, 255, "Blue"),
                new PredefinedColor(255, 138, 43, 226, "Blue Violet"),
                new PredefinedColor(255, 165, 42, 42, "Brown"),
                new PredefinedColor(255, 222, 184, 135, "Burly Wood"),
                new PredefinedColor(255, 95, 158, 160, "Cadet Blue"),
                new PredefinedColor(255, 127, 255, 0, "Chartreuse"),
                new PredefinedColor(255, 210, 105, 30, "Chocolate"),
                new PredefinedColor(255, 255, 127, 80, "Coral"),
                new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"),
                new PredefinedColor(255, 255, 248, 220, "Cornsilk"),
                new PredefinedColor(255, 220, 20, 60, "Crimson"),
                new PredefinedColor(255, 0, 255, 255, "Cyan"),
                new PredefinedColor(255, 0, 0, 139, "Dark Blue"),
                new PredefinedColor(255, 0, 139, 139, "Dark Cyan"),
                new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"),
                new PredefinedColor(255, 169, 169, 169, "Dark Gray"),
                new PredefinedColor(255, 0, 100, 0, "Dark Green"),
                new PredefinedColor(255, 189, 183, 107, "Dark Khaki"),
                new PredefinedColor(255, 139, 0, 139, "Dark Magenta"),
                new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"),
                new PredefinedColor(255, 255, 140, 0, "Dark Orange"),
                new PredefinedColor(255, 153, 50, 204, "Dark Orchid"),
                new PredefinedColor(255, 139, 0, 0, "Dark Red"),
                new PredefinedColor(255, 233, 150, 122, "Dark Salmon"),
                new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"),
                new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"),
                new PredefinedColor(255, 47, 79, 79, "Dark Slate Gray"),
                new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"),
                new PredefinedColor(255, 148, 0, 211, "Dark Violet"),
                new PredefinedColor(255, 255, 20, 147, "Deep Pink"),
                new PredefinedColor(255, 0, 191, 255, "Deep SkyBlue"),
                new PredefinedColor(255, 105, 105, 105, "Dim Gray"),
                new PredefinedColor(255, 30, 144, 255, "Dodger Blue"),
                new PredefinedColor(255, 178, 34, 34, "Firebrick"),
                new PredefinedColor(255, 255, 250, 240, "Floral White"),
                new PredefinedColor(255, 34, 139, 34, "Forest Green"),
                new PredefinedColor(255, 255, 0, 255, "Fuchsia"),
                new PredefinedColor(255, 220, 220, 220, "Gainsboro"),
                new PredefinedColor(255, 248, 248, 255, "Ghost White"),
                new PredefinedColor(255, 255, 215, 0, "Gold"),
                new PredefinedColor(255, 218, 165, 32, "Goldenrod"),
                new PredefinedColor(255, 128, 128, 128, "Gray"),
                new PredefinedColor(255, 0, 128, 0, "Green"),
                new PredefinedColor(255, 173, 255, 47, "Green Yellow"),
                new PredefinedColor(255, 240, 255, 240, "Honeydew"),
                new PredefinedColor(255, 255, 105, 180, "Hot Pink"),
                new PredefinedColor(255, 205, 92, 92, "Indian Red"),
                new PredefinedColor(255, 75, 0, 130, "Indigo"),
                new PredefinedColor(255, 255, 255, 240, "Ivory"),
                new PredefinedColor(255, 240, 230, 140, "Khaki"),
                new PredefinedColor(255, 230, 230, 250, "Lavender"),
                new PredefinedColor(255, 255, 240, 245, "Lavender Blush"),
                new PredefinedColor(255, 124, 252, 0, "Lawn Green"),
                new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"),
                new PredefinedColor(255, 173, 216, 230, "Light Blue"),
                new PredefinedColor(255, 240, 128, 128, "Light Coral"),
                new PredefinedColor(255, 224, 255, 255, "Light Cyan"),
                new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"),
                new PredefinedColor(255, 211, 211, 211, "Light Gray"),
                new PredefinedColor(255, 144, 238, 144, "Light Green"),
                new PredefinedColor(255, 255, 182, 193, "Light Pink"),
                new PredefinedColor(255, 255, 160, 122, "Light Salmon"),
                new PredefinedColor(255, 32, 178, 170, "Light Sea Green"),
                new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"),
                new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"),
                new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"),
                new PredefinedColor(255, 255, 255, 224, "Light Yellow"),
                new PredefinedColor(255, 0, 255, 0, "Lime"),
                new PredefinedColor(255, 50, 205, 50, "Lime Green"),
                new PredefinedColor(255, 250, 240, 230, "Linen"),
                new PredefinedColor(255, 255, 0, 255, "Magenta"),
                new PredefinedColor(255, 128, 0, 0, "Maroon"),
                new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"),
                new PredefinedColor(255, 0, 0, 205, "Medium Blue"),
                new PredefinedColor(255, 186, 85, 211, "Medium Orchid"),
                new PredefinedColor(255, 147, 112, 219, "Medium Purple"),
                new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"),
                new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"),
                new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"),
                new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"),
                new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"),
                new PredefinedColor(255, 25, 25, 112, "Midnight Blue"),
                new PredefinedColor(255, 245, 255, 250, "Mint Cream"),
                new PredefinedColor(255, 255, 228, 225, "Misty Rose"),
                new PredefinedColor(255, 255, 228, 181, "Moccasin"),
                new PredefinedColor(255, 255, 222, 173, "Navajo White"),
                new PredefinedColor(255, 0, 0, 128, "Navy"),
                new PredefinedColor(255, 253, 245, 230, "Old Lace"),
                new PredefinedColor(255, 128, 128, 0, "Olive"),
                new PredefinedColor(255, 107, 142, 35, "Olive Drab"),
                new PredefinedColor(255, 255, 165, 0, "Orange"),
                new PredefinedColor(255, 255, 69, 0, "Orange Red"),
                new PredefinedColor(255, 218, 112, 214, "Orchid"),
                new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"),
                new PredefinedColor(255, 152, 251, 152, "Pale Green"),
                new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"),
                new PredefinedColor(255, 219, 112, 147, "Pale VioletRed"),
                new PredefinedColor(255, 255, 239, 213, "Papaya Whip"),
                new PredefinedColor(255, 255, 218, 185, "Peach Puff"),
                new PredefinedColor(255, 205, 133, 63, "Peru"),
                new PredefinedColor(255, 255, 192, 203, "Pink"),
                new PredefinedColor(255, 221, 160, 221, "Plum"),
                new PredefinedColor(255, 176, 224, 230, "Powder Blue"),
                new PredefinedColor(255, 128, 0, 128, "Purple"),
                new PredefinedColor(255, 255, 0, 0, "Red"),
                new PredefinedColor(255, 188, 143, 143, "Rosy Brown"),
                new PredefinedColor(255, 65, 105, 225, "Royal Blue"),
                new PredefinedColor(255, 139, 69, 19, "Saddle Brown"),
                new PredefinedColor(255, 250, 128, 114, "Salmon"),
                new PredefinedColor(255, 244, 164, 96, "Sandy Brown"),
                new PredefinedColor(255, 46, 139, 87, "Sea Green"),
                new PredefinedColor(255, 255, 245, 238, "Sea Shell"),
                new PredefinedColor(255, 160, 82, 45, "Sienna"),
                new PredefinedColor(255, 192, 192, 192, "Silver"),
                new PredefinedColor(255, 135, 206, 235, "Sky Blue"),
                new PredefinedColor(255, 106, 90, 205, "Slate Blue"),
                new PredefinedColor(255, 112, 128, 144, "Slate Gray"),
                new PredefinedColor(255, 255, 250, 250, "Snow"),
                new PredefinedColor(255, 0, 255, 127, "Spring Green"),
                new PredefinedColor(255, 70, 130, 180, "Steel Blue"),
                new PredefinedColor(255, 210, 180, 140, "Tan"),
                new PredefinedColor(255, 0, 128, 128, "Teal"),
                new PredefinedColor(255, 216, 191, 216, "Thistle"),
                new PredefinedColor(255, 255, 99, 71, "Tomato"),
                new PredefinedColor(0, 255, 255, 255, "Transparent"),
                new PredefinedColor(255, 64, 224, 208, "Turquoise"),
                new PredefinedColor(255, 238, 130, 238, "Violet"),
                new PredefinedColor(255, 245, 222, 179, "Wheat"),
                new PredefinedColor(255, 255, 255, 255, "White"),
                new PredefinedColor(255, 245, 245, 245, "White Smoke"),
                new PredefinedColor(255, 255, 255, 0, "Yellow"),
                new PredefinedColor(255, 154, 205, 50, "Yellow Green")
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
                new PredefinedColor(0, 255, 255, 255, "Transparent"),
                new PredefinedColor(255, 0, 0, 0, "Black"),
                new PredefinedColor(255, 255, 255, 255, "White"),
                new PredefinedColor(255, 105, 105, 105, "Dim Gray"),
                new PredefinedColor(255, 128, 128, 128, "Gray"),
                new PredefinedColor(255, 169, 169, 169, "Dark Gray"),
                new PredefinedColor(255, 192, 192, 192, "Silver"),
                new PredefinedColor(255, 211, 211, 211, "Light Gray"),
                new PredefinedColor(255, 220, 220, 220, "Gainsboro"),
                new PredefinedColor(255, 245, 245, 245, "White Smoke"),
                new PredefinedColor(255, 128, 0, 0, "Maroon"),
                new PredefinedColor(255, 139, 0, 0, "Dark Red"),
                new PredefinedColor(255, 255, 0, 0, "Red"),
                new PredefinedColor(255, 165, 42, 42, "Brown"),
                new PredefinedColor(255, 178, 34, 34, "Firebrick"),
                new PredefinedColor(255, 205, 92, 92, "Indian Red"),
                new PredefinedColor(255, 255, 250, 250, "Snow"),
                new PredefinedColor(255, 240, 128, 128, "Light Coral"),
                new PredefinedColor(255, 188, 143, 143, "Rosy Brown"),
                new PredefinedColor(255, 255, 228, 225, "Misty Rose"),
                new PredefinedColor(255, 250, 128, 114, "Salmon"),
                new PredefinedColor(255, 255, 99, 71, "Tomato"),
                new PredefinedColor(255, 233, 150, 122, "Dark Salmon"),
                new PredefinedColor(255, 255, 127, 80, "Coral"),
                new PredefinedColor(255, 255, 69, 0, "Orange Red"),
                new PredefinedColor(255, 255, 160, 122, "Light Salmon"),
                new PredefinedColor(255, 160, 82, 45, "Sienna"),
                new PredefinedColor(255, 255, 245, 238, "Sea Shell"),
                new PredefinedColor(255, 210, 105, 30, "Chocolate"),
                new PredefinedColor(255, 139, 69, 19, "Saddle Brown"),
                new PredefinedColor(255, 244, 164, 96, "Sandy Brown"),
                new PredefinedColor(255, 255, 218, 185, "Peach Puff"),
                new PredefinedColor(255, 205, 133, 63, "Peru"),
                new PredefinedColor(255, 250, 240, 230, "Linen"),
                new PredefinedColor(255, 255, 228, 196, "Bisque"),
                new PredefinedColor(255, 255, 140, 0, "Dark Orange"),
                new PredefinedColor(255, 222, 184, 135, "Burly Wood"),
                new PredefinedColor(255, 210, 180, 140, "Tan"),
                new PredefinedColor(255, 250, 235, 215, "Antique White"),
                new PredefinedColor(255, 255, 222, 173, "Navajo White"),
                new PredefinedColor(255, 255, 235, 205, "Blanched Almond"),
                new PredefinedColor(255, 255, 239, 213, "Papaya Whip"),
                new PredefinedColor(255, 255, 228, 181, "Moccasin"),
                new PredefinedColor(255, 255, 165, 0, "Orange"),
                new PredefinedColor(255, 245, 222, 179, "Wheat"),
                new PredefinedColor(255, 253, 245, 230, "Old Lace"),
                new PredefinedColor(255, 255, 250, 240, "Floral White"),
                new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"),
                new PredefinedColor(255, 218, 165, 32, "Goldenrod"),
                new PredefinedColor(255, 255, 248, 220, "Cornsilk"),
                new PredefinedColor(255, 255, 215, 0, "Gold"),
                new PredefinedColor(255, 240, 230, 140, "Khaki"),
                new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"),
                new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"),
                new PredefinedColor(255, 189, 183, 107, "Dark Khaki"),
                new PredefinedColor(255, 245, 245, 220, "Beige"),
                new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"),
                new PredefinedColor(255, 128, 128, 0, "Olive"),
                new PredefinedColor(255, 255, 255, 0, "Yellow"),
                new PredefinedColor(255, 255, 255, 224, "Light Yellow"),
                new PredefinedColor(255, 255, 255, 240, "Ivory"),
                new PredefinedColor(255, 107, 142, 35, "Olive Drab"),
                new PredefinedColor(255, 154, 205, 50, "Yellow Green"),
                new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"),
                new PredefinedColor(255, 173, 255, 47, "Green Yellow"),
                new PredefinedColor(255, 127, 255, 0, "Chartreuse"),
                new PredefinedColor(255, 124, 252, 0, "Lawn Green"),
                new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"),
                new PredefinedColor(255, 144, 238, 144, "Light Green"),
                new PredefinedColor(255, 34, 139, 34, "Forest Green"),
                new PredefinedColor(255, 50, 205, 50, "Lime Green"),
                new PredefinedColor(255, 152, 251, 152, "Pale Green"),
                new PredefinedColor(255, 0, 100, 0, "Dark Green"),
                new PredefinedColor(255, 0, 128, 0, "Green"),
                new PredefinedColor(255, 0, 255, 0, "Lime"),
                new PredefinedColor(255, 240, 255, 240, "Honeydew"),
                new PredefinedColor(255, 46, 139, 87, "Sea Green"),
                new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"),
                new PredefinedColor(255, 0, 255, 127, "Spring Green"),
                new PredefinedColor(255, 245, 255, 250, "Mint Cream"),
                new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"),
                new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"),
                new PredefinedColor(255, 127, 255, 212, "Aquamarine"),
                new PredefinedColor(255, 64, 224, 208, "Turquoise"),
                new PredefinedColor(255, 32, 178, 170, "Light Sea Green"),
                new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"),
                new PredefinedColor(255, 47, 79, 79, "Dark SlateGray"),
                new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"),
                new PredefinedColor(255, 0, 128, 128, "Teal"),
                new PredefinedColor(255, 0, 139, 139, "Dark Cyan"),
                new PredefinedColor(255, 0, 255, 255, "Cyan"),
                new PredefinedColor(255, 0, 255, 255, "Aqua"),
                new PredefinedColor(255, 224, 255, 255, "Light Cyan"),
                new PredefinedColor(255, 240, 255, 255, "Azure"),
                new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"),
                new PredefinedColor(255, 95, 158, 160, "Cadet Blue"),
                new PredefinedColor(255, 176, 224, 230, "Powder Blue"),
                new PredefinedColor(255, 173, 216, 230, "Light Blue"),
                new PredefinedColor(255, 0, 191, 255, "Deep Sky Blue"),
                new PredefinedColor(255, 135, 206, 235, "Sky Blue"),
                new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"),
                new PredefinedColor(255, 70, 130, 180, "Steel Blue"),
                new PredefinedColor(255, 240, 248, 255, "Alice Blue"),
                new PredefinedColor(255, 30, 144, 255, "Dodger Blue"),
                new PredefinedColor(255, 112, 128, 144, "Slate Gray"),
                new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"),
                new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"),
                new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"),
                new PredefinedColor(255, 65, 105, 225, "Royal Blue"),
                new PredefinedColor(255, 25, 25, 112, "Midnight Blue"),
                new PredefinedColor(255, 230, 230, 250, "Lavender"),
                new PredefinedColor(255, 0, 0, 128, "Navy"),
                new PredefinedColor(255, 0, 0, 139, "Dark Blue"),
                new PredefinedColor(255, 0, 0, 205, "Medium Blue"),
                new PredefinedColor(255, 0, 0, 255, "Blue"),
                new PredefinedColor(255, 248, 248, 255, "Ghost White"),
                new PredefinedColor(255, 106, 90, 205, "Slate Blue"),
                new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"),
                new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"),
                new PredefinedColor(255, 147, 112, 219, "Medium Purple"),
                new PredefinedColor(255, 138, 43, 226, "Blue Violet"),
                new PredefinedColor(255, 75, 0, 130, "Indigo"),
                new PredefinedColor(255, 153, 50, 204, "Dark Orchid"),
                new PredefinedColor(255, 148, 0, 211, "Dark Violet"),
                new PredefinedColor(255, 186, 85, 211, "Medium Orchid"),
                new PredefinedColor(255, 216, 191, 216, "Thistle"),
                new PredefinedColor(255, 221, 160, 221, "Plum"),
                new PredefinedColor(255, 238, 130, 238, "Violet"),
                new PredefinedColor(255, 128, 0, 128, "Purple"),
                new PredefinedColor(255, 139, 0, 139, "Dark Magenta"),
                new PredefinedColor(255, 255, 0, 255, "Fuchsia"),
                new PredefinedColor(255, 255, 0, 255, "Magenta"),
                new PredefinedColor(255, 218, 112, 214, "Orchid"),
                new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"),
                new PredefinedColor(255, 255, 20, 147, "Deep Pink"),
                new PredefinedColor(255, 255, 105, 180, "Hot Pink"),
                new PredefinedColor(255, 255, 240, 245, "Lavender Blush"),
                new PredefinedColor(255, 219, 112, 147, "Pale Violet Red"),
                new PredefinedColor(255, 220, 20, 60, "Crimson"),
                new PredefinedColor(255, 255, 192, 203, "Pink"),
                new PredefinedColor(255, 255, 182, 193, "Light Pink")
            };

            return listColors;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PredefinedColor);
        }

        public bool Equals(PredefinedColor other)
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
    #endregion
}
