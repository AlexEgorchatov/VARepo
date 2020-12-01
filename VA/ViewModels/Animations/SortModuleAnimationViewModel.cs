using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using VA.Interfaces;

namespace VA.ViewModels
{
    public class SortModuleAnimationViewModel : BindableBase, IAnimation
    {
        private Timer _timer;

        private List<SortModuleItemViewModel> _animationItems;

        private List<double> _initialHeights;

        private int i;

        private int j;

        private int _itemsCount;

        private bool _isLast;

        public List<SortModuleItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

        private DelegateCommand _startTimer;

        public DelegateCommand StartTimer
        {
            get
            {
                return _startTimer ?? (_startTimer = new DelegateCommand(() =>
                {
                    _timer = new Timer();
                    _timer.Interval = 1000;
                    _timer.Start();
                    _timer.Elapsed += TimerElapsed;

                    _itemsCount = AnimationItems.Count;
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
                    ResetModuleAnimation();
                }));
            }
        }

        public SortModuleAnimationViewModel()
        {
            AnimationItems = new List<SortModuleItemViewModel>()
            {
                new SortModuleItemViewModel(30, 240, 330, 60),
                new SortModuleItemViewModel(30, 210, 330, 120),
                new SortModuleItemViewModel(30, 180, 330, 180),
                new SortModuleItemViewModel(30, 300, 330, 240),
                new SortModuleItemViewModel(30, 270, 330, 300),
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
                    ResetModuleAnimation();
                    _isLast = false;

                    return;
                }

                AnimationItems.ForEach(i => i.IsActive = false);
                if (AnimationItems[j].Height > AnimationItems[j + 1].Height)
                {
                    double tempHight = AnimationItems[j].Height;
                    AnimationItems[j].Height = AnimationItems[j + 1].Height;
                    AnimationItems[j + 1].Height = tempHight;
                }

                AnimationItems[j].IsActive = true;
                AnimationItems[j + 1].IsActive = true;

                if (j == _itemsCount - i - 2)
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
                if (i == _itemsCount - 1)
                {
                    i = 0;
                    _isLast = true;
                }
            });
        }

        private void ResetModuleAnimation()
        {
            for (int i = 0; i < AnimationItems.Count; i++)
            {
                AnimationItems[i].Height = _initialHeights[i];
            }
            AnimationItems.ForEach(i => i.IsActive = false);
        }
    }
}