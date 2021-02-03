using Prism.Mvvm;
using System.Windows.Media;

namespace VA.ViewModels.Animations
{
    public class StringMatchingModuleCharViewModel : BindableBase
    {
        #region Private Fields

        private SolidColorBrush _activeBrush;

        private SolidColorBrush _inactiveBrush;

        private bool _isActive;

        #endregion

        #region Public Properties

        public char Character { get; set; }

        public SolidColorBrush ForegroundBrush
        {
            get { return _isActive ? _activeBrush : _inactiveBrush; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged("ForegroundBrush");
            }
        }

        #endregion

        #region Public Constructors

        public StringMatchingModuleCharViewModel(char character)
        {
            Character = character;
            _inactiveBrush = new SolidColorBrush(Colors.White);
            _activeBrush = new SolidColorBrush(Color.FromRgb(245, 200, 26));
        }

        #endregion

        public override string ToString()
        {
            return $"{Character}";
        }
    }
}