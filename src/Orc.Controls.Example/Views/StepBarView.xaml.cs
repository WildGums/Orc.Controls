namespace Orc.Controls.Example.Views
{
    using System.Linq;
    using System.Windows;
    using System;

    public partial class StepBarView
    {
        public StepBarView()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Handle tab torn off event
        /// </summary>
        private void OnTabTornOff(object sender, TearOffEventArgs e)
        {
            Console.WriteLine($"Tab '{e.TabItem.Header}' was torn off");

            // You can perform custom logic here:
            // - Log the action
            // - Update UI state
            // - Notify other parts of the application
            // - Save window layout state

            // Example: Update status bar
            // StatusBar.Text = $"Tab '{e.TabItem.Header}' is now in a separate window";
        }

        /// <summary>
        /// Handle tab docked back event
        /// </summary>
        private void OnTabDocked(object sender, TearOffEventArgs e)
        {
            Console.WriteLine($"Tab '{e.TabItem.Header}' was docked back");

            // Custom logic for when tab is docked back:
            // - Restore focus
            // - Update layout
            // - Save state

            // Example: Focus the docked tab
            e.TabItem.Focus();

            // StatusBar.Text = $"Tab '{e.TabItem.Header}' has been docked back";
        }

        /// <summary>
        /// Button handler to dock all torn-off tabs back
        /// </summary>
        private void DockAllTabs_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.DockAllTabs();
            MessageBox.Show("All tabs have been docked back to the main window.");
        }

        /// <summary>
        /// Button handler to toggle tear-off functionality
        /// </summary>
        private void ToggleTearOff_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SetCurrentValue(Controls.TabControl.AllowTearOffProperty, !MainTabControl.AllowTearOff);

            var status = MainTabControl.AllowTearOff ? "enabled" : "disabled";
            MessageBox.Show($"Tear-off functionality is now {status}.");

            // Update individual tab items
            foreach (var item in MainTabControl.Items.OfType<TearOffTabItem>())
            {
                item.CanTearOff = MainTabControl.AllowTearOff;
            }
        }

        /// <summary>
        /// Example of checking torn-off tabs
        /// </summary>
        private void CheckTornOffTabs()
        {
            var tornOffTabs = MainTabControl.TornOffTabs.ToList();

            if (tornOffTabs.Any())
            {
                Console.WriteLine($"Currently {tornOffTabs.Count} tab(s) are torn off:");
                foreach (var tab in tornOffTabs)
                {
                    Console.WriteLine($"- {tab.Header}");
                }
            }
            else
            {
                Console.WriteLine("No tabs are currently torn off.");
            }
        }

        /// <summary>
        /// Example of programmatically tearing off a specific tab
        /// </summary>
        private void TearOffSpecificTab(string tabHeader)
        {
            var tab = MainTabControl.Items.OfType<TearOffTabItem>()
                .FirstOrDefault(t => t.Header?.ToString() == tabHeader);

            if (tab is not null && tab.CanTearOff && !tab.IsTornOff)
            {
                // You would need to add a public method to TearOffTabItem for this
                // or simulate the drag operation
                Console.WriteLine($"Would tear off tab: {tabHeader}");
            }
        }

        /// <summary>
        /// Example of saving and restoring tab layout state
        /// </summary>
        private void SaveTabLayout()
        {
            // This is a simplified example - you'd want to use a proper serialization method
            var tabStates = MainTabControl.Items.OfType<TearOffTabItem>()
                .Select(tab => new
                {
                    Header = tab.Header?.ToString(),
                    IsTornOff = tab.IsTornOff,
                    CanTearOff = tab.CanTearOff
                }).ToList();

            // Save tabStates to settings/file
            Console.WriteLine("Tab layout saved");
        }

        private void RestoreTabLayout()
        {
            // Load and restore tab states
            Console.WriteLine("Tab layout restored");
        }
    }
}
