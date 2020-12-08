using Prism.Commands;
using System.Timers;

namespace VA.Interfaces
{
    public interface IAnimation
    {
        DelegateCommand StartAnimation { get; }

        DelegateCommand StopAnimation { get; }

    }
}