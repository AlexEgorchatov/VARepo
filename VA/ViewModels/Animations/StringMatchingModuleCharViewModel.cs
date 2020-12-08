using Prism.Mvvm;
using System.Windows.Media;

namespace VA.ViewModels.Animations
{
    public class StringMatchingModuleCharViewModel : BindableBase
    {
        private SolidColorBrush _activeBrush;

        private SolidColorBrush _inactiveBrush;

        public SolidColorBrush ForegroundBrush
        {
            get { return _isActive ? _activeBrush : _inactiveBrush; }
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged("ForegroundBrush");
            }
        }

        public char Character { get; set; }


        public StringMatchingModuleCharViewModel(char character)
        {
            Character = character;
            _inactiveBrush = new SolidColorBrush(Colors.Black);
            _activeBrush = new SolidColorBrush(Colors.Green);
        }
    }
}