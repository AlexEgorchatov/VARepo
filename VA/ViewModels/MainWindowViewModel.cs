using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VA.Interfaces;
using VA.Modules;

namespace VA.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        #region Properties

        private List<IModule> _modules;
        public List<IModule> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        #endregion

        #region Commands

        /*private DelegateCommand _loadCommand;
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
        }*/

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {

        }

        #endregion

        #region Private Methods

        private void CreateModule()
        {
            Modules = new List<IModule>()
            {
                new SortModule(),
                new StringMatchingModule()
            };

        }

        #endregion

        #region Public Methods

        public Task Initialize()
        {
            return Task.Run(() =>
            {
                CreateModule();
            });
        }

        public void StartAnimation()
        {
            Modules.ForEach(i => i.StartAnimation());
        }

        #endregion
    }
}
