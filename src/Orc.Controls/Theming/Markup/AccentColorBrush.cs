namespace Orc.Controls
{
    using System;
    using Catel.Logging;
    using Catel.Windows.Markup;

    public class AccentColorBrush : UpdatableMarkupExtension
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public AccentColorStyle AccentColorStyle { get; set; }

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return ThemeHelper.GetAccentColorBrush(AccentColorStyle);
        }
    }
}
