namespace Orc.Controls
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Automation;
    using Catel;
    using Catel.Logging;
    using Catel.Windows.Data;
    using Image = System.Windows.Controls.Image;

    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class FontImage : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Theming.FontImage _fontImage = new();

        private Image _image;
        
        #region Dependency properties
        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            nameof(ItemName), typeof(string), typeof(FontImage), new PropertyMetadata(default(string),
                (sender, _) => ((FontImage)sender).OnItemNameChanged()));

        private void OnItemNameChanged()
        {
            Update();
        }
        #endregion

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
                if (!IsFontInstalled(fontName))
                {
                    return null;
                }

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
                Log.Warning($"Can't get image source for Item = '{itemName}', Font='{fontName}'");

                return null;
            }
        }

        private static bool IsFontInstalled(string fontName)
        {
            if (string.IsNullOrWhiteSpace(fontName))
            {
                return false;
            }

            using var testFont = new Font(fontName, 8);
            return fontName.EqualsIgnoreCase(testFont.Name);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FontImageAutomationPeer(this);
        }
    }
}
