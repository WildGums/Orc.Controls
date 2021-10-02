namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.Windows.Data;

    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class FontImage : Control
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private Image _image;

        private readonly Theming.FontImage _fontImage = new ();
        #endregion

        #region Dependency properties
        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            nameof(ItemName), typeof(string), typeof(FontImage), new PropertyMetadata(default(string),
                (sender, args) => ((FontImage)sender).OnItemNameChanged(args)));

        private void OnItemNameChanged(DependencyPropertyChangedEventArgs args)
        {
            Update();
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            _image = GetTemplateChild("PART_Image") as Image;
            if (_image is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Image'");
            }
            
            Update();

            this.SubscribeToDependencyProperty(nameof(FontFamily), (_, _) => Update());
            this.SubscribeToDependencyProperty(nameof(Foreground), (_, _) => Update());
        }

        private void Update()
        {
            _image?.SetCurrentValue(Image.SourceProperty, GenerateImageSource());
        }

        private ImageSource GenerateImageSource()
        {
            var itemName = ItemName;
            if (string.IsNullOrEmpty(itemName))
            {
                return null;
            }

            var fontFamily = FontFamily;
            if (fontFamily is null)
            {
                return null;
            }
            
            var fontName = fontFamily.ToString();
            var registeredFont = Theming.FontImage.GetRegisteredFont(fontName);
            if (registeredFont is null)
            {
                Theming.FontImage.RegisterFont(fontName, fontFamily);
            }

            _fontImage.ItemName = itemName;
            _fontImage.FontFamily = fontName;
            _fontImage.Brush = Foreground;

            try
            {
                return _fontImage.GetImageSource();
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
