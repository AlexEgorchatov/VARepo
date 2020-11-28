using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VA.ViewModels
{
    public class AnimationItemViewModel : BindableBase
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

        private int _value;
        public int Value { 
            get { return _value; }
            set 
            {
                _value = value;
                Left = 30 + 60 * Value;
            } 
        }

        public AnimationItemViewModel(double width, double height, double top, double left, int value)
        {
            Width = width;
            Height = height;
            Top = top;
            Left = left;
            _value = value;
        }

        public override string ToString()
        {
            return $"{Value} | {Left} | {Height}";
        }
    }
}
