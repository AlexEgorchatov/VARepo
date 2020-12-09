using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const double _cellSide = 60;

        public double Width { get; }

        public double Height { get; }

        public double Top { get; }

        public double Left { get; }

        public int Distance { get; set; }

        public bool IsVisited { get; set; }

        private ColorType _colorType;
        public ColorType ColorType 
        {
            get { return _colorType; }
            set
            {
                _colorType = value;
                RaisePropertyChanged("BackgroundBrush");
            } 
        }

        private int _xCoordinate;
        public int XCoordinate
        {
            get { return _xCoordinate; }
            set { SetProperty(ref _xCoordinate, value); }
        }

        private int _yCoordinate;
        public int YCoordinate
        {
            get { return _yCoordinate; }
            set { SetProperty(ref _yCoordinate, value); }
        }
        
        private SolidColorBrush _neutralBrush;
        private SolidColorBrush _startBrush;
        private SolidColorBrush _destinationBrush;
        private SolidColorBrush _pathBrush;
        private SolidColorBrush _searchBrush;

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
            _neutralBrush = new SolidColorBrush(Colors.Gray);
            _startBrush = new SolidColorBrush(Colors.Green);
            _destinationBrush = new SolidColorBrush(Colors.Red);
            _pathBrush = new SolidColorBrush(Colors.OrangeRed);
            _searchBrush = new SolidColorBrush(Colors.Orange);
        }
    }
}
