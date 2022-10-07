namespace Orc.Controls.Automation;

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

    private HsvCanvasColorBoardPart? _hsvCanvas;
    private Tab? _tab;

    public ColorBoardMap(ColorBoard target)
        : base(target.Element)
    {
        _target = target;
    }

    public override By By => new(Element, Tab);

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
