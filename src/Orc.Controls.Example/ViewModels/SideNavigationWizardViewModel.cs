namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;

    public class SideNavigationWizardViewModel : ViewModelBase
    {
        public IList<IStepBarItem> Items { get; }

        public SideNavigationWizardViewModel()
        {
            var items = new List<IStepBarItem>()
            {
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
            };
            var num = 0;
            foreach (var page in items)
                page.Number = num++;
            Items = items;
        }
    }
}
