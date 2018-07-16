// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterEditorViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Catel;
    using Catel.Fody;
    using Catel.MVVM;

    using Orc.Controls.Models;

    public class LogFilterEditorViewModel : ViewModelBase
    {
        public LogFilterEditorViewModel(LogFilter logFilter)
        {
            LogFilter = logFilter;
        }

        public ObservableCollection<LogFilterAction> Actions { get; set; }

        public ObservableCollection<LogFilterExpressionType> ExpressionTypes { get; set; }

        [Model]
        [Expose("Name")]
        [Expose("ExpressionType")]
        [Expose("ExpressionValue")]
        [Expose("Action")]
        [Expose("Target")]
        public LogFilter LogFilter { get; set; }

        public ObservableCollection<LogFilterTarget> Targets { get; set; }

        protected override async Task InitializeAsync()
        {
            ExpressionTypes = new ObservableCollection<LogFilterExpressionType>(Enum<LogFilterExpressionType>.GetValues());
            Actions = new ObservableCollection<LogFilterAction>(Enum<LogFilterAction>.GetValues());
            Targets = new ObservableCollection<LogFilterTarget>(Enum<LogFilterTarget>.GetValues());
        }
    }
}
