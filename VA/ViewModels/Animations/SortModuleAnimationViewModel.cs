using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VA.Interfaces;
using VA.Resources;

namespace VA.ViewModels
{
    public class SortModuleAnimationViewModel : BindableBase, IAnimation
    {
        #region Private Fields

        private const int _delayTime = 500;
        private readonly Mutex _mutex;
        private List<SortModuleItemViewModel> _animationItems;
        private double[] _initialHeights;
        private bool _isCanceled;
        private DelegateCommand _startAnimation;
        private DelegateCommand _stopAnimation;

        #endregion

        #region Public Properties

        public List<SortModuleItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

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
                                    AnimationItems.ForEach(i => i.State = SortItemsState.Inactive);
                                    if (AnimationItems[j].Height > AnimationItems[j + 1].Height)
                                    {
                                        var temp = AnimationItems[j].Height;
                                        AnimationItems[j].Height = AnimationItems[j + 1].Height;
                                        AnimationItems[j + 1].Height = temp;
                                        isSwapped = true;
                                    }
                                    AnimationItems[j].State = SortItemsState.Active;
                                    AnimationItems[j + 1].State = SortItemsState.Active;
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

        public DelegateCommand StopAnimation
        {
            get
            {
                return _stopAnimation ?? (_stopAnimation = new DelegateCommand(() =>
                {
                    if (_mutex.WaitOne())
                    {
                        _isCanceled = true;
                        _mutex.ReleaseMutex();
                    }
                }));
            }
        }

        #endregion

        #region Public Constructors

        public SortModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            AnimationItems = new List<SortModuleItemViewModel>()
            {
                new SortModuleItemViewModel(30, 300, 330, 60, 0),
                new SortModuleItemViewModel(30, 180, 330, 120, 0),
                new SortModuleItemViewModel(30, 210, 330, 180, 0),
                new SortModuleItemViewModel(30, 240, 330, 240, 0),
                new SortModuleItemViewModel(30, 270, 330, 300, 0),
            };

            _initialHeights = new double[AnimationItems.Count];
            for (int i = 0; i < _initialHeights.Length; i++)
            {
                _initialHeights[i] = AnimationItems[i].Height;
            }
        }

        #endregion

        #region Private Methods

        private void ResetModuleAnimation()
        {
            for (int i = 0; i < AnimationItems.Count; i++)
            {
                AnimationItems[i].Height = _initialHeights[i];
            }
            AnimationItems.ForEach(i => i.State = SortItemsState.Inactive);
        }

        #endregion
    }
}