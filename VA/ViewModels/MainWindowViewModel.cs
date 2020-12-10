using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using VA.Interfaces;
using VA.Modules;

namespace VA.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private List<IModule> _modules;

        #endregion

        #region Public Properties

        public List<IModule> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }

        #endregion

        #region Public Constructors

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