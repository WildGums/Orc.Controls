namespace Orc.Controls.Example;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Catel;
using Catel.Configuration;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orc.Automation;
using Orc.Controls.Example.Services;
using Orc.Controls.Example.Views;
using Orc.Controls.Example.Watchers;
using Orc.FileSystem;
using Orc.Serialization.Json;
using Orc.SystemInfo;
using Orc.Theming;
using Orchestra;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{

#pragma warning disable IDISP006 // Implement IDisposable
    private readonly IHost _host;
#pragma warning restore IDISP006 // Implement IDisposable

    public App()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(x =>
                {
                    x.SetMinimumLevel(LogLevel.Debug);

                    x.AddInMemory();

                    x.AddConsole();
                    x.AddDebug();
                });

                services.AddCatelCore();
                services.AddCatelMvvm();
                services.AddOrcAutomation();
                services.AddOrcControls();
                services.AddOrcFileSystem();
                services.AddOrcSerializationJson();
                services.AddOrcSystemInfo();
                services.AddOrcTheming();
                services.AddOrchestraCore();

                services.AddSingleton<IAutoCompletionService, ReverseAutoCompletionService>();

                services.AddSingleton<ShowCalloutAtStartupWatcher>();
            });

        _host = hostBuilder.Build();

        IoCContainer.ServiceProvider = _host.Services;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceProvider = IoCContainer.ServiceProvider;

        serviceProvider.CreateTypesThatMustBeConstructedAtStartup();

        var languageService = serviceProvider.GetRequiredService<ILanguageService>();

        // Note: it's best to use .CurrentUICulture in actual apps since it will use the preferred language
        // of the user. But in order to demo multilingual features for devs (who mostly have en-US as .CurrentUICulture),
        // we use .CurrentCulture for the sake of the demo
        languageService.PreferredCulture = CultureInfo.CurrentCulture;
        languageService.FallbackCulture = new CultureInfo("en-US");

        FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orc.Controls.Example;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome")); 
        Orc.Theming.FontImage.DefaultFontFamily = "FontAwesome";

        var configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
        await configurationService.LoadAsync();

        this.ApplyTheme();

        var mainWindow = ActivatorUtilities.CreateInstance<MainWindow>(_host.Services);
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}
