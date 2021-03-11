namespace Orc.Controls
{
    using System.Windows;

    public partial class WizardPageHeader
    {
        public WizardPageHeader()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(WizardPageHeader), new PropertyMetadata(null));


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(WizardPageHeader), new PropertyMetadata(null));


        public TextAlignment TextAlignment
        {
            get { return (TextAlignment )GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment ), typeof(WizardPageHeader), new PropertyMetadata(TextAlignment.Left));
    }
}
