using System.Windows.Media;
using VA.Interfaces;
using VA.ViewModels;

namespace VA.Modules
{
    public class SortModule : IModule
    {
        public string Title { get; private set; }

        public Brush Background { get; private set; }

        public IAnimation Animation { get; private set; }

        public ModuleType ModuleType => ModuleType.SortModule;

        public SortModule()
        {
        }

        public void StartAnimation()
        {
            Title = "Sort Module";
            Background = new SolidColorBrush(Colors.Red);
            Animation = new SortModuleAnimationViewModel();
        }
    }
}