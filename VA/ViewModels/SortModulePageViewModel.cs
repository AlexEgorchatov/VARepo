using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;

namespace VA.ViewModels
{
    public class SortModulePageViewModel : BindableBase
    {
        #region Private Fields

        private DelegateCommand _backCommand;
        private string _input;
        private Regex _regularExpression;
        private DelegateCommand _runCommand;

        private List<SortModuleItemViewModel> _sortItems;

        #endregion

        #region Public Properties

        public DelegateCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new DelegateCommand(() =>
                {
                    var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                    navigationService?.GoBack();
                }));
            }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                if (_regularExpression.IsMatch(value) || value.Length == 0)
                {
                    SetProperty(ref _input, value);
                }
            }
        }

        public DelegateCommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand = new DelegateCommand(() =>
                {
                    //MessageBox.Show(Input);
                }));
            }
        }

        public List<SortModuleItemViewModel> SortItems
        {
            get { return _sortItems; }
            set { SetProperty(ref _sortItems, value); }
        }

        public List<string> SortTabs { get; set; }

        #endregion

        #region Public Constructors

        public SortModulePageViewModel()
        {
            _regularExpression = new Regex(@"^[0-9]+(\s[0-9]+)*(\s)?$");
            SortTabs = new List<string>() { "Bubble Sort", "Quick Sort" };
            SortItems = new List<SortModuleItemViewModel>();
        }

        #endregion
    }
}