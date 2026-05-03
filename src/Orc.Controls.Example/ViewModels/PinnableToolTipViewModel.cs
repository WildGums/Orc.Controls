namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using System.Windows.Media;
using Catel.MVVM;

public class PinnableToolTipViewModel : ViewModelBase
{
    public PinnableToolTipViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
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

    public List<Brush> ColorList { get; set; }
}
