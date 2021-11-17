// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Automation.Host
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
            var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Starting application");
            Log.Info("This log message should show up as debug");

            base.OnStartup(e);
        }
    }
}
