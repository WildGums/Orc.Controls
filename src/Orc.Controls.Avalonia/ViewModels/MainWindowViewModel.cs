namespace Orc.Controls.Avalonia.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Catel.Services;
    using Orc.FileSystem;

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            DirectoryPicker = new DirectoryPickerViewModel(new SelectDirectoryService(), new DirectoryService(new FileService()), new ProcessService());
            CulturePicker = new CulturePickerViewModel();
        }

        public DirectoryPickerViewModel DirectoryPicker { get; }
        public CulturePickerViewModel CulturePicker { get; }

        public string Greeting => "Welcome to Avalonia!";
    }
}
