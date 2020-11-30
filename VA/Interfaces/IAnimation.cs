using Prism.Commands;

namespace VA.Interfaces
{
    public interface IAnimation
    {
        DelegateCommand StartTimers { get; }

        DelegateCommand StopTimer { get; }
    }
}