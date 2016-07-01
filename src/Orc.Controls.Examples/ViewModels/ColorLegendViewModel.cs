// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorLegendViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.Collections;
    using Catel.MVVM;

    public class ColorLegendViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorLegendViewModel"/> class.
        /// </summary>
        public ColorLegendViewModel()
        {
            CalendarStateLegend = new ObservableCollection<IColorLegendItem>();
        }

        /// <summary>
        /// Gets or sets the calendar state legend.
        /// </summary>
        public ObservableCollection<IColorLegendItem> CalendarStateLegend { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            UpdateCalenderStateLegend();
        }

        private void UpdateCalenderStateLegend()
        {
            var colors = new List<Color>();
            colors.Add(Colors.Blue);
            colors.Add(Colors.Red);
            colors.Add(Colors.Green);
            colors.Add(Colors.Orange);
            colors.Add(Colors.Gray);

            var items = new List<IColorLegendItem>();

            foreach (var color in colors)
            {
                items.Add(new ColorLegendItem
                {
                    Color = color,
                    Description = color.ToString(),
                    IsChecked = true,
                    Id = color.ToString(),
                    AdditionalData = "(1)"
                });
            }

            ((ICollection<IColorLegendItem>) CalendarStateLegend).ReplaceRange(items);
        }
    }
}