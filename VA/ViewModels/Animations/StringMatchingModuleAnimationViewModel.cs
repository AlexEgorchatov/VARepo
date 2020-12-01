using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    class StringMatchingModuleAnimationViewModel : BindableBase, IAnimation
    {
        private Timer _timer;

        private string _input;
        public string Input
        {
            get { return _input; }
            set { SetProperty(ref _input, value); }
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
                }));
            }
        }

        public StringMatchingModuleAnimationViewModel()
        {

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                
            });
        }

        private void ResetModuleAnimation()
        {

        }
    }
}
