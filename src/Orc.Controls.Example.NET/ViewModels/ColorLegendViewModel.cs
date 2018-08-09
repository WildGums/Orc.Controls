// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorLegendViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;

    public class ColorLegendViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorLegendViewModel"/> class.
        /// </summary>
        public ColorLegendViewModel()
        {
            CalendarStateLegend = new List<IColorLegendItem>();
            UpdateItems = new Command(UpdateCalenderStateLegend);
        }

        /// <summary>
        /// Gets or sets the calendar state legend.
        /// </summary>
        public IList<IColorLegendItem> CalendarStateLegend { get; set; }

        public Command UpdateItems { get; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            UpdateCalenderStateLegend();

            PropertyChanged += (sender, args) => { };
        }

        private void UpdateCalenderStateLegend()
        {
            var colors = new List<Color>
            {
                Colors.Blue,
                Colors.Red,
                Colors.Green,
                Colors.Orange,
                Colors.Gray
            };

            if (CalendarStateLegend == null)
            {
                CalendarStateLegend = new List<IColorLegendItem>();
            }
            else
            {
                CalendarStateLegend.Clear();
            }

            foreach (var color in colors)
            {
                CalendarStateLegend.Add(new ColorLegendItem
                {
                    Color = color,
                    Description = $"this color is {color}",
                    IsChecked = true,
                    Id = color.ToString(),
                    AdditionalData = "(1)"
                });
            }

            // PropertyChanged(this, new PropertyChangedEventArgs("CalendarStateLegend"));
            //   CalendarStateLegend = ; //new List<IColorLegendItem>(items);

            RaisePropertyChanged(nameof(CalendarStateLegend));
        }
    }
}
