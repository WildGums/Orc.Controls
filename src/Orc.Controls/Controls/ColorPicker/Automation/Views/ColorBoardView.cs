namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ColorBoardView : AutomationBase
    {
        private const int HSVTabIndex = 0;
        private const int ColorTabIndex = 1;

        private readonly ColorBoard _target;
        private HsvCanvasColorBoardPart _hsvCanvas;

        public ColorBoardView(ColorBoard target)
            : base(target.Element)
        {
            _target = target;
        }

        protected Tab Tab => By.One<Tab>();

        //HSV Tab item elements
        protected Slider HSVSlider => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Slider>());
        protected Slider ASlider => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Slider>());
        protected Slider RSlider => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Slider>());
        protected Slider GSlider => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Slider>());
        protected Slider BSlider => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Slider>());
        protected Edit AEdit => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Edit>());
        protected Edit REdit => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Edit>());
        protected Edit GEdit => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Edit>());
        protected Edit BEdit => Tab.TabScope(HSVTabIndex).Execute(() => By.Id().One<Edit>());
        protected HsvCanvasColorBoardPart HsvCanvas => Tab.TabScope(HSVTabIndex).Execute(() => _hsvCanvas ??= new (_target));


        //Out of tab elements
        protected Edit ColorEdit => By.Id().One<Edit>();
        protected ComboBox ColorComboBox => By.Id().One<ComboBox>();

        //Apply/Cancel buttons
        protected Button SelectButton => By.Id().One<Button>();
        protected Button CancelButton => By.Id().One<Button>();

        /// <summary>
        /// Set colors using sliders
        /// </summary>
        public Color ArgbColor
        {
            get => Color.FromArgb((byte)ASlider.Value, (byte)RSlider.Value, (byte)GSlider.Value, (byte)BSlider.Value);
            set => SetARgbColor(value);
        }

        /// <summary>
        /// Set colors using editors
        /// </summary>
        public Color ArgbColorAlt
        {
            get => Color.FromArgb(EditToByte(AEdit), EditToByte(REdit), EditToByte(GEdit), EditToByte(BEdit));
            set => SetARgbColorAlt(value);
        }

        public Color HsvColor
        {
            get
            {
                var sv = HsvCanvas.GetSV();
                var h = HSVSlider.Value;

                var hsvColor = ColorHelper.HSV2RGB(h, sv.X, sv.Y);

                //Alpha channel we should take from other place
                return Color.FromArgb(EditToByte(AEdit), hsvColor.R, hsvColor.G, hsvColor.B);
            }
            set => SetHsvColor(value);
        }

        public string ColorName
        {
            get
            {
                var colorEditText = ColorEdit.Text;
                if (!string.IsNullOrWhiteSpace(colorEditText))
                {
                    return colorEditText;
                }

                var text = PredefinedColorName;
                return text;
            }
            set
            {
                var colorEdit = ColorEdit;

                colorEdit.SetFocus();

                colorEdit.Text = value;

                //We should move focus, for example on 1st element
                AEdit.SetFocus();
            }
        }

        public string PredefinedColorName
        {
            get => ColorComboBox.InvokeInExpandState(() => ColorComboBox.SelectedItem?.TryGetDisplayText());
            set
            {
                var colorComboBox = ColorComboBox;
                colorComboBox.InvokeInExpandState(() =>
                {
                    var itemToSelect = colorComboBox.Items?.FirstOrDefault(x => Equals(x.TryGetDisplayText(), value));
                    itemToSelect?.Select();
                });
            }
        }

        public List<string> AvailableColorNames
        {
            get => ColorComboBox.Items.Select(x => x.TryGetDisplayText()).ToList();
        }

        public bool Apply()
        {
            return SelectButton.TryInvoke();
        }

        public bool Cancel()
        {
            return CancelButton.TryInvoke();
        }

        public void SetHsvColor(Color color)
        {
            using (Tab.TabScope(HSVTabIndex))
            {
                HsvCanvas.SetColor(color);
                Wait.UntilResponsive();

                var h = ColorHelper.GetHSV_H(color);
                HSVSlider.Value = h;
                Wait.UntilResponsive();

                //Should set Alpha channel after, because
                //HSV has nothing to do with alpha
                ASlider.Value = color.A;
            }
        }

        private void SetARgbColor(Color color)
        {
            using (Tab.TabScope(HSVTabIndex))
            {
                ASlider.Value = color.A;
                Wait.UntilResponsive();

                RSlider.Value = color.R;
                Wait.UntilResponsive();

                GSlider.Value = color.G;
                Wait.UntilResponsive();

                BSlider.Value = color.B;
                Wait.UntilResponsive();
            };
        }

        private void SetARgbColorAlt(Color color)
        {
            using (Tab.TabScope(HSVTabIndex))
            {
                AEdit.Text = color.A.ToString("X");
                Wait.UntilResponsive();

                REdit.Text = color.R.ToString("X");
                Wait.UntilResponsive();

                GEdit.Text = color.G.ToString("X");
                Wait.UntilResponsive();

                BEdit.Text = color.B.ToString("X");
                Wait.UntilResponsive();

                //We should move focus to apply changes, for example on 1st element
                AEdit.SetFocus();
            };
        }

        private byte EditToByte(Edit edit)
        {
            return byte.Parse(edit.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
    }
}
