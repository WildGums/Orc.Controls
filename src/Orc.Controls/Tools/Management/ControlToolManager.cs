namespace Orc.Controls.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;
    using Attributes;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Runtime.Serialization;
    using FileSystem;

    public class ControlToolManager : IControlToolManager
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly FrameworkElement _frameworkElement;
        private readonly ITypeFactory _typeFactory;
        private readonly IDirectoryService _directoryService;

        public ControlToolManager(FrameworkElement frameworkElement, ITypeFactory typeFactory, IDirectoryService directoryService)
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);
            ArgumentNullException.ThrowIfNull(typeFactory);
            ArgumentNullException.ThrowIfNull(directoryService);

            _frameworkElement = frameworkElement;
            _typeFactory = typeFactory;
            _directoryService = directoryService;
        }

        public IList<IControlTool> Tools { get; } = new List<IControlTool>();

        public bool CanAttachTool(Type toolType)
        {
            if (toolType is null)
            {
                return false;
            }

            var tools = Tools;
            var existingTool = tools.FirstOrDefault(x => x.GetType() == toolType);
            return existingTool is null;
        }

        public object AttachTool(Type toolType)
        {
            ArgumentNullException.ThrowIfNull(toolType);

            var existingTool = Tools.FirstOrDefault(x => x.GetType() == toolType);
            if (existingTool is not null)
            {
                return existingTool;
            }

            if (_typeFactory.CreateInstanceWithParametersAndAutoCompletion(toolType) is not IControlTool tool)
            {
                return null;
            }

            if (!Tools.Any())
            {
                _frameworkElement.Unloaded += OnFrameworkElementUnloaded;
            }

            Tools.Add(tool);
            tool.Attach(_frameworkElement);

            tool.Opening += OnToolOpening;
            tool.Opened += OnToolOpened;
            tool.Closed += OnToolClosed;

            ToolAttached?.Invoke(this, new ToolManagementEventArgs(tool));

            return tool;
        }

        public bool DetachTool(Type toolType)
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

            tool.Close();
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
            ToolOpened?.Invoke(this, new ToolManagementEventArgs(sender as IControlTool));
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

                var serializer = SerializationFactory.GetXmlSerializer();
                using (var fileStream = File.Open(settingsFilePath, FileMode.Open))
                {
                    try
                    {
                        var settings = serializer.Deserialize(settingsProperty.PropertyType, fileStream);
                        settingsProperty.SetValue(tool, settings);
                    }
                    catch (Exception e)
                    {
                        //Vladimir:Don't crash if something went wrong while loading file
                        Log.Error(e);
                    }
                }
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

                var serializer = SerializationFactory.GetXmlSerializer();
                var settingsFilePath = GetSettingsFilePath(tool, settingsProperty);
                using (var fileStream = File.Open(settingsFilePath, FileMode.Create))
                {
                    try
                    {
                        serializer.Serialize(settings, fileStream);   
                    }
                    catch (Exception e)
                    {
                        //Vladimir:Don't crash if something went wrong while saving tool settings into file
                        Log.Error(e);
                    }
                }
            }
        }

        private string GetSettingsFilePath(IControlTool tool, PropertyInfo settingsProperty)
        {
            var toolSettingsAttribute = Attribute.GetCustomAttribute(settingsProperty, typeof(ToolSettingsAttribute)) as ToolSettingsAttribute;
            var settingsStorage = toolSettingsAttribute.Storage;
            var appDataDirectory = Catel.IO.Path.GetApplicationDataDirectory();
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

        private void OnFrameworkElementUnloaded(object? sender, RoutedEventArgs e)
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

                tool.Close();
                tool.Detach();
            }

            frameworkElement.Unloaded -= OnFrameworkElementUnloaded;
        }

        public event EventHandler<ToolManagementEventArgs>? ToolAttached;
        public event EventHandler<ToolManagementEventArgs>? ToolDetached;
        public event EventHandler<ToolManagementEventArgs>? ToolOpening;
        public event EventHandler<ToolManagementEventArgs>? ToolOpened;
        public event EventHandler<ToolManagementEventArgs>? ToolClosed;
    }
}
