using Prism.Mvvm;
using System.Windows.Media;
using VA.Resources;

namespace VA.ViewModels.Animations
{
    public class StringMatchingModuleCharViewModel : BindableBase
    {
        #region Private Fields

        #endregion

        #region Public Properties

        private StringMatchingItemsState _state;
        public StringMatchingItemsState State
        {
            get { return _state; }
            set 
            { 
                SetProperty(ref _state, value);
                RaisePropertyChanged(nameof(ForegroundBrush));
            }
        }

        private char _character;
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

        #endregion

        #region Public Constructors

        public StringMatchingModuleCharViewModel(char character)
        {
            Character = character;
        }

        #endregion

        public override string ToString()
        {
            return $"{Character}";
        }
    }
}