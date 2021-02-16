using Prism.Mvvm;
using System.Windows.Media;
using VA.Resources;

namespace VA.ViewModels.Animations
{
    public class StringMatchingModuleCharViewModel : BindableBase
    {
        #region Private Fields

        private char _character;
        private StringMatchingItemsState _state;

        #endregion

        #region Public Properties

        public char Character
        {
            get { return _character; }
            set { SetProperty(ref _character, value); }
        }

        public SolidColorBrush ForegroundBrush
        {
            get
            {
                switch (State)
                {
                    case StringMatchingItemsState.Inactive:
                        return new SolidColorBrush(Color.FromRgb(255, 255, 255));

                    case StringMatchingItemsState.Active:
                        return new SolidColorBrush(Color.FromRgb(245, 200, 26));

                    case StringMatchingItemsState.Space:
                        return new SolidColorBrush(Color.FromRgb(119, 119, 119));

                    default:
                        return null;
                }
            }
        }

        public StringMatchingItemsState State
        {
            get { return _state; }
            set
            {
                SetProperty(ref _state, value);
                RaisePropertyChanged(nameof(ForegroundBrush));
            }
        }

        #endregion

        #region Public Constructors

        public StringMatchingModuleCharViewModel(char character)
        {
            Character = character;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Character}";
        }

        #endregion
    }
}