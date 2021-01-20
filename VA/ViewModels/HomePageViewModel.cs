using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VA.Interfaces;
using VA.Modules;
using VA.Resources;
using VA.Views.Pages;

namespace VA.ViewModels
{
    public class HomePageViewModel : BindableBase
    {
        #region Private Fields

        private List<IModule> _modules;
        private IModule _selectedModule;

        #endregion

        #region Public Properties

        public List<IModule> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        public IModule SelectedModule
        {
            get { return _selectedModule; }
            set 
            { 
                SetProperty(ref _selectedModule, value);
                if (SelectedModule != null)
                {
                    var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                    switch (SelectedModule.ModuleType)
                    {
                        case ModuleType.SortModule:
                            navigationService?.Navigate(new SortModulePage(new SortModulePageViewModel()));
                            break;

                        case ModuleType.StringMatchingModule:
                            navigationService?.Navigate(new SortModulePage(new SortModulePageViewModel()));
                            break;

                        case ModuleType.GridPathModule:
                            navigationService?.Navigate(new SortModulePage(new SortModulePageViewModel()));
                            break;
                    }
                }
            }
        }

        #endregion

        #region Public Constructors

        public HomePageViewModel()
        {
        }

        #endregion

        #region Private Methods

        private void CreateModules()
        {
            Modules = new List<IModule>()
            {
                new SortModule(),
                new StringMatchingModule(),
                new GridPathModule()
            };
        }

        #endregion

        #region Public Methods

        public Task Initialize()
        {
            return Task.Run(() =>
            {
                CreateModules();
                var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                navigationService.Navigated += OnPageNavigated;
            });
        }

        private void OnPageNavigated(object sender, NavigationEventArgs e)
        {
            switch ((e.Content as Page).DataContext) 
            {
                case HomePageViewModel:
                    ((e.Content as Page).DataContext as HomePageViewModel).SelectedModule = null;
                    break;
            }
        }

        public void InitializeModules()
        {
            Modules.ForEach(i => i.InitializeModule());
        }

        #endregion
    }
}