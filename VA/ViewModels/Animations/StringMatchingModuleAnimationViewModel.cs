using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    internal class StringMatchingModuleAnimationViewModel : BindableBase, IAnimation
    {
        #region Private Fields

        private const int _delayTime = 500;
        private readonly Mutex _mutex;
        private List<StringMatchingModuleCharViewModel> _input;
        private bool _isCanceled;
        private bool _isFirstMatch;
        private string _result;
        private DelegateCommand _startAnimation;
        private DelegateCommand _stopAnimation;

        #endregion

        #region Private Properties

        private List<int> _activeItems { get; set; }

        #endregion

        #region Public Properties

        public List<StringMatchingModuleCharViewModel> Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        public string Pattern { get; private set; }

        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

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

        #endregion

        #region Public Constructors

        public StringMatchingModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            var input = "eeaea";
            Input = new List<StringMatchingModuleCharViewModel>(input.Select(i => new StringMatchingModuleCharViewModel(i)));
            _activeItems = new List<int>();
            Pattern = "ae";
            _isFirstMatch = true;
        }

        #endregion

        #region Private Methods

        private void ResetActiveItems()
        {
            for (int i = 0; i < _activeItems.Count; i++)
            {
                Input[_activeItems[i]].IsActive = false;
            }
            _activeItems.Clear();
        }

        private void ResetModuleAnimation()
        {
            ResetActiveItems();
            Result = "";
            _isFirstMatch = true;
        }

        #endregion
    }
}