﻿namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using Controls;

    public class CulturePickerMap : AutomationBase
    {
        public CulturePickerMap(AutomationElement element) 
            : base(element)
        {
        }

        public ComboBox Combobox => By.One<ComboBox>();
    }
}
