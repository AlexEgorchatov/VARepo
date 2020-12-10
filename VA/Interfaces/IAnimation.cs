using Prism.Commands;

namespace VA.Interfaces
{
    public interface IAnimation
    {
        #region Public Properties

        DelegateCommand StartAnimation { get; }

        DelegateCommand StopAnimation { get; }

        #endregion
    }
}