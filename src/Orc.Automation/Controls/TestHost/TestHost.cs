namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using Catel.Logging;
    using Theming;

    [TemplatePart(Name = "PART_HostGrid", Type = typeof(Grid))]
    public class TestHost : Control
    {
        [DllImport("kernel32.dll")]
        private static extern bool LoadLibraryA(string hModule);

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly HashSet<string> _loadedAssemblyNames = new ();

        private Grid _hostGrid;

        public override void OnApplyTemplate()
        {
            _hostGrid = GetTemplateChild("PART_HostGrid") as Grid;
            if (_hostGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HostGrid'");
            }
        }

        public string PutControl(string fullName)
        {
            var controlType = TypeHelper.GetTypeByName(fullName);
            if (controlType is null)
            {
                return $"Error: Can't find control Type {fullName}";
            }

            if (Activator.CreateInstance(controlType) is not FrameworkElement control)
            {
                return $"Error: Can't instantiate control Type {controlType}";
            }

            var newAutomationId = Guid.NewGuid().ToString();

            control.SetCurrentValue(AutomationProperties.AutomationIdProperty, newAutomationId);

            _hostGrid.Children.Add(control);

            return newAutomationId;
        }

        public bool LoadResources(string resourcesPath)
        {
            try
            {
                var uri = new Uri(resourcesPath, UriKind.Absolute);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            try
            {
                StyleHelper.CreateStyleForwardersForDefaultStyles();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }

        public void LoadUnmanaged(string path)
        {
            LoadLibraryA(path);
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            try
            {
                var assembly = Assembly.LoadFile(assemblyPath);

                LoadAssembly(assembly.GetName(), Path.GetDirectoryName(assemblyPath));

                return assembly;
            }
            catch (Exception ex)
            {
                Console.Write(ex);

                return null;
            }
        }

        private void LoadAssembly(AssemblyName assemblyName, string rootDirectory = null)
        {
            if (_loadedAssemblyNames.Contains(assemblyName.FullName))
            {
                return;
            }

            var loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);

            var referencedAssemblies = loadedAssembly.GetReferencedAssemblies();
            foreach (var referencedAssembly in referencedAssemblies)
            {
                try
                {
                    LoadAssembly(referencedAssembly, rootDirectory);
                }
                catch
                {
                    //TODO
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(rootDirectory))
                        {
                            LoadAssembly(Path.Combine(rootDirectory, referencedAssembly.Name + ".dll"));
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);

                        return;
                    }
                }

                _loadedAssemblyNames.Add(assemblyName.FullName);
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TestHostAutomationPeer(this);
        }
    }
}
