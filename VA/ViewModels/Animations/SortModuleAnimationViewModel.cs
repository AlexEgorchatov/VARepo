using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using VA.Interfaces;

namespace VA.ViewModels
{
    public class SortModuleAnimationViewModel : BindableBase, IAnimation
    {
        private bool _isCanceled;
        private readonly Mutex _mutex;
        private List<SortModuleItemViewModel> _animationItems;
        private double[] _initialHeights;
        private const int _delayTime = 600;

        public List<SortModuleItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

        private DelegateCommand _startAnimation;

        public DelegateCommand StartAnimation
        {
            get
            {
                return _startAnimation ?? (_startAnimation = new DelegateCommand(async () =>
                {
                    _isCanceled = false;

                    for (int i = 0; i < AnimationItems.Count - 1; i++)
                    {
                        bool isSwapped = false;
                        for (int j = 0; j < AnimationItems.Count - i - 1; j++)
                        {
                            if (_mutex.WaitOne())
                            {
                                if (_isCanceled)
                                {
                                    ResetModuleAnimation();
                                    return;
                                }
                                else
                                {
                                    AnimationItems.ForEach(i => i.IsActive = false);
                                    if (AnimationItems[j].Height > AnimationItems[j + 1].Height)
                                    {
                                        var temp = AnimationItems[j].Height;
                                        AnimationItems[j].Height = AnimationItems[j + 1].Height;
                                        AnimationItems[j + 1].Height = temp;
                                        isSwapped = true;
                                    }
                                    AnimationItems[j].IsActive = true;
                                    AnimationItems[j + 1].IsActive = true;
                                    await Task.Delay(_delayTime);
                                }
                                _mutex.ReleaseMutex();
                            }
                        }

                        if (!isSwapped)
                        {
                            i = -1;
                            ResetModuleAnimation();
                            await Task.Delay(_delayTime);
                        }
                    }
                }));
            }
        }

        private DelegateCommand _stopAnimation;

        public DelegateCommand StopAnimation
        {
            get
            {
                return _stopAnimation ?? (_stopAnimation = new DelegateCommand(async () =>
                {
                    if (_mutex.WaitOne())
                    {
                        _isCanceled = true;
                        _mutex.ReleaseMutex();
                    }
                }));
            }
        }

        public SortModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            AnimationItems = new List<SortModuleItemViewModel>()
            {
                new SortModuleItemViewModel(30, 300, 330, 60),
                new SortModuleItemViewModel(30, 270, 330, 120),
                new SortModuleItemViewModel(30, 180, 330, 180),
                new SortModuleItemViewModel(30, 210, 330, 240),
                new SortModuleItemViewModel(30, 240, 330, 300),
            };

            _initialHeights = new double[AnimationItems.Count];
            for (int i = 0; i < _initialHeights.Length; i++)
            {
                _initialHeights[i] = AnimationItems[i].Height;
            }
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