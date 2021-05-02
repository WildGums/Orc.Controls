namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using Catel.MVVM.Views;

    [ContentProperty(nameof(InnerContent))]
    public partial class Callout
    {
        static Callout()
        {
            typeof(Callout).AutoDetectViewPropertiesToSubscribe();
        }

        public Callout()
        {
            InitializeComponent();
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object InnerContent
        {
            get { return GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register(nameof(InnerContent), typeof(object), typeof(Callout));


        [ViewToViewModel(viewModelPropertyName: nameof(CalloutViewModel.Name), MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string CalloutName
        {
            get { return (string)GetValue(CalloutNameProperty); }
            set { SetValue(CalloutNameProperty, value); }
        }

        public static readonly DependencyProperty CalloutNameProperty = DependencyProperty.Register(nameof(CalloutName),
            typeof(string), typeof(Callout), new PropertyMetadata(null));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object PlacementTarget
        {
            get { return GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }

        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.Register(nameof(PlacementTarget), typeof(object), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register(nameof(Delay), typeof(int), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(nameof(IsClosable), 
            typeof(bool), typeof(Callout), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan ShowTime
        {
            get { return (TimeSpan)GetValue(ShowTimeProperty); }
            set { SetValue(ShowTimeProperty, value); }
        }

        public static readonly DependencyProperty ShowTimeProperty = DependencyProperty.Register(nameof(ShowTime),
            typeof(TimeSpan), typeof(Callout), new PropertyMetadata(TimeSpan.FromSeconds(30)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), 
            typeof(ICommand), typeof(Callout), new PropertyMetadata(null));
    }
}
