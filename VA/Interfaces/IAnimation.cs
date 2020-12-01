using Prism.Commands;
using System.Timers;

namespace VA.Interfaces
{
    public interface IAnimation
    {
        DelegateCommand StartTimer { get; }

        DelegateCommand StopTimer { get; }

    }
}