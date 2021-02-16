using Prism.Mvvm;
using System.Windows.Media;
using VA.Resources;

namespace VA.ViewModels
{
    public class SortModuleItemViewModel : BindableBase
    {
        #region Private Fields

        private double _height;
        private double _left;
        private SortItemsState _state;
        private double _top;
        private int _value;
        private double _width;

        #endregion

        #region Public Properties

        public SolidColorBrush BackgroundBrush
        {
            get
            {
                switch (State)
                {
                    case SortItemsState.Inactive:
                        return new SolidColorBrush(Color.FromRgb(255, 255, 255));

                    case SortItemsState.Active:
                        return new SolidColorBrush(Color.FromRgb(245, 200, 26));

                    case SortItemsState.Pivot:
                        return new SolidColorBrush(Color.FromRgb(105, 255, 116));

                    default:
                        return null;
                }
            }
        }

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }

        public SortItemsState State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged(nameof(BackgroundBrush));
            }
        }

        public double Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        #endregion

        #region Public Constructors

        public SortModuleItemViewModel(double width, double height, double top, double left, int value)
        {
            Width = width;
            Height = height;
            Top = top;
            Left = left;
            Value = value;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Left} | {Value}";
        }

        #endregion
    }
}