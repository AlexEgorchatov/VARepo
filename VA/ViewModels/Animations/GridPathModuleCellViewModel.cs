using Prism.Mvvm;
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

        private const double _cellSide = 60;

        private ColorType _colorType;

        private SolidColorBrush _destinationBrush;

        private SolidColorBrush _neutralBrush;

        private SolidColorBrush _pathBrush;

        private SolidColorBrush _searchBrush;

        private SolidColorBrush _startBrush;

        private int _xCoordinate;

        private int _yCoordinate;

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

        public int Distance { get; set; }

        public double Height { get; }

        public bool IsVisited { get; set; }

        public double Left { get; }

        public double Top { get; }

        public double Width { get; }

        public int XCoordinate
        {
            get { return _xCoordinate; }
            set { SetProperty(ref _xCoordinate, value); }
        }

        public int YCoordinate
        {
            get { return _yCoordinate; }
            set { SetProperty(ref _yCoordinate, value); }
        }

        #endregion

        #region Public Constructors

        public GridPathModuleCellViewModel(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
            Distance = 0;
            Width = _cellSide;
            Height = _cellSide;
            Top = 30 + 60 * x;
            Left = 15 + 60 * y;
            ColorType = ColorType.Neutral;
            IsVisited = false;
            _neutralBrush = new SolidColorBrush(Colors.White);
            _startBrush = new SolidColorBrush(Colors.Green);
            _destinationBrush = new SolidColorBrush(Colors.Red);
            _pathBrush = new SolidColorBrush(Colors.OrangeRed);
            _searchBrush = new SolidColorBrush(Color.FromRgb(245, 200, 26));
        }

        #endregion
    }
}