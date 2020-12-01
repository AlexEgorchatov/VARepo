using System.Windows.Media;

namespace VA.Interfaces
{
    public enum ModuleType
    {
        SortModule, StringModule
    }

    public interface IModule
    {
        string Title { get; }

        ModuleType ModuleType { get; }

        Brush Background { get; }

        IAnimation Animation { get; }

        void StartAnimation();
    }
}
