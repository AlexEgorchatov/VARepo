using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;

namespace VA.ViewModels
{
    public class SortModulePageViewModel : BindableBase
    {
        #region Private Fields

        private DelegateCommand _applyCommand;
        private DelegateCommand _backCommand;
        private string _input;
        private bool _isApplied;
        private Regex _regularExpression;
        private DelegateCommand _runCommand;
        private ObservableCollection<SortModuleItemViewModel> _sortItems;

        #endregion

        #region Public Properties

        public DelegateCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new DelegateCommand(() =>
                {
                    if (Input == null) return;

                    string[] numbers = Input.Split(' ');
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        if(numbers[i] != "")
                        {
                            SortItems.Add(new SortModuleItemViewModel(30, 30 * (i + 1), 330, 60 * (i + 1)));
                        }
                    }
                    IsApplied = true;
                }));
            }
        }

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
                    if(IsApplied) IsApplied = false;
                }
            }
        }

        public bool IsApplied
        {
            get { return _isApplied; }
            set 
            { 
                SetProperty(ref _isApplied, value);
                if (!value)
                {
                    SortItems.Clear();
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

        public ObservableCollection<SortModuleItemViewModel> SortItems
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
            SortItems = new ObservableCollection<SortModuleItemViewModel>();
            IsApplied = false;
        }

        #endregion
    }
}