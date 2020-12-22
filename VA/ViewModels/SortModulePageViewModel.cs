using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Navigation;

namespace VA.ViewModels
{
    public class SortModulePageViewModel : BindableBase
    {
        #region Private Fields

        private DelegateCommand _backCommand;

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

        #endregion
    }
}