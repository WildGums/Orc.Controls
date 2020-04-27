// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
            //var sqLiteProviderInfo = new DbProviderInfo
            //{
            //    Name = "SQLite Data Provider",
            //    InvariantName = "System.Data.SQLite",
            //    Description = ".NET Framework Data Provider for SQLite",
            //    AssemblyQualifiedName = "System.Data.SQLite.SQLiteFactory, System.Data.SQLite, Version=1.0.110.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139"
            //};
            //DbProvider.RegisterProvider(sqLiteProviderInfo);

            //var oracleProviderInfo = new DbProviderInfo
            //{
            //    Name = "ODP.NET, Managed Driver",
            //    InvariantName = "Oracle.ManagedDataAccess.Client",
            //    Description = "Oracle Data Provider for .NET, Managed Driver",
            //    AssemblyQualifiedName = "Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Version=4.122.18.3, Culture=neutral, PublicKeyToken=89b483f429c47342"
            //};
            //DbProvider.RegisterProvider(oracleProviderInfo);

            var languageService = ServiceLocator.Default.ResolveType<ILanguageService>();

            // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
            // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
            // we use .CurrentCulture for the sake of the demo
            languageService.PreferredCulture = CultureInfo.CurrentCulture;
            languageService.FallbackCulture = new CultureInfo("en-US");

            // Some test logging, but important to load the assembly first
            var externalTypeToForceAssemblyLoad = typeof(LogViewerLogListener);

            Orc.Controls.FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Controls.Example.NET;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));
            Orc.Controls.FontImage.DefaultFontFamily = "FontAwesome";

            Log.Info("Starting application");
            Log.Info("This log message should show up as debug");

            base.OnStartup(e);
        }
        #endregion
    }
}
