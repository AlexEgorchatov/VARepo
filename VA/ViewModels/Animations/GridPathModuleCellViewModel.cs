using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;

namespace VA.ViewModels.Animations
{
    public enum ColorType
    {
        Neutral, Start, Destination, Path, Search
    }

    public class GridPathModuleCellViewModel : BindableBase
    {
        #region Private Fields

        private ColorType _colorType;
        private SolidColorBrush _destinationBrush;
        private double _height;
        private bool _isVisible;
        private double _left;
        private SolidColorBrush _neutralBrush;
        private SolidColorBrush _pathBrush;
        private SolidColorBrush _searchBrush;
        private SolidColorBrush _startBrush;
        private double _top;
        private double _width;

        #endregion

        #region Public Fields

        public static Thickness Margin = new Thickness(15, 30, 15, 30);

        #endregion

        #region Public Properties

        public SolidColorBrush BackgroundBrush
        {
            get
            {
                switch (ColorType)
                {
                    case ColorType.Neutral:
                        return _neutralBrush;

                    case ColorType.Start:
                        return _startBrush;

                    case ColorType.Destination:
                        return _destinationBrush;

                    case ColorType.Path:
                        return _pathBrush;

                    case ColorType.Search:
                        return _searchBrush;

                    default:
                        return _neutralBrush;
                }
            }
        }

        public ColorType ColorType
        {
            get { return _colorType; }
            set
            {
                _colorType = value;
                RaisePropertyChanged("BackgroundBrush");
            }
        }

        public int Column { get; }

        public int Distance { get; set; }

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public bool IsVisited { get; set; }

        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }

        public int Row { get; }

        public double Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        #endregion

        #region Public Constructors

        public GridPathModuleCellViewModel(int x, int y, double size)
        {
            Row = x;
            Column = y;
            Distance = -1;
            SetSize(size);
            ColorType = ColorType.Neutral;
            IsVisited = false;
            _neutralBrush = new SolidColorBrush(Colors.White);
            _startBrush = new SolidColorBrush(Colors.Green);
            _destinationBrush = new SolidColorBrush(Colors.Red);
            _pathBrush = new SolidColorBrush(Colors.DarkRed);
            _searchBrush = new SolidColorBrush(Color.FromRgb(245, 200, 26));
        }

        #endregion

        #region Public Methods

        public void SetSize(double size)
        {
            Width = size;
            Height = size;
            Top = Margin.Top + size * Row;
            Left = Margin.Left + size * Column;
        }

        #endregion
    }
}