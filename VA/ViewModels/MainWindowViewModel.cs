using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VA.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties

        public ObservableCollection<string> Customers { get; private set; }

        private string _selectedCustomer;
        public string SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { SetProperty(ref _selectedCustomer, value); }
        }

        #endregion

        #region Commands

        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand = new DelegateCommand(() =>
                {
                    Customers.Clear();
                    Customers.Add("four");
                    Customers.Add("five");
                    Customers.Add("six");
                }));
            }
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            Customers = new ObservableCollection<string>()
            {
                "one", "two", "three"
            };
            SelectedCustomer = Customers[1];
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }
}
