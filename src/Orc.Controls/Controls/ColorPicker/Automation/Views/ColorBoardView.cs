namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ColorBoardMap : AutomationBase
    {
        private const int HsvTabIndex = 0;
        private const int ColorTabIndex = 1;

        public static string TabId = nameof(Tab);
        public static string HSVSliderId = nameof(HSVSlider);
        public static string ASliderId = nameof(ASlider);
        public static string RSliderId = nameof(RSlider);
        public static string GSliderId = nameof(GSlider);
        public static string BSliderId = nameof(BSlider);
        public static string AEditId = nameof(AEdit);
        public static string REditId = nameof(REdit);
        public static string GEditId = nameof(GEdit);
        public static string BEditId = nameof(BEdit);
        public static string ColorEditId = nameof(ColorEdit);
        public static string ColorComboBoxId = nameof(ColorComboBox);
        public static string SelectButtonId = nameof(SelectButton);
        public static string CancelButtonId = nameof(CancelButton);
        public static string ThemeColorsListBoxId = nameof(ThemeColorsListBox);
        public static string RecentColorsListBoxId = nameof(RecentColorsListBox);

        private readonly ColorBoard _target;

        private HsvCanvasColorBoardPart _hsvCanvas;
        private Tab _tab;

        public ColorBoardMap(ColorBoard target)
            : base(target.Element)
        {
            _target = target;
        }

        public override By By => new (Element, Tab);

        public Tab Tab => _tab ??= base.By.Id().One<Tab>();

        public List ThemeColorsListBox => By.Tab(ColorTabIndex).Id().One<List>();
        public List RecentColorsListBox => By.Tab(ColorTabIndex).Id().One<List>();

        //HSV Tab item elements
        public Slider HSVSlider => By.Tab(HsvTabIndex).Id().One<Slider>();
        public Slider ASlider => By.Tab(HsvTabIndex).Id().One<Slider>();
        public Slider RSlider => By.Tab(HsvTabIndex).Id().One<Slider>();
        public Slider GSlider => By.Tab(HsvTabIndex).Id().One<Slider>();
        public Slider BSlider => By.Tab(HsvTabIndex).Id().One<Slider>();
        public Edit AEdit => By.Tab(HsvTabIndex).Id().One<Edit>();
        public Edit REdit => By.Tab(HsvTabIndex).Id().One<Edit>();
        public Edit GEdit => By.Tab(HsvTabIndex).Id().One<Edit>();
        public Edit BEdit => By.Tab(HsvTabIndex).Id().One<Edit>();
        public HsvCanvasColorBoardPart HsvCanvas => Tab.InTab(HsvTabIndex, () => _hsvCanvas ??= new(_target));


        //Out of tab elements
        public Edit ColorEdit => By.Id().One<Edit>();
        public ComboBox ColorComboBox => By.Id().One<ComboBox>();

        //Apply/Cancel buttons
        public Button SelectButton => By.Id().One<Button>();
        public Button CancelButton => By.Id().One<Button>();
    }

    public class ColorBoardView : AutomationBase
    {
        private readonly ColorBoardMap _map;

        public ColorBoardView(ColorBoard target)
            : base(target.Element)
        {
            _map = new ColorBoardMap(target);
        }

        /// <summary>
        /// Set colors using sliders
        /// </summary>
        public Color ArgbColor
        {
            get => Color.FromArgb((byte)_map.ASlider.Value, (byte)_map.RSlider.Value, (byte)_map.GSlider.Value, (byte)_map.BSlider.Value);
            set => SetARgbColor(value);
        }

        /// <summary>
        /// Set colors using editors
        /// </summary>
        public Color ArgbColorAlt
        {
            get => Color.FromArgb(EditToByte(_map.AEdit), EditToByte(_map.REdit), EditToByte(_map.GEdit), EditToByte(_map.BEdit));
            set => SetARgbColorAlt(value);
        }

        public Color HsvColor
        {
            get
            {
                var sv = _map.HsvCanvas.GetSV();
                var h = _map.HSVSlider.Value;

                var hsvColor = ColorHelper.HSV2RGB(h, sv.X, sv.Y);

                //Alpha channel we should take from other place
                return Color.FromArgb(EditToByte(_map.AEdit), hsvColor.R, hsvColor.G, hsvColor.B);
            }
            set => SetHsvColor(value);
        }

        public string ColorName
        {
            get
            {
                var colorEditText = _map.ColorEdit.Text;
                if (!string.IsNullOrWhiteSpace(colorEditText))
                {
                    return colorEditText;
                }

                var text = PredefinedColorName;
                return text;
            }
            set
            {
                var colorEdit = _map.ColorEdit;

                colorEdit.SetFocus();

                colorEdit.Text = value;

                //We should move focus, for example on 1st element
                _map.AEdit.SetFocus();
            }
        }

        public string PredefinedColorName
        {
            get => _map.ColorComboBox.SelectedItem?.Find<PredefinedColorItem>()?.Text;
            set
            {
                var colorComboBox = _map.ColorComboBox;
                colorComboBox.InvokeInExpandState(() =>
                {
                    var itemToSelect = colorComboBox.Items?.FirstOrDefault(x => Equals(GetColorName(x), value));
                    itemToSelect?.Select();
                });
            }
        }

        public Color? SelectedThemeColor
        {
            get => _map.ThemeColorsListBox?.SelectedItem?.Find<PredefinedColorItem>()?.Color;
            set
            {
                var themeColor = ThemeColors.FirstOrDefault(x => Equals(x.Color, value));
                if (themeColor is null)
                {
                    return;
                }

                themeColor.IsSelected = true;
            }
        }
        
        public Color? SelectedRecentColor
        {
            get => _map.RecentColorsListBox?.SelectedItem.Find<PredefinedColorItem>()?.Color;
            set
            {
                var recentColor = RecentColors.FirstOrDefault(x => Equals(x.Color, value));
                if (recentColor is null)
                {
                    return;
                }

                recentColor.IsSelected = true;
            }
        }

        public List<string> AvailableColorNames
        {
            get => _map.ColorComboBox.GetItemsOfType<PredefinedColorItem>().Select(x => x?.Text).ToList();
        }

        public IReadOnlyList<PredefinedColorItem> ThemeColors
        {
            get => _map.ThemeColorsListBox.GetItemsOfType<PredefinedColorItem>();
        }

        public IReadOnlyList<PredefinedColorItem> RecentColors
        {
            get => _map.RecentColorsListBox.GetItemsOfType<PredefinedColorItem>();
        }

        public bool Apply()
        {
            return _map.SelectButton.TryInvoke();
        }

        public bool Cancel()
        {
            return _map.CancelButton.TryInvoke();
        }

        public void SetHsvColor(Color color)
        {
            _map.HsvCanvas.SetColor(color);
            Wait.UntilResponsive();

            var h = ColorHelper.GetHSV_H(color);
            _map.HSVSlider.Value = h;
            Wait.UntilResponsive();

            //Should set Alpha channel after, because
            //HSV has nothing to do with alpha
            _map.ASlider.Value = color.A;
        }

        private void SetARgbColor(Color color)
        {
            _map.ASlider.Value = color.A;
            Wait.UntilResponsive();

            _map.RSlider.Value = color.R;
            Wait.UntilResponsive();

            _map.GSlider.Value = color.G;
            Wait.UntilResponsive();

            _map.BSlider.Value = color.B;
            Wait.UntilResponsive();
        }

        private void SetARgbColorAlt(Color color)
        {
            _map.AEdit.Text = color.A.ToString("X");
            Wait.UntilResponsive();

            _map.REdit.Text = color.R.ToString("X");
            Wait.UntilResponsive();

            _map.GEdit.Text = color.G.ToString("X");
            Wait.UntilResponsive();

            _map.BEdit.Text = color.B.ToString("X");
            Wait.UntilResponsive();

            //We should move focus to apply changes, for example on 1st element
            _map.AEdit.SetFocus();
        }

        private byte EditToByte(Edit edit)
        {
            return byte.Parse(edit.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        private string GetColorName(AutomationElement listItem)
        {
            return listItem?.GetChild(0).As<PredefinedColorItem>()?.Text;
        }
    }
}
