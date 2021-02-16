using System.Windows.Media;
using VA.Interfaces;
using VA.Resources;
using VA.ViewModels;

namespace VA.Modules
{
    public class SortModule : IModule
    {
        #region Public Properties

        public IAnimation Animation { get; private set; }

        public Brush Background { get; private set; }

        public ModuleType ModuleType => ModuleType.SortModule;

        public string Title { get; private set; }

        #endregion

        #region Public Constructors

        public SortModule()
        {
        }

        #endregion

        #region Public Methods

        public void InitializeModule()
        {
            Title = "Sort Module";
            Background = new SolidColorBrush(Color.FromRgb(119, 119, 119));
            Animation = new SortModuleAnimationViewModel();
        }

        #endregion
    }
}