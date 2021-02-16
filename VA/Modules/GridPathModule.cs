using System.Windows.Media;
using VA.Interfaces;
using VA.Resources;
using VA.ViewModels.Animations;

namespace VA.Modules
{
    public class GridPathModule : IModule
    {
        #region Public Properties

        public IAnimation Animation { get; private set; }

        public Brush Background { get; private set; }

        public ModuleType ModuleType => ModuleType.GridPathModule;

        public string Title { get; private set; }

        #endregion

        #region Public Constructors

        public GridPathModule()
        {
        }

        #endregion

        #region Public Methods

        public void InitializeModule()
        {
            Title = "Grid Path Module";
            Background = new SolidColorBrush(Color.FromRgb(119, 119, 119));
            Animation = new GridPathModuleAnimationViewModel();
        }

        #endregion
    }
}