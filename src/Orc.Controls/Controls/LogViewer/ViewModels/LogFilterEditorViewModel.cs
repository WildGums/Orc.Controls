// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterEditorViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Orc.Controls.Models;

    public class LogFilterEditorViewModel : ViewModelBase
    {
        public LogFilterEditorViewModel(LogFilter logFilter)
        {
            LogFilter = logFilter;
        }

        [Model]
        public LogFilter LogFilter { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public string Name { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public ExpressionType ExpressionType { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public string ExpressionValue { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public Action Action { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public Target Target { get; set; }

        public ObservableCollection<Action> Actions { get; set; }

        public ObservableCollection<Target> Targets { get; set; }

        public ObservableCollection<ExpressionType> ExpressionTypes { get; set; }

        protected override async Task InitializeAsync()
        {
            ExpressionTypes = new ObservableCollection<ExpressionType>(Enum<ExpressionType>.GetValues());
            Actions = new ObservableCollection<Action>(Enum<Action>.GetValues());
            Targets = new ObservableCollection<Target>(Enum<Target>.GetValues());
        }
    }
}
