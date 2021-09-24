namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using Catel.MVVM;

    public class SpinButtonViewModel : ViewModelBase
    {
        #region Fields
        private readonly List<string> _allText = new()
        {
            "Olivia",
            "Emma",
            "Ava",
            "Charlotte",
            "Sophia",
            "Amelia",
            "Isabella",
            "Mia"
        };
        #endregion

        #region Constructors
        public SpinButtonViewModel()
        {
            _allText.Sort();

            ShowNextName = new Command<string>(OnShowNextName, CanShowNextName);
            ShowPreviousName = new Command<string>(OnShowPreviousName, CanShowPreviousName);

            Text = _allText[0];
        }
        #endregion

        #region Properties
        public string Text { get; set; }
        #endregion

        #region Next Name command
        public Command<string> ShowNextName { get; }

        private void OnShowNextName(string name)
        {
            var nameIndex = _allText.IndexOf(name);
            if (nameIndex < 0 || nameIndex >= _allText.Count - 1)
            {
                return;
            }

            Text = _allText[nameIndex + 1];
        }

        private bool CanShowNextName(string name)
        {
            var nameIndex = _allText.IndexOf(name);
            return nameIndex >= 0 && nameIndex < _allText.Count - 1;
        }
        #endregion

        #region Previous name command
        public Command<string> ShowPreviousName { get; }

        private void OnShowPreviousName(string name)
        {
            var nameIndex = _allText.IndexOf(name);
            if (nameIndex <= 0)
            {
                return;
            }

            Text = _allText[nameIndex - 1];
        }

        private bool CanShowPreviousName(string name)
        {
            var nameIndex = _allText.IndexOf(name);
            return nameIndex > 0;
        }
        #endregion
    }
}
