namespace Orc.Controls
{
    using System;
    using Catel.Logging;
    using Catel.Windows.Markup;

    public class AccentColor : UpdatableMarkupExtension
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public AccentColorStyle AccentColorStyle { get; set; }

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return ThemeHelper.GetAccentColor(AccentColorStyle);
        }
    }
}
