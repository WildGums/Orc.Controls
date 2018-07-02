namespace Orc.Controls.Views
{
    using System.Windows;
    using Catel.MVVM.Views;
    using Orc.Controls.Models;

    /// <summary>
    /// Interaction logic for LogFilterEditorControl.xaml
    /// </summary>
    public partial class LogFilterEditorControl
    {
        public static readonly DependencyProperty LogFilterProperty = DependencyProperty.Register(
            "LogFilter", typeof(LogFilter), typeof(LogFilterEditorControl), new PropertyMetadata(default(LogFilter)));

        public LogFilterEditorControl()
        {
            InitializeComponent();
            // DataContextChanged += OnDataContextChanged;
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public LogFilter LogFilter
        {
            get => (LogFilter)GetValue(LogFilterProperty);
            set => SetValue(LogFilterProperty, value);
        }

        /*
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LogFilter = (LogFilter)e.NewValue;
        }
        */
    }
}
