using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using VA.Interfaces;

namespace VA.ViewModels
{
    public class AnimationViewModel : BindableBase, IAnimation
    {
        private Timer _timer;

        private List<AnimationItemViewModel> _animationItems;

        private List<double> _initialHeights;

        private int i;

        private int j;

        private int itemsCount;

        private bool _isLast;

        public List<AnimationItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

        private DelegateCommand _startTimer;

        public DelegateCommand StartTimers
        {
            get
            {
                return _startTimer ?? (_startTimer = new DelegateCommand(() =>
                {
                    _timer = new Timer();
                    _timer.Interval = 1000;
                    _timer.Start();
                    _timer.Elapsed += TimerElapsed;

                    itemsCount = AnimationItems.Count;
                    i = 0;
                    j = 0;
                    _isLast = false;
                }));
            }
        }

        private DelegateCommand _stopTimer;

        public DelegateCommand StopTimer
        {
            get
            {
                return _stopTimer ?? (_stopTimer = new DelegateCommand(() =>
                {
                    _timer.Stop();
                    for (int i = 0; i < AnimationItems.Count; i++)
                    {
                        AnimationItems[i].Height = _initialHeights[i];
                    }
                    AnimationItems.ForEach(i => i.IsActive = false);
                }));
            }
        }

        public AnimationViewModel()
        {
            AnimationItems = new List<AnimationItemViewModel>()
            {
                new AnimationItemViewModel(30, 240, 330, 60),
                new AnimationItemViewModel(30, 210, 330, 120),
                new AnimationItemViewModel(30, 180, 330, 180),
                new AnimationItemViewModel(30, 300, 330, 240),
                new AnimationItemViewModel(30, 270, 330, 300),
            };

            _initialHeights = new List<double>();
            foreach (var item in AnimationItems)
            {
                _initialHeights.Add(item.Height);
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if(_isLast)
                {
                    for (int k = 0; k < AnimationItems.Count; k++)
                    {
                        AnimationItems[k].Height = _initialHeights[k];
                    }
                    AnimationItems.ForEach(i => i.IsActive = false);
                    _isLast = false;

                    return;
                }
                //bool isSwapped = false;
                AnimationItems.ForEach(i => i.IsActive = false);
                if (AnimationItems[j].Height > AnimationItems[j + 1].Height)
                {
                    double tempHight = AnimationItems[j].Height;
                    AnimationItems[j].Height = AnimationItems[j + 1].Height;
                    AnimationItems[j + 1].Height = tempHight;
                    //isSwapped = true;
                }

                AnimationItems[j].IsActive = true;
                AnimationItems[j + 1].IsActive = true;

                if (j == itemsCount - i - 2)
                {
                    j = 0;
                }
                else
                {
                    j++;
                }

                if (j == 0)
                {
                    i++;
                }
                if (i == itemsCount - 1)
                {
                    i = 0;
                    _isLast = true;
                }
            });
        }
    }
}