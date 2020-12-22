using System.Windows.Media;
using VA.Interfaces;
using VA.ViewModels.Animations;

namespace VA.Modules
{
    public class StringMatchingModule : IModule
    {
        #region Public Properties

        public IAnimation Animation { get; private set; }

        public Brush Background { get; private set; }

        public ModuleType ModuleType => ModuleType.StringMatchingModule;

        public string Title { get; private set; }

        #endregion

        #region Public Constructors

        public StringMatchingModule()
        {
        }

        #endregion

        #region Public Methods

        public void InitializeModule()
        {
            Title = "String Matching Module";
            Background = new SolidColorBrush(Color.FromRgb(119, 119, 119));
            Animation = new StringMatchingModuleAnimationViewModel();
        }

        #endregion
    }
}