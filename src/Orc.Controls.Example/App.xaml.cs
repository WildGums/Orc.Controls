namespace Orc.Controls.Example
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Orc.Controls.Example.Views;
    using Orc.Theming;
    using Orchestra;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public App()
        {
            //LogManager.AddDebugListener(false);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var languageService = ServiceLocator.Default.ResolveRequiredType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            // Some test logging, but important to load the assembly first
            var externalTypeToForceAssemblyLoad = typeof(LogViewerLogListener);

            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Controls.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            FontImage.DefaultFontFamily = "FontAwesome";

            var configurationService = ServiceLocator.Default.ResolveRequiredType<IConfigurationService>();
            await configurationService.LoadAsync();

            this.ApplyTheme();

            Log.Info("Starting application");
            Log.Info("This log message should show up as debug");

            base.OnStartup(e);
        }
    }
}
