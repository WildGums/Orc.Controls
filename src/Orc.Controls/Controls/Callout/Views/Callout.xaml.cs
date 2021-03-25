using System.Windows;
using System.Windows.Markup;

namespace Orc.Controls
{
    /// <summary>
    /// Interaction logic for Callout.xaml
    /// </summary>
    [ContentProperty(nameof(InnerContent))]
    public partial class Callout
    {
        public Callout()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register(nameof(InnerContent), typeof(object), typeof(Callout));

        public object InnerContent
        {
            get { return (object)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }
    }
}
