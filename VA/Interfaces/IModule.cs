using System.Windows.Media;

namespace VA.Interfaces
{
    public enum ModuleType
    {
        SortModule, StringMatchingModule, GridPathModule
    }

    public interface IModule
    {
        #region Public Properties

        IAnimation Animation { get; }

        Brush Background { get; }

        ModuleType ModuleType { get; }

        string Title { get; }

        #endregion

        #region Public Methods

        void InitializeModule();

        #endregion
    }
}