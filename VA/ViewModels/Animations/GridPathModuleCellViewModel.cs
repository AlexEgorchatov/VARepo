using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;
using VA.Resources;

namespace VA.ViewModels.Animations
{
    public class GridPathModuleCellViewModel : BindableBase
    {
        #region Private Fields

        private double _height;
        private bool _isVisible;
        private double _left;
        private GridPathItemsState _state;
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
                switch (State)
                {
                    case GridPathItemsState.Neutral:
                        return new SolidColorBrush(Colors.White);

                    case GridPathItemsState.Start:
                        return new SolidColorBrush(Colors.Green);

                    case GridPathItemsState.Destination:
                        return new SolidColorBrush(Colors.Red);

                    case GridPathItemsState.Path:
                        return new SolidColorBrush(Colors.DarkRed);

                    case GridPathItemsState.Search:
                        return new SolidColorBrush(Color.FromRgb(245, 200, 26));

                    default:
                        return null;
                }
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

        public GridPathItemsState State
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
            IsVisited = false;
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