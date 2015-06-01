// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColor.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
        private static Dictionary<string, PredefinedColor> dictionaryColors;

        /// <summary>
        /// The list colors.
        /// </summary>
        private static List<PredefinedColor> listColors;

        /// <summary>
        /// The list theme colors.
        /// </summary>
        private static List<PredefinedColor> listThemeColors;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor"/> class.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="r">
        /// The r.
        /// </param>
        /// <param name="g">
        /// The g.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        private PredefinedColor(byte a, byte r, byte g, byte b, string name)
        {
            this.Value = Color.FromArgb(a, r, g, b);
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor"/> class.
        /// </summary>
        /// <param name="rgb">
        /// The rgb.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        private PredefinedColor(string rgb, string name)
        {
            this.Value = Color.FromArgb(
                0xff,
                byte.Parse(rgb.Substring(0, 2), NumberStyles.HexNumber),
                byte.Parse(rgb.Substring(2, 2), NumberStyles.HexNumber),
                byte.Parse(rgb.Substring(4, 2), NumberStyles.HexNumber));
            this.Name = name;
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

                return listColors;
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

                return listThemeColors;
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
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetColorName(Color color)
        {
            Initialize();

            string code = color.ToString();
            if (dictionaryColors.ContainsKey(code))
            {
                return dictionaryColors[code].Name;
            }
            else
            {
                return code;
            }
        }

        /// <summary>
        /// The get predefined color.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="PredefinedColor"/>.
        /// </returns>
        public static PredefinedColor GetPredefinedColor(Color color)
        {
            Initialize();

            string code = color.ToString();
            if (dictionaryColors.ContainsKey(code))
            {
                return dictionaryColors[code];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The is predefined.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsPredefined(Color color)
        {
            Initialize();

            string code = color.ToString();
            return dictionaryColors.ContainsKey(code);
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
            if (listThemeColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (listThemeColors != null)
                {
                    return;
                }

                listThemeColors = new List<PredefinedColor>();

                listThemeColors.Add(new PredefinedColor("ffffff", "#ffffff"));
                listThemeColors.Add(new PredefinedColor("000000", "#000000"));
                listThemeColors.Add(new PredefinedColor("eeece1", "#eeece1"));
                listThemeColors.Add(new PredefinedColor("1f497d", "#1f497d"));
                listThemeColors.Add(new PredefinedColor("4f81bd", "#4f81bd"));
                listThemeColors.Add(new PredefinedColor("c0504d", "#c0504d"));
                listThemeColors.Add(new PredefinedColor("9bbb59", "#9bbb59"));
                listThemeColors.Add(new PredefinedColor("8064a2", "#8064a2"));
                listThemeColors.Add(new PredefinedColor("4bacc6", "#4bacc6"));
                listThemeColors.Add(new PredefinedColor("f79646", "#f79646"));

                listThemeColors.Add(new PredefinedColor("f2f2f2", "#f2f2f2"));
                listThemeColors.Add(new PredefinedColor("7f7f7f", "#7f7f7f"));
                listThemeColors.Add(new PredefinedColor("ddd9c3", "#ddd9c3"));
                listThemeColors.Add(new PredefinedColor("c6d9f0", "#c6d9f0"));
                listThemeColors.Add(new PredefinedColor("dbe5f1", "#dbe5f1"));
                listThemeColors.Add(new PredefinedColor("f2dcdb", "#f2dcdb"));
                listThemeColors.Add(new PredefinedColor("ebf1dd", "#ebf1dd"));
                listThemeColors.Add(new PredefinedColor("e5e0ec", "#e5e0ec"));
                listThemeColors.Add(new PredefinedColor("dbeef3", "#dbeef3"));
                listThemeColors.Add(new PredefinedColor("fdeada", "#fdeada"));

                listThemeColors.Add(new PredefinedColor("d8d8d8", "#d8d8d8"));
                listThemeColors.Add(new PredefinedColor("595959", "#595959"));
                listThemeColors.Add(new PredefinedColor("c4bd97", "#c4bd97"));
                listThemeColors.Add(new PredefinedColor("8db3e2", "#8db3e2"));
                listThemeColors.Add(new PredefinedColor("b8cce4", "#b8cce4"));
                listThemeColors.Add(new PredefinedColor("e5b9b7", "#e5b9b7"));
                listThemeColors.Add(new PredefinedColor("d7e3bc", "#d7e3bc"));
                listThemeColors.Add(new PredefinedColor("ccc1d9", "#ccc1d9"));
                listThemeColors.Add(new PredefinedColor("b7dde8", "#b7dde8"));
                listThemeColors.Add(new PredefinedColor("fbd5b5", "#fbd5b5"));

                listThemeColors.Add(new PredefinedColor("bfbfbf", "#bfbfbf"));
                listThemeColors.Add(new PredefinedColor("3f3f3f", "#3f3f3f"));
                listThemeColors.Add(new PredefinedColor("938953", "#938953"));
                listThemeColors.Add(new PredefinedColor("548dd4", "#548dd4"));
                listThemeColors.Add(new PredefinedColor("95b3d7", "#95b3d7"));
                listThemeColors.Add(new PredefinedColor("d99694", "#d99694"));
                listThemeColors.Add(new PredefinedColor("c3d69b", "#c3d69b"));
                listThemeColors.Add(new PredefinedColor("b2a2c7", "#b2a2c7"));
                listThemeColors.Add(new PredefinedColor("92cddc", "#92cddc"));
                listThemeColors.Add(new PredefinedColor("fac08f", "#fac08f"));

                listThemeColors.Add(new PredefinedColor("a5a5a5", "#a5a5a5"));
                listThemeColors.Add(new PredefinedColor("262626", "#262626"));
                listThemeColors.Add(new PredefinedColor("494429", "#494429"));
                listThemeColors.Add(new PredefinedColor("17365d", "#17365d"));
                listThemeColors.Add(new PredefinedColor("366092", "#366092"));
                listThemeColors.Add(new PredefinedColor("953734", "#953734"));
                listThemeColors.Add(new PredefinedColor("76923c", "#76923c"));
                listThemeColors.Add(new PredefinedColor("5f497a", "#5f497a"));
                listThemeColors.Add(new PredefinedColor("31859b", "#31859b"));
                listThemeColors.Add(new PredefinedColor("e36c09", "#e36c09"));

                listThemeColors.Add(new PredefinedColor("7f7f7f", "#7f7f7f"));
                listThemeColors.Add(new PredefinedColor("0c0c0c", "#0c0c0c"));
                listThemeColors.Add(new PredefinedColor("1d1b10", "#1d1b10"));
                listThemeColors.Add(new PredefinedColor("0f243e", "#0f243e"));
                listThemeColors.Add(new PredefinedColor("244061", "#244061"));
                listThemeColors.Add(new PredefinedColor("632423", "#632423"));
                listThemeColors.Add(new PredefinedColor("4f6128", "#4f6128"));
                listThemeColors.Add(new PredefinedColor("3f3151", "#3f3151"));
                listThemeColors.Add(new PredefinedColor("205867", "#205867"));
                listThemeColors.Add(new PredefinedColor("974806", "#974806"));
            }
        }

        /// <summary>
        /// The initialize_ alphabet.
        /// </summary>
        private static void Initialize_Alphabet()
        {
            if (listColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (listColors != null)
                {
                    return;
                }

                listColors = new List<PredefinedColor>();

                listColors.Add(new PredefinedColor(255, 240, 248, 255, "Alice Blue"));
                listColors.Add(new PredefinedColor(255, 250, 235, 215, "Antique White"));
                listColors.Add(new PredefinedColor(255, 0, 255, 255, "Aqua"));
                listColors.Add(new PredefinedColor(255, 127, 255, 212, "Aquamarine"));
                listColors.Add(new PredefinedColor(255, 240, 255, 255, "Azure"));
                listColors.Add(new PredefinedColor(255, 245, 245, 220, "Beige"));
                listColors.Add(new PredefinedColor(255, 255, 228, 196, "Bisque"));
                listColors.Add(new PredefinedColor(255, 0, 0, 0, "Black"));
                listColors.Add(new PredefinedColor(255, 255, 235, 205, "Blanched Almond"));
                listColors.Add(new PredefinedColor(255, 0, 0, 255, "Blue"));
                listColors.Add(new PredefinedColor(255, 138, 43, 226, "Blue Violet"));
                listColors.Add(new PredefinedColor(255, 165, 42, 42, "Brown"));
                listColors.Add(new PredefinedColor(255, 222, 184, 135, "Burly Wood"));
                listColors.Add(new PredefinedColor(255, 95, 158, 160, "Cadet Blue"));
                listColors.Add(new PredefinedColor(255, 127, 255, 0, "Chartreuse"));
                listColors.Add(new PredefinedColor(255, 210, 105, 30, "Chocolate"));
                listColors.Add(new PredefinedColor(255, 255, 127, 80, "Coral"));
                listColors.Add(new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"));
                listColors.Add(new PredefinedColor(255, 255, 248, 220, "Cornsilk"));
                listColors.Add(new PredefinedColor(255, 220, 20, 60, "Crimson"));
                listColors.Add(new PredefinedColor(255, 0, 255, 255, "Cyan"));
                listColors.Add(new PredefinedColor(255, 0, 0, 139, "Dark Blue"));
                listColors.Add(new PredefinedColor(255, 0, 139, 139, "Dark Cyan"));
                listColors.Add(new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"));
                listColors.Add(new PredefinedColor(255, 169, 169, 169, "Dark Gray"));
                listColors.Add(new PredefinedColor(255, 0, 100, 0, "Dark Green"));
                listColors.Add(new PredefinedColor(255, 189, 183, 107, "Dark Khaki"));
                listColors.Add(new PredefinedColor(255, 139, 0, 139, "Dark Magenta"));
                listColors.Add(new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"));
                listColors.Add(new PredefinedColor(255, 255, 140, 0, "Dark Orange"));
                listColors.Add(new PredefinedColor(255, 153, 50, 204, "Dark Orchid"));
                listColors.Add(new PredefinedColor(255, 139, 0, 0, "Dark Red"));
                listColors.Add(new PredefinedColor(255, 233, 150, 122, "Dark Salmon"));
                listColors.Add(new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"));
                listColors.Add(new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"));
                listColors.Add(new PredefinedColor(255, 47, 79, 79, "Dark Slate Gray"));
                listColors.Add(new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"));
                listColors.Add(new PredefinedColor(255, 148, 0, 211, "Dark Violet"));
                listColors.Add(new PredefinedColor(255, 255, 20, 147, "Deep Pink"));
                listColors.Add(new PredefinedColor(255, 0, 191, 255, "Deep SkyBlue"));
                listColors.Add(new PredefinedColor(255, 105, 105, 105, "Dim Gray"));
                listColors.Add(new PredefinedColor(255, 30, 144, 255, "Dodger Blue"));
                listColors.Add(new PredefinedColor(255, 178, 34, 34, "Firebrick"));
                listColors.Add(new PredefinedColor(255, 255, 250, 240, "Floral White"));
                listColors.Add(new PredefinedColor(255, 34, 139, 34, "Forest Green"));
                listColors.Add(new PredefinedColor(255, 255, 0, 255, "Fuchsia"));
                listColors.Add(new PredefinedColor(255, 220, 220, 220, "Gainsboro"));
                listColors.Add(new PredefinedColor(255, 248, 248, 255, "Ghost White"));
                listColors.Add(new PredefinedColor(255, 255, 215, 0, "Gold"));
                listColors.Add(new PredefinedColor(255, 218, 165, 32, "Goldenrod"));
                listColors.Add(new PredefinedColor(255, 128, 128, 128, "Gray"));
                listColors.Add(new PredefinedColor(255, 0, 128, 0, "Green"));
                listColors.Add(new PredefinedColor(255, 173, 255, 47, "Green Yellow"));
                listColors.Add(new PredefinedColor(255, 240, 255, 240, "Honeydew"));
                listColors.Add(new PredefinedColor(255, 255, 105, 180, "Hot Pink"));
                listColors.Add(new PredefinedColor(255, 205, 92, 92, "Indian Red"));
                listColors.Add(new PredefinedColor(255, 75, 0, 130, "Indigo"));
                listColors.Add(new PredefinedColor(255, 255, 255, 240, "Ivory"));
                listColors.Add(new PredefinedColor(255, 240, 230, 140, "Khaki"));
                listColors.Add(new PredefinedColor(255, 230, 230, 250, "Lavender"));
                listColors.Add(new PredefinedColor(255, 255, 240, 245, "Lavender Blush"));
                listColors.Add(new PredefinedColor(255, 124, 252, 0, "Lawn Green"));
                listColors.Add(new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"));
                listColors.Add(new PredefinedColor(255, 173, 216, 230, "Light Blue"));
                listColors.Add(new PredefinedColor(255, 240, 128, 128, "Light Coral"));
                listColors.Add(new PredefinedColor(255, 224, 255, 255, "Light Cyan"));
                listColors.Add(new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"));
                listColors.Add(new PredefinedColor(255, 211, 211, 211, "Light Gray"));
                listColors.Add(new PredefinedColor(255, 144, 238, 144, "Light Green"));
                listColors.Add(new PredefinedColor(255, 255, 182, 193, "Light Pink"));
                listColors.Add(new PredefinedColor(255, 255, 160, 122, "Light Salmon"));
                listColors.Add(new PredefinedColor(255, 32, 178, 170, "Light Sea Green"));
                listColors.Add(new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"));
                listColors.Add(new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"));
                listColors.Add(new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"));
                listColors.Add(new PredefinedColor(255, 255, 255, 224, "Light Yellow"));
                listColors.Add(new PredefinedColor(255, 0, 255, 0, "Lime"));
                listColors.Add(new PredefinedColor(255, 50, 205, 50, "Lime Green"));
                listColors.Add(new PredefinedColor(255, 250, 240, 230, "Linen"));
                listColors.Add(new PredefinedColor(255, 255, 0, 255, "Magenta"));
                listColors.Add(new PredefinedColor(255, 128, 0, 0, "Maroon"));
                listColors.Add(new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"));
                listColors.Add(new PredefinedColor(255, 0, 0, 205, "Medium Blue"));
                listColors.Add(new PredefinedColor(255, 186, 85, 211, "Medium Orchid"));
                listColors.Add(new PredefinedColor(255, 147, 112, 219, "Medium Purple"));
                listColors.Add(new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"));
                listColors.Add(new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"));
                listColors.Add(new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"));
                listColors.Add(new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"));
                listColors.Add(new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"));
                listColors.Add(new PredefinedColor(255, 25, 25, 112, "Midnight Blue"));
                listColors.Add(new PredefinedColor(255, 245, 255, 250, "Mint Cream"));
                listColors.Add(new PredefinedColor(255, 255, 228, 225, "Misty Rose"));
                listColors.Add(new PredefinedColor(255, 255, 228, 181, "Moccasin"));
                listColors.Add(new PredefinedColor(255, 255, 222, 173, "Navajo White"));
                listColors.Add(new PredefinedColor(255, 0, 0, 128, "Navy"));
                listColors.Add(new PredefinedColor(255, 253, 245, 230, "Old Lace"));
                listColors.Add(new PredefinedColor(255, 128, 128, 0, "Olive"));
                listColors.Add(new PredefinedColor(255, 107, 142, 35, "Olive Drab"));
                listColors.Add(new PredefinedColor(255, 255, 165, 0, "Orange"));
                listColors.Add(new PredefinedColor(255, 255, 69, 0, "Orange Red"));
                listColors.Add(new PredefinedColor(255, 218, 112, 214, "Orchid"));
                listColors.Add(new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"));
                listColors.Add(new PredefinedColor(255, 152, 251, 152, "Pale Green"));
                listColors.Add(new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"));
                listColors.Add(new PredefinedColor(255, 219, 112, 147, "Pale VioletRed"));
                listColors.Add(new PredefinedColor(255, 255, 239, 213, "Papaya Whip"));
                listColors.Add(new PredefinedColor(255, 255, 218, 185, "Peach Puff"));
                listColors.Add(new PredefinedColor(255, 205, 133, 63, "Peru"));
                listColors.Add(new PredefinedColor(255, 255, 192, 203, "Pink"));
                listColors.Add(new PredefinedColor(255, 221, 160, 221, "Plum"));
                listColors.Add(new PredefinedColor(255, 176, 224, 230, "Powder Blue"));
                listColors.Add(new PredefinedColor(255, 128, 0, 128, "Purple"));
                listColors.Add(new PredefinedColor(255, 255, 0, 0, "Red"));
                listColors.Add(new PredefinedColor(255, 188, 143, 143, "Rosy Brown"));
                listColors.Add(new PredefinedColor(255, 65, 105, 225, "Royal Blue"));
                listColors.Add(new PredefinedColor(255, 139, 69, 19, "Saddle Brown"));
                listColors.Add(new PredefinedColor(255, 250, 128, 114, "Salmon"));
                listColors.Add(new PredefinedColor(255, 244, 164, 96, "Sandy Brown"));
                listColors.Add(new PredefinedColor(255, 46, 139, 87, "Sea Green"));
                listColors.Add(new PredefinedColor(255, 255, 245, 238, "Sea Shell"));
                listColors.Add(new PredefinedColor(255, 160, 82, 45, "Sienna"));
                listColors.Add(new PredefinedColor(255, 192, 192, 192, "Silver"));
                listColors.Add(new PredefinedColor(255, 135, 206, 235, "Sky Blue"));
                listColors.Add(new PredefinedColor(255, 106, 90, 205, "Slate Blue"));
                listColors.Add(new PredefinedColor(255, 112, 128, 144, "Slate Gray"));
                listColors.Add(new PredefinedColor(255, 255, 250, 250, "Snow"));
                listColors.Add(new PredefinedColor(255, 0, 255, 127, "Spring Green"));
                listColors.Add(new PredefinedColor(255, 70, 130, 180, "Steel Blue"));
                listColors.Add(new PredefinedColor(255, 210, 180, 140, "Tan"));
                listColors.Add(new PredefinedColor(255, 0, 128, 128, "Teal"));
                listColors.Add(new PredefinedColor(255, 216, 191, 216, "Thistle"));
                listColors.Add(new PredefinedColor(255, 255, 99, 71, "Tomato"));
                listColors.Add(new PredefinedColor(0, 255, 255, 255, "Transparent"));
                listColors.Add(new PredefinedColor(255, 64, 224, 208, "Turquoise"));
                listColors.Add(new PredefinedColor(255, 238, 130, 238, "Violet"));
                listColors.Add(new PredefinedColor(255, 245, 222, 179, "Wheat"));
                listColors.Add(new PredefinedColor(255, 255, 255, 255, "White"));
                listColors.Add(new PredefinedColor(255, 245, 245, 245, "White Smoke"));
                listColors.Add(new PredefinedColor(255, 255, 255, 0, "Yellow"));
                listColors.Add(new PredefinedColor(255, 154, 205, 50, "Yellow Green"));

                dictionaryColors = new Dictionary<string, PredefinedColor>();
                foreach (PredefinedColor item in listColors)
                {
                    string code = item.Value.ToString();
                    if (dictionaryColors.ContainsKey(code))
                    {
                        continue;
                    }

                    dictionaryColors.Add(code, item);
                }
            }
        }

        /// <summary>
        /// The initialize_ windows.
        /// </summary>
        private static void Initialize_Windows()
        {
            if (listColors != null)
            {
                return;
            }

            lock (LockSingleton)
            {
                if (listColors != null)
                {
                    return;
                }

                listColors = new List<PredefinedColor>();

                listColors.Add(new PredefinedColor(0, 255, 255, 255, "Transparent"));
                listColors.Add(new PredefinedColor(255, 0, 0, 0, "Black"));
                listColors.Add(new PredefinedColor(255, 255, 255, 255, "White"));
                listColors.Add(new PredefinedColor(255, 105, 105, 105, "Dim Gray"));
                listColors.Add(new PredefinedColor(255, 128, 128, 128, "Gray"));
                listColors.Add(new PredefinedColor(255, 169, 169, 169, "Dark Gray"));
                listColors.Add(new PredefinedColor(255, 192, 192, 192, "Silver"));
                listColors.Add(new PredefinedColor(255, 211, 211, 211, "Light Gray"));
                listColors.Add(new PredefinedColor(255, 220, 220, 220, "Gainsboro"));
                listColors.Add(new PredefinedColor(255, 245, 245, 245, "White Smoke"));
                listColors.Add(new PredefinedColor(255, 128, 0, 0, "Maroon"));
                listColors.Add(new PredefinedColor(255, 139, 0, 0, "Dark Red"));
                listColors.Add(new PredefinedColor(255, 255, 0, 0, "Red"));
                listColors.Add(new PredefinedColor(255, 165, 42, 42, "Brown"));
                listColors.Add(new PredefinedColor(255, 178, 34, 34, "Firebrick"));
                listColors.Add(new PredefinedColor(255, 205, 92, 92, "Indian Red"));
                listColors.Add(new PredefinedColor(255, 255, 250, 250, "Snow"));
                listColors.Add(new PredefinedColor(255, 240, 128, 128, "Light Coral"));
                listColors.Add(new PredefinedColor(255, 188, 143, 143, "Rosy Brown"));
                listColors.Add(new PredefinedColor(255, 255, 228, 225, "Misty Rose"));
                listColors.Add(new PredefinedColor(255, 250, 128, 114, "Salmon"));
                listColors.Add(new PredefinedColor(255, 255, 99, 71, "Tomato"));
                listColors.Add(new PredefinedColor(255, 233, 150, 122, "Dark Salmon"));
                listColors.Add(new PredefinedColor(255, 255, 127, 80, "Coral"));
                listColors.Add(new PredefinedColor(255, 255, 69, 0, "Orange Red"));
                listColors.Add(new PredefinedColor(255, 255, 160, 122, "Light Salmon"));
                listColors.Add(new PredefinedColor(255, 160, 82, 45, "Sienna"));
                listColors.Add(new PredefinedColor(255, 255, 245, 238, "Sea Shell"));
                listColors.Add(new PredefinedColor(255, 210, 105, 30, "Chocolate"));
                listColors.Add(new PredefinedColor(255, 139, 69, 19, "Saddle Brown"));
                listColors.Add(new PredefinedColor(255, 244, 164, 96, "Sandy Brown"));
                listColors.Add(new PredefinedColor(255, 255, 218, 185, "Peach Puff"));
                listColors.Add(new PredefinedColor(255, 205, 133, 63, "Peru"));
                listColors.Add(new PredefinedColor(255, 250, 240, 230, "Linen"));
                listColors.Add(new PredefinedColor(255, 255, 228, 196, "Bisque"));
                listColors.Add(new PredefinedColor(255, 255, 140, 0, "Dark Orange"));
                listColors.Add(new PredefinedColor(255, 222, 184, 135, "Burly Wood"));
                listColors.Add(new PredefinedColor(255, 210, 180, 140, "Tan"));
                listColors.Add(new PredefinedColor(255, 250, 235, 215, "Antique White"));
                listColors.Add(new PredefinedColor(255, 255, 222, 173, "Navajo White"));
                listColors.Add(new PredefinedColor(255, 255, 235, 205, "Blanched Almond"));
                listColors.Add(new PredefinedColor(255, 255, 239, 213, "Papaya Whip"));
                listColors.Add(new PredefinedColor(255, 255, 228, 181, "Moccasin"));
                listColors.Add(new PredefinedColor(255, 255, 165, 0, "Orange"));
                listColors.Add(new PredefinedColor(255, 245, 222, 179, "Wheat"));
                listColors.Add(new PredefinedColor(255, 253, 245, 230, "Old Lace"));
                listColors.Add(new PredefinedColor(255, 255, 250, 240, "Floral White"));
                listColors.Add(new PredefinedColor(255, 184, 134, 11, "Dark Goldenrod"));
                listColors.Add(new PredefinedColor(255, 218, 165, 32, "Goldenrod"));
                listColors.Add(new PredefinedColor(255, 255, 248, 220, "Cornsilk"));
                listColors.Add(new PredefinedColor(255, 255, 215, 0, "Gold"));
                listColors.Add(new PredefinedColor(255, 240, 230, 140, "Khaki"));
                listColors.Add(new PredefinedColor(255, 255, 250, 205, "Lemon Chiffon"));
                listColors.Add(new PredefinedColor(255, 238, 232, 170, "Pale Goldenrod"));
                listColors.Add(new PredefinedColor(255, 189, 183, 107, "Dark Khaki"));
                listColors.Add(new PredefinedColor(255, 245, 245, 220, "Beige"));
                listColors.Add(new PredefinedColor(255, 250, 250, 210, "Light Goldenrod Yellow"));
                listColors.Add(new PredefinedColor(255, 128, 128, 0, "Olive"));
                listColors.Add(new PredefinedColor(255, 255, 255, 0, "Yellow"));
                listColors.Add(new PredefinedColor(255, 255, 255, 224, "Light Yellow"));
                listColors.Add(new PredefinedColor(255, 255, 255, 240, "Ivory"));
                listColors.Add(new PredefinedColor(255, 107, 142, 35, "Olive Drab"));
                listColors.Add(new PredefinedColor(255, 154, 205, 50, "Yellow Green"));
                listColors.Add(new PredefinedColor(255, 85, 107, 47, "Dark Olive Green"));
                listColors.Add(new PredefinedColor(255, 173, 255, 47, "Green Yellow"));
                listColors.Add(new PredefinedColor(255, 127, 255, 0, "Chartreuse"));
                listColors.Add(new PredefinedColor(255, 124, 252, 0, "Lawn Green"));
                listColors.Add(new PredefinedColor(255, 143, 188, 139, "Dark Sea Green"));
                listColors.Add(new PredefinedColor(255, 144, 238, 144, "Light Green"));
                listColors.Add(new PredefinedColor(255, 34, 139, 34, "Forest Green"));
                listColors.Add(new PredefinedColor(255, 50, 205, 50, "Lime Green"));
                listColors.Add(new PredefinedColor(255, 152, 251, 152, "Pale Green"));
                listColors.Add(new PredefinedColor(255, 0, 100, 0, "Dark Green"));
                listColors.Add(new PredefinedColor(255, 0, 128, 0, "Green"));
                listColors.Add(new PredefinedColor(255, 0, 255, 0, "Lime"));
                listColors.Add(new PredefinedColor(255, 240, 255, 240, "Honeydew"));
                listColors.Add(new PredefinedColor(255, 46, 139, 87, "Sea Green"));
                listColors.Add(new PredefinedColor(255, 60, 179, 113, "Medium Sea Green"));
                listColors.Add(new PredefinedColor(255, 0, 255, 127, "Spring Green"));
                listColors.Add(new PredefinedColor(255, 245, 255, 250, "Mint Cream"));
                listColors.Add(new PredefinedColor(255, 0, 250, 154, "Medium Spring Green"));
                listColors.Add(new PredefinedColor(255, 102, 205, 170, "Medium Aquamarine"));
                listColors.Add(new PredefinedColor(255, 127, 255, 212, "Aquamarine"));
                listColors.Add(new PredefinedColor(255, 64, 224, 208, "Turquoise"));
                listColors.Add(new PredefinedColor(255, 32, 178, 170, "Light Sea Green"));
                listColors.Add(new PredefinedColor(255, 72, 209, 204, "Medium Turquoise"));
                listColors.Add(new PredefinedColor(255, 47, 79, 79, "Dark SlateGray"));
                listColors.Add(new PredefinedColor(255, 175, 238, 238, "Pale Turquoise"));
                listColors.Add(new PredefinedColor(255, 0, 128, 128, "Teal"));
                listColors.Add(new PredefinedColor(255, 0, 139, 139, "Dark Cyan"));
                listColors.Add(new PredefinedColor(255, 0, 255, 255, "Cyan"));
                listColors.Add(new PredefinedColor(255, 0, 255, 255, "Aqua"));
                listColors.Add(new PredefinedColor(255, 224, 255, 255, "Light Cyan"));
                listColors.Add(new PredefinedColor(255, 240, 255, 255, "Azure"));
                listColors.Add(new PredefinedColor(255, 0, 206, 209, "Dark Turquoise"));
                listColors.Add(new PredefinedColor(255, 95, 158, 160, "Cadet Blue"));
                listColors.Add(new PredefinedColor(255, 176, 224, 230, "Powder Blue"));
                listColors.Add(new PredefinedColor(255, 173, 216, 230, "Light Blue"));
                listColors.Add(new PredefinedColor(255, 0, 191, 255, "Deep Sky Blue"));
                listColors.Add(new PredefinedColor(255, 135, 206, 235, "Sky Blue"));
                listColors.Add(new PredefinedColor(255, 135, 206, 250, "Light Sky Blue"));
                listColors.Add(new PredefinedColor(255, 70, 130, 180, "Steel Blue"));
                listColors.Add(new PredefinedColor(255, 240, 248, 255, "Alice Blue"));
                listColors.Add(new PredefinedColor(255, 30, 144, 255, "Dodger Blue"));
                listColors.Add(new PredefinedColor(255, 112, 128, 144, "Slate Gray"));
                listColors.Add(new PredefinedColor(255, 119, 136, 153, "Light Slate Gray"));
                listColors.Add(new PredefinedColor(255, 176, 196, 222, "Light Steel Blue"));
                listColors.Add(new PredefinedColor(255, 100, 149, 237, "Cornflower Blue"));
                listColors.Add(new PredefinedColor(255, 65, 105, 225, "Royal Blue"));
                listColors.Add(new PredefinedColor(255, 25, 25, 112, "Midnight Blue"));
                listColors.Add(new PredefinedColor(255, 230, 230, 250, "Lavender"));
                listColors.Add(new PredefinedColor(255, 0, 0, 128, "Navy"));
                listColors.Add(new PredefinedColor(255, 0, 0, 139, "Dark Blue"));
                listColors.Add(new PredefinedColor(255, 0, 0, 205, "Medium Blue"));
                listColors.Add(new PredefinedColor(255, 0, 0, 255, "Blue"));
                listColors.Add(new PredefinedColor(255, 248, 248, 255, "Ghost White"));
                listColors.Add(new PredefinedColor(255, 106, 90, 205, "Slate Blue"));
                listColors.Add(new PredefinedColor(255, 72, 61, 139, "Dark Slate Blue"));
                listColors.Add(new PredefinedColor(255, 123, 104, 238, "Medium Slate Blue"));
                listColors.Add(new PredefinedColor(255, 147, 112, 219, "Medium Purple"));
                listColors.Add(new PredefinedColor(255, 138, 43, 226, "Blue Violet"));
                listColors.Add(new PredefinedColor(255, 75, 0, 130, "Indigo"));
                listColors.Add(new PredefinedColor(255, 153, 50, 204, "Dark Orchid"));
                listColors.Add(new PredefinedColor(255, 148, 0, 211, "Dark Violet"));
                listColors.Add(new PredefinedColor(255, 186, 85, 211, "Medium Orchid"));
                listColors.Add(new PredefinedColor(255, 216, 191, 216, "Thistle"));
                listColors.Add(new PredefinedColor(255, 221, 160, 221, "Plum"));
                listColors.Add(new PredefinedColor(255, 238, 130, 238, "Violet"));
                listColors.Add(new PredefinedColor(255, 128, 0, 128, "Purple"));
                listColors.Add(new PredefinedColor(255, 139, 0, 139, "Dark Magenta"));
                listColors.Add(new PredefinedColor(255, 255, 0, 255, "Fuchsia"));
                listColors.Add(new PredefinedColor(255, 255, 0, 255, "Magenta"));
                listColors.Add(new PredefinedColor(255, 218, 112, 214, "Orchid"));
                listColors.Add(new PredefinedColor(255, 199, 21, 133, "Medium Violet Red"));
                listColors.Add(new PredefinedColor(255, 255, 20, 147, "Deep Pink"));
                listColors.Add(new PredefinedColor(255, 255, 105, 180, "Hot Pink"));
                listColors.Add(new PredefinedColor(255, 255, 240, 245, "Lavender Blush"));
                listColors.Add(new PredefinedColor(255, 219, 112, 147, "Pale Violet Red"));
                listColors.Add(new PredefinedColor(255, 220, 20, 60, "Crimson"));
                listColors.Add(new PredefinedColor(255, 255, 192, 203, "Pink"));
                listColors.Add(new PredefinedColor(255, 255, 182, 193, "Light Pink"));

                dictionaryColors = new Dictionary<string, PredefinedColor>();
                foreach (PredefinedColor item in listColors)
                {
                    string code = item.Value.ToString();
                    if (dictionaryColors.ContainsKey(code))
                    {
                        continue;
                    }

                    dictionaryColors.Add(code, item);
                }
            }
        }

        #endregion
    }
}