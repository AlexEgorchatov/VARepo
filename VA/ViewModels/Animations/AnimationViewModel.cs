using Prism.Mvvm;
using System.Collections.Generic;
using System.Timers;
using VA.Interfaces;

namespace VA.ViewModels
{
    public class AnimationViewModel : BindableBase, IAnimation
    {
        private Timer _timer;
        private List<AnimationItemViewModel> _animationItems;
        private double[,] _heights;
        private int _counter;

        public List<AnimationItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

        public AnimationViewModel()
        {
            AnimationItems = new List<AnimationItemViewModel>()
            {
                new AnimationItemViewModel(30, 100, 30, 30),
                new AnimationItemViewModel(30, 100, 30, 90),
                new AnimationItemViewModel(30, 100, 30, 150),
                new AnimationItemViewModel(30, 100, 30, 210),
                new AnimationItemViewModel(30, 100, 30, 270),
            };
            _heights = new double[5, 5] {
                { 210, 240, 300, 180, 270 },
                { 240, 210, 270, 180, 300 },
                { 240, 180, 210, 300, 270 },
                { 300, 180, 240, 210, 270 },
                { 180, 210, 240, 270, 300 },
            };
        }

        public void StartTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            AnimationItems.ForEach(i => i.Height = _heights[_counter, AnimationItems.IndexOf(i)]);
            _counter = _counter == AnimationItems.Count - 1 ? 0 : _counter + 1;
        }
    }
}