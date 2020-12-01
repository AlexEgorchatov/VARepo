using System;
using System.Windows.Media;
using VA.Interfaces;
using VA.ViewModels;
using VA.ViewModels.Animations;

namespace VA.Modules
{
    public class StringMatchingModule : IModule
    {
        public string Title { get; private set; }

        public Brush Background { get; private set; }

        public IAnimation Animation { get; private set; }

        public ModuleType ModuleType => ModuleType.StringModule;

        public StringMatchingModule()
        {

        }

        public void StartAnimation()
        {
            Title = "String Matching Module";
            Background = new SolidColorBrush(Colors.Blue); 
            Animation = new StringMatchingModuleAnimationViewModel();
        }
    }
}
