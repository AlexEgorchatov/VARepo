using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;

namespace VA.ViewModels
{
    public class SortModuleItemViewModel : BindableBase
    {
        #region Private Fields

        private SolidColorBrush _activeBrush;
        private Thickness _columnMargin;
        private double _height;
        private SolidColorBrush _inactiveBrush;
        private bool _isActive;
        private double _left;
        private double _top;
        private int _value;
        private double _width;

        #endregion

        #region Public Properties

        public SolidColorBrush BackgroundBrush
        {
            get { return _isActive ? _activeBrush : _inactiveBrush; }
        }

        public Thickness ColumnMargin
        {
            get { return _columnMargin; }
            set { SetProperty(ref _columnMargin, value); }
        }

        public double Height
        {
            get { return _height; }
            set 
            {
                SetProperty(ref _height, value);
                ColumnMargin = new Thickness(0, -value, 0, 0);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged("BackgroundBrush");
            }
        }

        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
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
            _inactiveBrush = new SolidColorBrush(Colors.White);
            _activeBrush = new SolidColorBrush(Color.FromRgb(245, 200, 26));
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{Left} | {Height}";
        }

        #endregion
    }
}