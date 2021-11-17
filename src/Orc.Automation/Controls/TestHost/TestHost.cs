namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Resources;
    using Automation;
    using Catel.Logging;
    using Catel.Reflection;
    using Theming;

    [TemplatePart(Name = "PART_HostGrid", Type = typeof(Grid))]
    public class TestHost : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string TestedControlAutomationId = "TestedControlId";

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
            var controlType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => string.Equals(x.FullName, fullName));

            if (controlType is null)
            {
                return $"Error: Can't find control Type {fullName}";
            }

            if (Activator.CreateInstance(controlType) is not FrameworkElement control)
            {
                return $"Error: Can't instantiate control Type {controlType}";
            }

            control.SetCurrentValue(AutomationProperties.AutomationIdProperty, TestedControlAutomationId);

            _hostGrid.Children.Add(control);

            return TestedControlAutomationId;
        }

        public bool LoadResources(string resourcesPath)
        {
            try
            {
                var uri = new Uri(resourcesPath, UriKind.Absolute);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
                StyleHelper.CreateStyleForwardersForDefaultStyles();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public Assembly LoadAssembly(string assemblyPath)
        {
            try
            {
                var assembly = Assembly.LoadFile(assemblyPath);

                LoadAssembly(assembly.GetName());

                return assembly;
            }
            catch (Exception ex)
            {
                Console.Write(ex);

                return null;
            }
        }

        private void LoadAssembly(AssemblyName assemblyName)
        {
            if (!_loadedAssemblyNames.Add(assemblyName.FullName))
            {
                return;
            }

            var loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);

            var referencedAssemblies = loadedAssembly.GetReferencedAssemblies();
            foreach (var referencedAssembly in referencedAssemblies)
            {
                try
                {
                    LoadAssembly(referencedAssembly);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TestHostAutomationPeer(this);
        }
    }
}
