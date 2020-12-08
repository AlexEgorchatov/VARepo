using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VA.ViewModels.Animations
{
    public class GridPathModuleCellViewModel : BindableBase
    {
        private double _width;

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        private double _height;

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private double _top;

        public double Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

        private double _left;

        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }

        private SolidColorBrush _activeBrush;

        private SolidColorBrush _inactiveBrush;

        public SolidColorBrush BackgroundBrush
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

        public GridPathModuleCellViewModel(double width, double height, double top, double left)
        {
            Width = width;
            Height = height;
            Top = top;
            Left = left;
            _inactiveBrush = new SolidColorBrush(Colors.Gray);
            _activeBrush = new SolidColorBrush(Colors.Yellow);
        }
    }
}
