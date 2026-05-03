#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Media;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.ColorBoard))]
public class ColorBoard : FrameworkElement<ColorBoardModel, ColorBoardMap>
{
    public ColorBoard(AutomationElement element)
        : base(element)
    {
    }

    /// <summary>
    /// Set colors using sliders
    /// </summary>
    public Color ArgbColor
    {
        get => Color.FromArgb((byte)Map.ASlider.Value, (byte)Map.RSlider.Value, (byte)Map.GSlider.Value, (byte)Map.BSlider.Value);
        set => SetARgbColor(value);
    }

    /// <summary>
    /// Set colors using editors
    /// </summary>
    public Color ArgbColorAlt
    {
        get => Color.FromArgb(EditToByte(Map.AEdit), EditToByte(Map.REdit), EditToByte(Map.GEdit), EditToByte(Map.BEdit));
        set => SetARgbColorAlt(value);
    }

    public Color HsvColor
    {
        get
        {
            var sv = Map.HsvCanvas.GetSV();
            var h = Map.HSVSlider.Value;

            var hsvColor = ColorHelper.HSV2RGB(h, sv.X, sv.Y);

            //Alpha channel we should take from other place
            return Color.FromArgb(EditToByte(Map.AEdit), hsvColor.R, hsvColor.G, hsvColor.B);
        }
        set => SetHsvColor(value);
    }

    public string ColorName
    {
        get
        {
            var colorEditText = Map.ColorEdit.Text;
            if (!string.IsNullOrWhiteSpace(colorEditText))
            {
                return colorEditText;
            }

            var text = PredefinedColorName;
            return text;
        }
        set
        {
            var colorEdit = Map.ColorEdit;

            colorEdit.SetFocus();

            colorEdit.Text = value;

            //We should move focus, for example on 1st element
            Map.AEdit.SetFocus();
        }
    }

    public string PredefinedColorName
    {
        get => Map.ColorComboBox.SelectedItem?.Find<PredefinedColorItem>()?.Text;
        set => Map.ColorComboBox.Select(x => Equals(GetColorName(x), value));
    }

    public Color? SelectedThemeColor
    {
        get => Map.ThemeColorsListBox?.SelectedItem?.Find<PredefinedColorItem>()?.Color;
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
        get => Map.RecentColorsListBox?.SelectedItem.Find<PredefinedColorItem>()?.Color;
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
        get => Map.ColorComboBox.GetItemsOfType<PredefinedColorItem>().Select(x => x?.Text).ToList();
    }

    public IReadOnlyList<PredefinedColorItem> ThemeColors
    {
        get => Map.ThemeColorsListBox.GetItemsOfType<PredefinedColorItem>();
    }

    public IReadOnlyList<PredefinedColorItem> RecentColors
    {
        get => Map.RecentColorsListBox.GetItemsOfType<PredefinedColorItem>();
    }

    public bool Apply()
    {
        return Map.SelectButton.Click();
    }

    public bool Cancel()
    {
        return Map.CancelButton.Click();
    }

    public void SetHsvColor(Color color)
    {
        Map.HsvCanvas.SetColor(color);
        Wait.UntilResponsive();

        var h = ColorHelper.GetHSV_H(color);
        Map.HSVSlider.Value = h;
        Wait.UntilResponsive();

        //Should set Alpha channel after, because
        //HSV has nothing to do with alpha
        Map.ASlider.Value = color.A;
    }

    private void SetARgbColor(Color color)
    {
        Map.ASlider.Value = color.A;
        Wait.UntilResponsive();

        Map.RSlider.Value = color.R;
        Wait.UntilResponsive();

        Map.GSlider.Value = color.G;
        Wait.UntilResponsive();

        Map.BSlider.Value = color.B;
        Wait.UntilResponsive();
    }

    private void SetARgbColorAlt(Color color)
    {
        Map.AEdit.Text = color.A.ToString("X");
        Wait.UntilResponsive();

        Map.REdit.Text = color.R.ToString("X");
        Wait.UntilResponsive();

        Map.GEdit.Text = color.G.ToString("X");
        Wait.UntilResponsive();

        Map.BEdit.Text = color.B.ToString("X");
        Wait.UntilResponsive();

        //We should move focus to apply changes, for example on 1st element
        Map.AEdit.SetFocus();
    }

    private byte EditToByte(Edit edit)
    {
        return byte.Parse(edit.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    }

    private string GetColorName(AutomationElement listItem)
    {
        return listItem?.GetChild(0).As<PredefinedColorItem>()?.Text;
    }

#pragma warning disable CS0067
    public event EventHandler<EventArgs> DoneClicked;
    public event EventHandler<EventArgs> CancelClicked;
#pragma warning restore CS0067
}
#nullable enable
