namespace Orc.Controls.Example
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Orc.Theming;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public App()
        {
            //LogManager.AddDebugListener(false);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var languageService = ServiceLocator.Default.ResolveRequiredType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            // Some test logging, but important to load the assembly first
            var externalTypeToForceAssemblyLoad = typeof(LogViewerLogListener);

            Orc.Theming.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Controls.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Starting application");
            Log.Info("This log message should show up as debug");

            base.OnStartup(e);
        }
    }
}
