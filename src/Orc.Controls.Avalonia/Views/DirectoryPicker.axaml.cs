namespace Orc.Controls.Avalonia.Views
{
    using Catel.MVVM.Views;
    using global::Avalonia.Controls;
    using global::Avalonia.Markup.Xaml;
    using global::Avalonia;

    /// <summary>
    ///     Interaction logic for DirectoryPicker.xaml
    /// </summary>
    public partial class DirectoryPicker : UserControl
    {
        #region Constructors
        static DirectoryPicker()
        {
            
            typeof(DirectoryPicker).AutoDetectViewPropertiesToSubscribe();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPicker"/> class.
        /// </summary>
        /// <remarks>This method is required for design time support.</remarks>
        public DirectoryPicker()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set
            {
                using (SetValue(LabelWidthProperty, value = 125d))
                {
                }
            }
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...

        public static readonly StyledProperty<double> LabelWidthProperty = AvaloniaProperty.Register<DirectoryPicker, double>(nameof(LabelWidth));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set
            {
                using (SetValue(LabelTextProperty, value))
                {
                }
            }
        }

        // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...

        public static readonly StyledProperty<string> LabelTextProperty = AvaloniaProperty.Register<DirectoryPicker, string>(nameof(LabelText));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedDirectory
        {
            get { return (string)GetValue(SelectedDirectoryProperty); }
            set
            {
                using (SetValue(SelectedDirectoryProperty, value))
                {
                }
            }
        }

        // Using a DependencyProperty as the backing store for SelectedDirectory.  This enables animation, styling, binding, etc...
        public static readonly StyledProperty<string> SelectedDirectoryProperty = AvaloniaProperty.Register<DirectoryPicker, string>(nameof(SelectedDirectory));
        #endregion

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
