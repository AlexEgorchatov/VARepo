using System.Windows.Media;
using VA.Resources;

namespace VA.Interfaces
{
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