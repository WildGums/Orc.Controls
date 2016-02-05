// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnableToolTipViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using Catel.MVVM;

    public class PinnableToolTipViewModel : ViewModelBase
    {
        public PinnableToolTipViewModel()
        {
            ColorList = new List<Brush>()
            {
                new SolidColorBrush(Colors.CornflowerBlue),
                new SolidColorBrush(Colors.CadetBlue),
                new SolidColorBrush(Colors.LightSeaGreen),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.OrangeRed),
                new SolidColorBrush(Colors.Red),
            };
        }

        public List<Brush> ColorList{ get; set; }
    }
}