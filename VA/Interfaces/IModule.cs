using System.Windows.Media;

namespace VA.Interfaces
{
    public interface IModule
    {
        string Title { get; }

        Brush Background { get; }

        IAnimation Animation { get; }

        void StartAnimation();
    }
}
