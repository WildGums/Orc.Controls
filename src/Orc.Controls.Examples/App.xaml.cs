// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples
{
    using System.Globalization;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Windows;
    using Logging;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            // Some test logging, but important to load the assembly first
            var externalTypeToForceAssemblyLoad = typeof (LogViewerLogListener);

            Log.Info("Starting application");
            Log.Info("This log message should show up as debug");

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            base.OnStartup(e);
        }
    }
}