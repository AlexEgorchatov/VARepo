using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    internal class StringMatchingModuleAnimationViewModel : BindableBase, IAnimation
    {
        private Timer _timer;

        private int i;

        private int j;

        private bool _isFirstMatch;

        private bool _isMatch;

        private List<StringMatchingModuleItemViewModel> _input;

        public List<StringMatchingModuleItemViewModel> Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
        }

        public string Pattern { get; private set; }

        private string _result;
        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
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
                    i = 0;
                    j = 0;
                }));
            }
        }

        public StringMatchingModuleAnimationViewModel()
        {
            var input = "aeedeae";
            Input = new List<StringMatchingModuleItemViewModel>(input.Select(i => new StringMatchingModuleItemViewModel(i)));
            Pattern = "ae";
            i = 0;
            j = 0;
            _isFirstMatch = true;
            _isMatch = false;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!_isMatch)
                {
                    Input[i].IsActive = false;
                }
                Input[i + j].IsActive = true;
                if (Input[i + j].Character == Pattern[j])
                {
                    j++;
                    _isMatch = true;
                }
                else
                {
                    j = 0;
                    i++;
                    _isMatch = false;
                    return;
                }

                if(j == Pattern.Length)
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
                    j = 0;
                }

                if(j == 0)
                {
                    i++;
                }
                if(i == Input.Count - Pattern.Length)
                {
                    i = 0;
                    _timer.Stop();
                }
            });
        }

        private void ResetModuleAnimation()
        {
            Input.ForEach(i => i.IsActive = false);
            Result = "";
            _isFirstMatch = true;
        }
    }
}