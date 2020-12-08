using System.Windows.Media;
using VA.Interfaces;
using VA.ViewModels.Animations;

namespace VA.Modules
{
    public class GridPathModule : IModule
    {
        public string Title { get; private set; }

        public Brush Background { get; private set; }

        public IAnimation Animation { get; private set; }

        public ModuleType ModuleType => ModuleType.GridPathModule;

        public GridPathModule()
        {
        }

        public void StartAnimation()
        {
            Title = "Grid Path Module";
            Background = new SolidColorBrush(Colors.Green);
            Animation = new GridPathModuleAnimationViewModel();
        }
    }
}