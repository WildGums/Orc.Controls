namespace Orc.Controls.Avalonia.Views
{
    using System.Globalization;
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Markup.Xaml;

    public partial class CulturePicker : UserControl
    {
        public CulturePicker()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        #region Dependency properties

        public static readonly StyledProperty<CultureInfo> SelectedCultureProperty = AvaloniaProperty.Register<CulturePicker, CultureInfo>(nameof(SelectedCulture));

        public CultureInfo SelectedCulture
        {
            get { return (CultureInfo)GetValue(SelectedCultureProperty); }
            set
            {
                using (SetValue(SelectedCultureProperty, value))
                {
                }
            }
        }
        #endregion

        //protected override AutomationPeer OnCreateAutomationPeer()
        //{
        //    return new CulturePickerAutomationPeer(this);
        //}
    }
}
