// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColor.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Media;

    /// <summary>
    /// The predefined color.
    /// </summary>
    public class PredefinedColor
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

            string code = color.ToString();
            if (_dictionaryColors.ContainsKey(code))
            {
                return _dictionaryColors[code].Name;
            }
            else
            {
                return code;
            }
        }

        /// <summary>
        /// The get predefined color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="PredefinedColor" />.</returns>
        public static PredefinedColor GetPredefinedColor(Color color)
        {
            Initialize();

            string code = color.ToString();
            if (_dictionaryColors.ContainsKey(code))
            {
                return _dictionaryColors[code];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The is predefined.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool IsPredefined(Color color)
        {
            Initialize();

            string code = color.ToString();
            return _dictionaryColors.ContainsKey(code);
        }
        #endregion

        #region Methods
        /// <summary>
        /// The initialize.
        /// </summary>
        private static void Initialize()
        {
            Initialize_Windows();
        }

        /// <summary>
        /// The initialize theme colors.
        /// </summary>
        private static void InitializeThemeColors()
        {
            if (_listThemeColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (_listThemeColors != null)
                {
                    return;
                }

                _listThemeColors = new List<PredefinedColor>();

                _listThemeColors.Add(new PredefinedColor("ffffff", "#ffffff"));
                _listThemeColors.Add(new PredefinedColor("000000", "#000000"));
                _listThemeColors.Add(new PredefinedColor("eeece1", "#eeece1"));
                _listThemeColors.Add(new PredefinedColor("1f497d", "#1f497d"));
                _listThemeColors.Add(new PredefinedColor("4f81bd", "#4f81bd"));
                _listThemeColors.Add(new PredefinedColor("c0504d", "#c0504d"));
                _listThemeColors.Add(new PredefinedColor("9bbb59", "#9bbb59"));
                _listThemeColors.Add(new PredefinedColor("8064a2", "#8064a2"));
                _listThemeColors.Add(new PredefinedColor("4bacc6", "#4bacc6"));
                _listThemeColors.Add(new PredefinedColor("f79646", "#f79646"));

                _listThemeColors.Add(new PredefinedColor("f2f2f2", "#f2f2f2"));
                _listThemeColors.Add(new PredefinedColor("7f7f7f", "#7f7f7f"));
                _listThemeColors.Add(new PredefinedColor("ddd9c3", "#ddd9c3"));
                _listThemeColors.Add(new PredefinedColor("c6d9f0", "#c6d9f0"));
                _listThemeColors.Add(new PredefinedColor("dbe5f1", "#dbe5f1"));
                _listThemeColors.Add(new PredefinedColor("f2dcdb", "#f2dcdb"));
                _listThemeColors.Add(new PredefinedColor("ebf1dd", "#ebf1dd"));
                _listThemeColors.Add(new PredefinedColor("e5e0ec", "#e5e0ec"));
                _listThemeColors.Add(new PredefinedColor("dbeef3", "#dbeef3"));
                _listThemeColors.Add(new PredefinedColor("fdeada", "#fdeada"));

                _listThemeColors.Add(new PredefinedColor("d8d8d8", "#d8d8d8"));
                _listThemeColors.Add(new PredefinedColor("595959", "#595959"));
                _listThemeColors.Add(new PredefinedColor("c4bd97", "#c4bd97"));
                _listThemeColors.Add(new PredefinedColor("8db3e2", "#8db3e2"));
                _listThemeColors.Add(new PredefinedColor("b8cce4", "#b8cce4"));
                _listThemeColors.Add(new PredefinedColor("e5b9b7", "#e5b9b7"));
                _listThemeColors.Add(new PredefinedColor("d7e3bc", "#d7e3bc"));
                _listThemeColors.Add(new PredefinedColor("ccc1d9", "#ccc1d9"));
                _listThemeColors.Add(new PredefinedColor("b7dde8", "#b7dde8"));
                _listThemeColors.Add(new PredefinedColor("fbd5b5", "#fbd5b5"));

                _listThemeColors.Add(new PredefinedColor("bfbfbf", "#bfbfbf"));
                _listThemeColors.Add(new PredefinedColor("3f3f3f", "#3f3f3f"));
                _listThemeColors.Add(new PredefinedColor("938953", "#938953"));
                _listThemeColors.Add(new PredefinedColor("548dd4", "#548dd4"));
                _listThemeColors.Add(new PredefinedColor("95b3d7", "#95b3d7"));
                _listThemeColors.Add(new PredefinedColor("d99694", "#d99694"));
                _listThemeColors.Add(new PredefinedColor("c3d69b", "#c3d69b"));
                _listThemeColors.Add(new PredefinedColor("b2a2c7", "#b2a2c7"));
                _listThemeColors.Add(new PredefinedColor("92cddc", "#92cddc"));
                _listThemeColors.Add(new PredefinedColor("fac08f", "#fac08f"));

                _listThemeColors.Add(new PredefinedColor("a5a5a5", "#a5a5a5"));
                _listThemeColors.Add(new PredefinedColor("262626", "#262626"));
                _listThemeColors.Add(new PredefinedColor("494429", "#494429"));
                _listThemeColors.Add(new PredefinedColor("17365d", "#17365d"));
                _listThemeColors.Add(new PredefinedColor("366092", "#366092"));
                _listThemeColors.Add(new PredefinedColor("953734", "#953734"));
                _listThemeColors.Add(new PredefinedColor("76923c", "#76923c"));
                _listThemeColors.Add(new PredefinedColor("5f497a", "#5f497a"));
                _listThemeColors.Add(new PredefinedColor("31859b", "#31859b"));
                _listThemeColors.Add(new PredefinedColor("e36c09", "#e36c09"));

                _listThemeColors.Add(new PredefinedColor("7f7f7f", "#7f7f7f"));
                _listThemeColors.Add(new PredefinedColor("0c0c0c", "#0c0c0c"));
                _listThemeColors.Add(new PredefinedColor("1d1b10", "#1d1b10"));
                _listThemeColors.Add(new PredefinedColor("0f243e", "#0f243e"));
                _listThemeColors.Add(new PredefinedColor("244061", "#244061"));
                _listThemeColors.Add(new PredefinedColor("632423", "#632423"));
                _listThemeColors.Add(new PredefinedColor("4f6128", "#4f6128"));
                _listThemeColors.Add(new PredefinedColor("3f3151", "#3f3151"));
                _listThemeColors.Add(new PredefinedColor("205867", "#205867"));
                _listThemeColors.Add(new PredefinedColor("974806", "#974806"));
            }
        }

        /// <summary>
        /// The initialize_ alphabet.
        /// </summary>
        private static void Initialize_Alphabet()
        {
            if (_listColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (_listColors != null)
                {
                    return;
                }

                _listColors = new List<PredefinedColor>();

                _listColors.Add(new PredefinedColor(255, 240, 248, 255, "Alice Blue"));
                _listColors.Add(new PredefinedColor(255, 250, 235, 215, "Antique White"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 255, "Aqua"));
                _listColors.Add(new PredefinedColor(255, 127, 255, 212, "Aquamarine"));
                _listColors.Add(new PredefinedColor(255, 240, 255, 255, "Azure"));
                _listColors.Add(new PredefinedColor(255, 245, 245, 220, "Beige"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 196, "Bisque"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 0, "Black"));
                _listColors.Add(new PredefinedColor(255, 255, 235, 205, "Blanched Almond"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 255, "Blue"));
                _listColors.Add(new PredefinedColor(255, 138, 43, 226, "Blue Violet"));
                _listColors.Add(new PredefinedColor(255, 165, 42, 42, "Brown"));
                _listColors.Add(new PredefinedColor(255, 222, 184, 135, "Burly Wood"));
                _listColors.Add(new PredefinedColor(255, 95, 158, 160, "Cadet Blue"));
                _listColors.Add(new PredefinedColor(255, 127, 255, 0, "Chartreuse"));
                _listColors.Add(new PredefinedColor(255, 210, 105, 30, "Chocolate"));
                _listColors.Add(new PredefinedColor(255, 255, 127, 80, "Coral"));
                _listColors.Add(new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"));
                _listColors.Add(new PredefinedColor(255, 255, 248, 220, "Cornsilk"));
                _listColors.Add(new PredefinedColor(255, 220, 20, 60, "Crimson"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 255, "Cyan"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 139, "Dark Blue"));
                _listColors.Add(new PredefinedColor(255, 0, 139, 139, "Dark Cyan"));
                _listColors.Add(new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 169, 169, 169, "Dark Gray"));
                _listColors.Add(new PredefinedColor(255, 0, 100, 0, "Dark Green"));
                _listColors.Add(new PredefinedColor(255, 189, 183, 107, "Dark Khaki"));
                _listColors.Add(new PredefinedColor(255, 139, 0, 139, "Dark Magenta"));
                _listColors.Add(new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"));
                _listColors.Add(new PredefinedColor(255, 255, 140, 0, "Dark Orange"));
                _listColors.Add(new PredefinedColor(255, 153, 50, 204, "Dark Orchid"));
                _listColors.Add(new PredefinedColor(255, 139, 0, 0, "Dark Red"));
                _listColors.Add(new PredefinedColor(255, 233, 150, 122, "Dark Salmon"));
                _listColors.Add(new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"));
                _listColors.Add(new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 47, 79, 79, "Dark Slate Gray"));
                _listColors.Add(new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"));
                _listColors.Add(new PredefinedColor(255, 148, 0, 211, "Dark Violet"));
                _listColors.Add(new PredefinedColor(255, 255, 20, 147, "Deep Pink"));
                _listColors.Add(new PredefinedColor(255, 0, 191, 255, "Deep SkyBlue"));
                _listColors.Add(new PredefinedColor(255, 105, 105, 105, "Dim Gray"));
                _listColors.Add(new PredefinedColor(255, 30, 144, 255, "Dodger Blue"));
                _listColors.Add(new PredefinedColor(255, 178, 34, 34, "Firebrick"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 240, "Floral White"));
                _listColors.Add(new PredefinedColor(255, 34, 139, 34, "Forest Green"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 255, "Fuchsia"));
                _listColors.Add(new PredefinedColor(255, 220, 220, 220, "Gainsboro"));
                _listColors.Add(new PredefinedColor(255, 248, 248, 255, "Ghost White"));
                _listColors.Add(new PredefinedColor(255, 255, 215, 0, "Gold"));
                _listColors.Add(new PredefinedColor(255, 218, 165, 32, "Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 128, 128, 128, "Gray"));
                _listColors.Add(new PredefinedColor(255, 0, 128, 0, "Green"));
                _listColors.Add(new PredefinedColor(255, 173, 255, 47, "Green Yellow"));
                _listColors.Add(new PredefinedColor(255, 240, 255, 240, "Honeydew"));
                _listColors.Add(new PredefinedColor(255, 255, 105, 180, "Hot Pink"));
                _listColors.Add(new PredefinedColor(255, 205, 92, 92, "Indian Red"));
                _listColors.Add(new PredefinedColor(255, 75, 0, 130, "Indigo"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 240, "Ivory"));
                _listColors.Add(new PredefinedColor(255, 240, 230, 140, "Khaki"));
                _listColors.Add(new PredefinedColor(255, 230, 230, 250, "Lavender"));
                _listColors.Add(new PredefinedColor(255, 255, 240, 245, "Lavender Blush"));
                _listColors.Add(new PredefinedColor(255, 124, 252, 0, "Lawn Green"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"));
                _listColors.Add(new PredefinedColor(255, 173, 216, 230, "Light Blue"));
                _listColors.Add(new PredefinedColor(255, 240, 128, 128, "Light Coral"));
                _listColors.Add(new PredefinedColor(255, 224, 255, 255, "Light Cyan"));
                _listColors.Add(new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"));
                _listColors.Add(new PredefinedColor(255, 211, 211, 211, "Light Gray"));
                _listColors.Add(new PredefinedColor(255, 144, 238, 144, "Light Green"));
                _listColors.Add(new PredefinedColor(255, 255, 182, 193, "Light Pink"));
                _listColors.Add(new PredefinedColor(255, 255, 160, 122, "Light Salmon"));
                _listColors.Add(new PredefinedColor(255, 32, 178, 170, "Light Sea Green"));
                _listColors.Add(new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"));
                _listColors.Add(new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"));
                _listColors.Add(new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 224, "Light Yellow"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 0, "Lime"));
                _listColors.Add(new PredefinedColor(255, 50, 205, 50, "Lime Green"));
                _listColors.Add(new PredefinedColor(255, 250, 240, 230, "Linen"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 255, "Magenta"));
                _listColors.Add(new PredefinedColor(255, 128, 0, 0, "Maroon"));
                _listColors.Add(new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 205, "Medium Blue"));
                _listColors.Add(new PredefinedColor(255, 186, 85, 211, "Medium Orchid"));
                _listColors.Add(new PredefinedColor(255, 147, 112, 219, "Medium Purple"));
                _listColors.Add(new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"));
                _listColors.Add(new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"));
                _listColors.Add(new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"));
                _listColors.Add(new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"));
                _listColors.Add(new PredefinedColor(255, 25, 25, 112, "Midnight Blue"));
                _listColors.Add(new PredefinedColor(255, 245, 255, 250, "Mint Cream"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 225, "Misty Rose"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 181, "Moccasin"));
                _listColors.Add(new PredefinedColor(255, 255, 222, 173, "Navajo White"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 128, "Navy"));
                _listColors.Add(new PredefinedColor(255, 253, 245, 230, "Old Lace"));
                _listColors.Add(new PredefinedColor(255, 128, 128, 0, "Olive"));
                _listColors.Add(new PredefinedColor(255, 107, 142, 35, "Olive Drab"));
                _listColors.Add(new PredefinedColor(255, 255, 165, 0, "Orange"));
                _listColors.Add(new PredefinedColor(255, 255, 69, 0, "Orange Red"));
                _listColors.Add(new PredefinedColor(255, 218, 112, 214, "Orchid"));
                _listColors.Add(new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 152, 251, 152, "Pale Green"));
                _listColors.Add(new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"));
                _listColors.Add(new PredefinedColor(255, 219, 112, 147, "Pale VioletRed"));
                _listColors.Add(new PredefinedColor(255, 255, 239, 213, "Papaya Whip"));
                _listColors.Add(new PredefinedColor(255, 255, 218, 185, "Peach Puff"));
                _listColors.Add(new PredefinedColor(255, 205, 133, 63, "Peru"));
                _listColors.Add(new PredefinedColor(255, 255, 192, 203, "Pink"));
                _listColors.Add(new PredefinedColor(255, 221, 160, 221, "Plum"));
                _listColors.Add(new PredefinedColor(255, 176, 224, 230, "Powder Blue"));
                _listColors.Add(new PredefinedColor(255, 128, 0, 128, "Purple"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 0, "Red"));
                _listColors.Add(new PredefinedColor(255, 188, 143, 143, "Rosy Brown"));
                _listColors.Add(new PredefinedColor(255, 65, 105, 225, "Royal Blue"));
                _listColors.Add(new PredefinedColor(255, 139, 69, 19, "Saddle Brown"));
                _listColors.Add(new PredefinedColor(255, 250, 128, 114, "Salmon"));
                _listColors.Add(new PredefinedColor(255, 244, 164, 96, "Sandy Brown"));
                _listColors.Add(new PredefinedColor(255, 46, 139, 87, "Sea Green"));
                _listColors.Add(new PredefinedColor(255, 255, 245, 238, "Sea Shell"));
                _listColors.Add(new PredefinedColor(255, 160, 82, 45, "Sienna"));
                _listColors.Add(new PredefinedColor(255, 192, 192, 192, "Silver"));
                _listColors.Add(new PredefinedColor(255, 135, 206, 235, "Sky Blue"));
                _listColors.Add(new PredefinedColor(255, 106, 90, 205, "Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 112, 128, 144, "Slate Gray"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 250, "Snow"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 127, "Spring Green"));
                _listColors.Add(new PredefinedColor(255, 70, 130, 180, "Steel Blue"));
                _listColors.Add(new PredefinedColor(255, 210, 180, 140, "Tan"));
                _listColors.Add(new PredefinedColor(255, 0, 128, 128, "Teal"));
                _listColors.Add(new PredefinedColor(255, 216, 191, 216, "Thistle"));
                _listColors.Add(new PredefinedColor(255, 255, 99, 71, "Tomato"));
                _listColors.Add(new PredefinedColor(0, 255, 255, 255, "Transparent"));
                _listColors.Add(new PredefinedColor(255, 64, 224, 208, "Turquoise"));
                _listColors.Add(new PredefinedColor(255, 238, 130, 238, "Violet"));
                _listColors.Add(new PredefinedColor(255, 245, 222, 179, "Wheat"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 255, "White"));
                _listColors.Add(new PredefinedColor(255, 245, 245, 245, "White Smoke"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 0, "Yellow"));
                _listColors.Add(new PredefinedColor(255, 154, 205, 50, "Yellow Green"));

                _dictionaryColors = new Dictionary<string, PredefinedColor>();
                foreach (PredefinedColor item in _listColors)
                {
                    string code = item.Value.ToString();
                    if (_dictionaryColors.ContainsKey(code))
                    {
                        continue;
                    }

                    _dictionaryColors.Add(code, item);
                }
            }
        }

        /// <summary>
        /// The initialize_ windows.
        /// </summary>
        private static void Initialize_Windows()
        {
            if (_listColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (_listColors != null)
                {
                    return;
                }

                _listColors = new List<PredefinedColor>();

                _listColors.Add(new PredefinedColor(0, 255, 255, 255, "Transparent"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 0, "Black"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 255, "White"));
                _listColors.Add(new PredefinedColor(255, 105, 105, 105, "Dim Gray"));
                _listColors.Add(new PredefinedColor(255, 128, 128, 128, "Gray"));
                _listColors.Add(new PredefinedColor(255, 169, 169, 169, "Dark Gray"));
                _listColors.Add(new PredefinedColor(255, 192, 192, 192, "Silver"));
                _listColors.Add(new PredefinedColor(255, 211, 211, 211, "Light Gray"));
                _listColors.Add(new PredefinedColor(255, 220, 220, 220, "Gainsboro"));
                _listColors.Add(new PredefinedColor(255, 245, 245, 245, "White Smoke"));
                _listColors.Add(new PredefinedColor(255, 128, 0, 0, "Maroon"));
                _listColors.Add(new PredefinedColor(255, 139, 0, 0, "Dark Red"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 0, "Red"));
                _listColors.Add(new PredefinedColor(255, 165, 42, 42, "Brown"));
                _listColors.Add(new PredefinedColor(255, 178, 34, 34, "Firebrick"));
                _listColors.Add(new PredefinedColor(255, 205, 92, 92, "Indian Red"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 250, "Snow"));
                _listColors.Add(new PredefinedColor(255, 240, 128, 128, "Light Coral"));
                _listColors.Add(new PredefinedColor(255, 188, 143, 143, "Rosy Brown"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 225, "Misty Rose"));
                _listColors.Add(new PredefinedColor(255, 250, 128, 114, "Salmon"));
                _listColors.Add(new PredefinedColor(255, 255, 99, 71, "Tomato"));
                _listColors.Add(new PredefinedColor(255, 233, 150, 122, "Dark Salmon"));
                _listColors.Add(new PredefinedColor(255, 255, 127, 80, "Coral"));
                _listColors.Add(new PredefinedColor(255, 255, 69, 0, "Orange Red"));
                _listColors.Add(new PredefinedColor(255, 255, 160, 122, "Light Salmon"));
                _listColors.Add(new PredefinedColor(255, 160, 82, 45, "Sienna"));
                _listColors.Add(new PredefinedColor(255, 255, 245, 238, "Sea Shell"));
                _listColors.Add(new PredefinedColor(255, 210, 105, 30, "Chocolate"));
                _listColors.Add(new PredefinedColor(255, 139, 69, 19, "Saddle Brown"));
                _listColors.Add(new PredefinedColor(255, 244, 164, 96, "Sandy Brown"));
                _listColors.Add(new PredefinedColor(255, 255, 218, 185, "Peach Puff"));
                _listColors.Add(new PredefinedColor(255, 205, 133, 63, "Peru"));
                _listColors.Add(new PredefinedColor(255, 250, 240, 230, "Linen"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 196, "Bisque"));
                _listColors.Add(new PredefinedColor(255, 255, 140, 0, "Dark Orange"));
                _listColors.Add(new PredefinedColor(255, 222, 184, 135, "Burly Wood"));
                _listColors.Add(new PredefinedColor(255, 210, 180, 140, "Tan"));
                _listColors.Add(new PredefinedColor(255, 250, 235, 215, "Antique White"));
                _listColors.Add(new PredefinedColor(255, 255, 222, 173, "Navajo White"));
                _listColors.Add(new PredefinedColor(255, 255, 235, 205, "Blanched Almond"));
                _listColors.Add(new PredefinedColor(255, 255, 239, 213, "Papaya Whip"));
                _listColors.Add(new PredefinedColor(255, 255, 228, 181, "Moccasin"));
                _listColors.Add(new PredefinedColor(255, 255, 165, 0, "Orange"));
                _listColors.Add(new PredefinedColor(255, 245, 222, 179, "Wheat"));
                _listColors.Add(new PredefinedColor(255, 253, 245, 230, "Old Lace"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 240, "Floral White"));
                _listColors.Add(new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 218, 165, 32, "Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 255, 248, 220, "Cornsilk"));
                _listColors.Add(new PredefinedColor(255, 255, 215, 0, "Gold"));
                _listColors.Add(new PredefinedColor(255, 240, 230, 140, "Khaki"));
                _listColors.Add(new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"));
                _listColors.Add(new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"));
                _listColors.Add(new PredefinedColor(255, 189, 183, 107, "Dark Khaki"));
                _listColors.Add(new PredefinedColor(255, 245, 245, 220, "Beige"));
                _listColors.Add(new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"));
                _listColors.Add(new PredefinedColor(255, 128, 128, 0, "Olive"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 0, "Yellow"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 224, "Light Yellow"));
                _listColors.Add(new PredefinedColor(255, 255, 255, 240, "Ivory"));
                _listColors.Add(new PredefinedColor(255, 107, 142, 35, "Olive Drab"));
                _listColors.Add(new PredefinedColor(255, 154, 205, 50, "Yellow Green"));
                _listColors.Add(new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"));
                _listColors.Add(new PredefinedColor(255, 173, 255, 47, "Green Yellow"));
                _listColors.Add(new PredefinedColor(255, 127, 255, 0, "Chartreuse"));
                _listColors.Add(new PredefinedColor(255, 124, 252, 0, "Lawn Green"));
                _listColors.Add(new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"));
                _listColors.Add(new PredefinedColor(255, 144, 238, 144, "Light Green"));
                _listColors.Add(new PredefinedColor(255, 34, 139, 34, "Forest Green"));
                _listColors.Add(new PredefinedColor(255, 50, 205, 50, "Lime Green"));
                _listColors.Add(new PredefinedColor(255, 152, 251, 152, "Pale Green"));
                _listColors.Add(new PredefinedColor(255, 0, 100, 0, "Dark Green"));
                _listColors.Add(new PredefinedColor(255, 0, 128, 0, "Green"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 0, "Lime"));
                _listColors.Add(new PredefinedColor(255, 240, 255, 240, "Honeydew"));
                _listColors.Add(new PredefinedColor(255, 46, 139, 87, "Sea Green"));
                _listColors.Add(new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 127, "Spring Green"));
                _listColors.Add(new PredefinedColor(255, 245, 255, 250, "Mint Cream"));
                _listColors.Add(new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"));
                _listColors.Add(new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"));
                _listColors.Add(new PredefinedColor(255, 127, 255, 212, "Aquamarine"));
                _listColors.Add(new PredefinedColor(255, 64, 224, 208, "Turquoise"));
                _listColors.Add(new PredefinedColor(255, 32, 178, 170, "Light Sea Green"));
                _listColors.Add(new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"));
                _listColors.Add(new PredefinedColor(255, 47, 79, 79, "Dark SlateGray"));
                _listColors.Add(new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"));
                _listColors.Add(new PredefinedColor(255, 0, 128, 128, "Teal"));
                _listColors.Add(new PredefinedColor(255, 0, 139, 139, "Dark Cyan"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 255, "Cyan"));
                _listColors.Add(new PredefinedColor(255, 0, 255, 255, "Aqua"));
                _listColors.Add(new PredefinedColor(255, 224, 255, 255, "Light Cyan"));
                _listColors.Add(new PredefinedColor(255, 240, 255, 255, "Azure"));
                _listColors.Add(new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"));
                _listColors.Add(new PredefinedColor(255, 95, 158, 160, "Cadet Blue"));
                _listColors.Add(new PredefinedColor(255, 176, 224, 230, "Powder Blue"));
                _listColors.Add(new PredefinedColor(255, 173, 216, 230, "Light Blue"));
                _listColors.Add(new PredefinedColor(255, 0, 191, 255, "Deep Sky Blue"));
                _listColors.Add(new PredefinedColor(255, 135, 206, 235, "Sky Blue"));
                _listColors.Add(new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"));
                _listColors.Add(new PredefinedColor(255, 70, 130, 180, "Steel Blue"));
                _listColors.Add(new PredefinedColor(255, 240, 248, 255, "Alice Blue"));
                _listColors.Add(new PredefinedColor(255, 30, 144, 255, "Dodger Blue"));
                _listColors.Add(new PredefinedColor(255, 112, 128, 144, "Slate Gray"));
                _listColors.Add(new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"));
                _listColors.Add(new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"));
                _listColors.Add(new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"));
                _listColors.Add(new PredefinedColor(255, 65, 105, 225, "Royal Blue"));
                _listColors.Add(new PredefinedColor(255, 25, 25, 112, "Midnight Blue"));
                _listColors.Add(new PredefinedColor(255, 230, 230, 250, "Lavender"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 128, "Navy"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 139, "Dark Blue"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 205, "Medium Blue"));
                _listColors.Add(new PredefinedColor(255, 0, 0, 255, "Blue"));
                _listColors.Add(new PredefinedColor(255, 248, 248, 255, "Ghost White"));
                _listColors.Add(new PredefinedColor(255, 106, 90, 205, "Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"));
                _listColors.Add(new PredefinedColor(255, 147, 112, 219, "Medium Purple"));
                _listColors.Add(new PredefinedColor(255, 138, 43, 226, "Blue Violet"));
                _listColors.Add(new PredefinedColor(255, 75, 0, 130, "Indigo"));
                _listColors.Add(new PredefinedColor(255, 153, 50, 204, "Dark Orchid"));
                _listColors.Add(new PredefinedColor(255, 148, 0, 211, "Dark Violet"));
                _listColors.Add(new PredefinedColor(255, 186, 85, 211, "Medium Orchid"));
                _listColors.Add(new PredefinedColor(255, 216, 191, 216, "Thistle"));
                _listColors.Add(new PredefinedColor(255, 221, 160, 221, "Plum"));
                _listColors.Add(new PredefinedColor(255, 238, 130, 238, "Violet"));
                _listColors.Add(new PredefinedColor(255, 128, 0, 128, "Purple"));
                _listColors.Add(new PredefinedColor(255, 139, 0, 139, "Dark Magenta"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 255, "Fuchsia"));
                _listColors.Add(new PredefinedColor(255, 255, 0, 255, "Magenta"));
                _listColors.Add(new PredefinedColor(255, 218, 112, 214, "Orchid"));
                _listColors.Add(new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"));
                _listColors.Add(new PredefinedColor(255, 255, 20, 147, "Deep Pink"));
                _listColors.Add(new PredefinedColor(255, 255, 105, 180, "Hot Pink"));
                _listColors.Add(new PredefinedColor(255, 255, 240, 245, "Lavender Blush"));
                _listColors.Add(new PredefinedColor(255, 219, 112, 147, "Pale Violet Red"));
                _listColors.Add(new PredefinedColor(255, 220, 20, 60, "Crimson"));
                _listColors.Add(new PredefinedColor(255, 255, 192, 203, "Pink"));
                _listColors.Add(new PredefinedColor(255, 255, 182, 193, "Light Pink"));

                _dictionaryColors = new Dictionary<string, PredefinedColor>();
                foreach (PredefinedColor item in _listColors)
                {
                    string code = item.Value.ToString();
                    if (_dictionaryColors.ContainsKey(code))
                    {
                        continue;
                    }

                    _dictionaryColors.Add(code, item);
                }
            }
        }
        #endregion
    }
}