namespace Orc.Controls
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;

    /// <summary>
    /// Interaction logic for AgeWizardPageView.xaml
    /// </summary>
    public partial class AgeWizardPageView : IWizardPage
    {
        public AgeWizardPageView()
        {
            InitializeComponent();
        }

        //public IViewModel ViewModel { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Title { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string BreadcrumbTitle { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Description { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Number { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool IsOptional => throw new System.NotImplementedException();

        public bool IsVisited { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Task AfterWizardPagesSavedAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task CancelAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
