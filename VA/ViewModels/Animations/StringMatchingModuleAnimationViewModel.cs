using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    internal class StringMatchingModuleAnimationViewModel : BindableBase, IAnimation
    {
        private bool _isCanceled;
        private const int _delayTime = 600;
        private readonly Mutex _mutex;

        private bool _isFirstMatch;

        private List<StringMatchingModuleCharViewModel> _input;

        public List<StringMatchingModuleCharViewModel> Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        private List<int> _activeItems { get; set; }

        public string Pattern { get; private set; }

        private string _result;
        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        private DelegateCommand _startAnimation;

        public DelegateCommand StartAnimation
        {
            get
            {
                return _startAnimation ?? (_startAnimation = new DelegateCommand(async () =>
                {
                    _isCanceled = false;

                    for (int i = 0; i <= Input.Count - Pattern.Length; i++)
                    {
                        for (int j = 0; j < Pattern.Length; j++)
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

                                    if (j == 0)
                                    {
                                        ResetActiveItems();
                                    }

                                    Input[i + j].IsActive = true;
                                    _activeItems.Add(i + j);
                                    if (Input[i + j].Character != Pattern[j])
                                    {
                                        await Task.Delay(_delayTime);
                                        break;
                                    }

                                    if (j == Pattern.Length - 1)
                                    {
                                        if (_isFirstMatch)
                                        {
                                            Result += i;
                                            _isFirstMatch = false;
                                        }
                                        else
                                        {
                                            Result = Result + ", " + i;
                                        }
                                    }
                                    await Task.Delay(_delayTime);
                                }

                                _mutex.ReleaseMutex();
                            }
                        }
                        if (i == (Input.Count - Pattern.Length))
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

        public StringMatchingModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            var input = "eeaea";
            Input = new List<StringMatchingModuleCharViewModel>(input.Select(i => new StringMatchingModuleCharViewModel(i)));
            _activeItems = new List<int>();
            Pattern = "ae";
            _isFirstMatch = true;
        }

        private void ResetModuleAnimation()
        {
            ResetActiveItems();
            Result = "";
            _isFirstMatch = true;
        }

        private void ResetActiveItems()
        {
            for (int i = 0; i < _activeItems.Count; i++)
            {
                Input[_activeItems[i]].IsActive = false;
            }
            _activeItems.Clear();
        }
    }
}