namespace Orc.Controls.Tools;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Catel.IO;
using Catel.Logging;
using Catel.Services;
using FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Path = System.IO.Path;

public class ControlToolManager : IControlToolManager
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ControlToolManager));

    private readonly FrameworkElement _frameworkElement;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDirectoryService _directoryService;
    private readonly IAppDataService _appDataService;

    public ControlToolManager(FrameworkElement frameworkElement, IServiceProvider serviceProvider, 
        IDirectoryService directoryService, IAppDataService appDataService)
    {
        _frameworkElement = frameworkElement;
        _serviceProvider = serviceProvider;
        _directoryService = directoryService;
        _appDataService = appDataService;
    }

    public IList<IControlTool> Tools { get; } = new List<IControlTool>();

    public event EventHandler<ToolManagementEventArgs>? ToolAttached;
    public event EventHandler<ToolManagementEventArgs>? ToolDetached;
    public event EventHandler<ToolManagementEventArgs>? ToolOpening;
    public event EventHandler<ToolManagementEventArgs>? ToolOpened;
    public event EventHandler<ToolManagementEventArgs>? ToolClosed;

    public bool CanAttachTool(Type toolType)
    {
        var existingTool = Tools.FirstOrDefault(x => x.GetType() == toolType);
        return existingTool is null || !existingTool.IsAttached;
    }

    public async Task<object?> AttachToolAsync(Type toolType)
    {
        ArgumentNullException.ThrowIfNull(toolType);

        var tools = Tools;

        var tool = tools.FirstOrDefault(x => x.GetType() == toolType);
        if (tool is null)
        {
            tool = ActivatorUtilities.CreateInstance(_serviceProvider, toolType) as IControlTool;
            if (tool is not null)
            {
                tools.Add(tool);

                tool.Opening += OnToolOpening;
                tool.Opened += OnToolOpened;
                tool.Closed += OnToolClosed;
            }
        }

        if (!tools.Any())
        {
            _frameworkElement.Unloaded += OnFrameworkElementUnloaded;
        }

        if (tool is null)
        {
            return null;
        }

        if (!tool.IsAttached)
        {
            tool.Attach(_frameworkElement);

            ToolAttached?.Invoke(this, new ToolManagementEventArgs(tool));
        }

        return tool;
    }

    public async Task<bool> DetachToolAsync(Type toolType)
    {
        var tools = Tools;
        var tool = tools.FirstOrDefault(x => x.GetType() == toolType);
        if (tool is null)
        {
            return false;
        }

        tool.Opening -= OnToolOpening;
        tool.Opened -= OnToolOpened;
        tool.Closed -= OnToolClosed;

        await tool.CloseAsync();
        tool.Detach();

        tools.Remove(tool);

        ToolDetached?.Invoke(this, new ToolManagementEventArgs(tool));

        return true;
    }

    private void OnToolClosed(object? sender, EventArgs e)
    {
        if (sender is not IControlTool tool)
        {
            return;
        }

        SaveSettings(tool);
        ToolClosed?.Invoke(this, new ToolManagementEventArgs(tool));
    }

    private void OnToolOpening(object? sender, EventArgs e)
    {
        if (sender is not IControlTool tool)
        {
            return;
        }

        LoadSettings(tool);
        ToolOpening?.Invoke(this, new ToolManagementEventArgs(tool));
    }

    private void OnToolOpened(object? sender, EventArgs e)
    {
        if (sender is not IControlTool controlTool)
        {
            return;
        }

        ToolOpened?.Invoke(this, new ToolManagementEventArgs(controlTool));
    }

    protected virtual void LoadSettings(IControlTool tool)
    {
        var settingsProperties = tool.GetType()
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(ToolSettingsAttribute)));

        foreach (var settingsProperty in settingsProperties)
        {
            var settingsFilePath = GetSettingsFilePath(tool, settingsProperty);
            if (!File.Exists(settingsFilePath))
            {
                continue;
            }

            //var serializer = SerializationFactory.GetXmlSerializer();
            //using var fileStream = File.Open(settingsFilePath, FileMode.Open);

            //try
            //{
            //    var settings = serializer.Deserialize(settingsProperty.PropertyType, fileStream);
            //    settingsProperty.SetValue(tool, settings);
            //}
            //catch (Exception ex)
            //{
            //    //Vladimir:Don't crash if something went wrong while loading file
            //    Logger.LogDebug(ex, "Failed to load settings");
            //}
        }
    }

    protected virtual void SaveSettings(IControlTool tool)
    {
        var settingsProperties = tool.GetType()
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(ToolSettingsAttribute)));

        foreach (var settingsProperty in settingsProperties)
        {
            var settings = settingsProperty.GetValue(tool);
            if (settings is null)
            {
                continue;
            }

            //var serializer = SerializationFactory.GetXmlSerializer();
            //var settingsFilePath = GetSettingsFilePath(tool, settingsProperty);
            //using var fileStream = File.Open(settingsFilePath, FileMode.Create);

            //try
            //{
            //    serializer.Serialize(settings, fileStream);   
            //}
            //catch (Exception ex)
            //{
            //    //Vladimir:Don't crash if something went wrong while saving tool settings into file
            //    Logger.LogDebug(ex);
            //}
        }
    }

    private string GetSettingsFilePath(IControlTool tool, PropertyInfo settingsProperty)
    {
        var toolSettingsAttribute = Attribute.GetCustomAttribute(settingsProperty, typeof(ToolSettingsAttribute)) as ToolSettingsAttribute;
        var settingsStorage = toolSettingsAttribute?.Storage;
        var appDataDirectory = _appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var fileName = settingsProperty.Name + ".xml";

        if (string.IsNullOrWhiteSpace(settingsStorage))
        {
            settingsStorage = Path.Combine(appDataDirectory, tool.Name);
        }
        else
        {
            var companyDirectory = Catel.IO.Path.GetParentDirectory(appDataDirectory);

            settingsStorage = Regex.Replace(settingsStorage, "%AppData%", appDataDirectory, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
            settingsStorage = Regex.Replace(settingsStorage, "%Company%", companyDirectory, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
            settingsStorage = Regex.Replace(settingsStorage, "%Name%", tool.Name, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
        }

        _directoryService.Create(settingsStorage);
        return Path.Combine(settingsStorage, fileName);
    }

    private async void OnFrameworkElementUnloaded(object? sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement frameworkElement)
        {
            return;
        }

        foreach (var tool in Tools)
        {
            tool.Opening -= OnToolOpening;
            tool.Opened -= OnToolClosed;
            tool.Closed -= OnToolClosed;

            await tool.CloseAsync();
            tool.Detach();
        }

        frameworkElement.Unloaded -= OnFrameworkElementUnloaded;
    }
}
