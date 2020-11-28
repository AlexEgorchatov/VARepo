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
        private int i;
        private int j;
        private int itemsCount;

        public List<AnimationItemViewModel> AnimationItems
        {
            get { return _animationItems; }
            set { SetProperty(ref _animationItems, value); }
        }

        public AnimationViewModel()
        {
            AnimationItems = new List<AnimationItemViewModel>()
            {
                new AnimationItemViewModel(30, 270, 330 - 270, 30, 3),
                new AnimationItemViewModel(30, 300, 330 - 300, 90, 4),
                new AnimationItemViewModel(30, 180, 330 - 180, 150, 0),
                new AnimationItemViewModel(30, 240, 330 - 240, 210, 2),
                new AnimationItemViewModel(30, 210, 330 - 210, 270, 1),
            };
        }

        public void StartTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Start();
            _timer.Elapsed += TimerElapsed;

            itemsCount = AnimationItems.Count;
            i = 0;
            j = 0;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //MessageBox.Show($"{AnimationItems[j].Value} {AnimationItems[j + 1].Value} i: {i} j: {j} j: {j + 1}");
            if (AnimationItems[j].Value > AnimationItems[j + 1].Value)
            {
                var tempValue = AnimationItems[j].Value;
                AnimationItems[j].Value = AnimationItems[j + 1].Value;
                AnimationItems[j + 1].Value = tempValue;
                /*AnimationItems[j].Height = AnimationItems[j + 1].Height;
                AnimationItems[j + 1].Height = tempHeight;
                AnimationItems[j].Top = 330 - AnimationItems[j].Height;
                AnimationItems[j + 1].Top = 330 - AnimationItems[j + 1].Height;
                var temp = AnimationItems[j];
                AnimationItems[j] = AnimationItems[j + 1];
                AnimationItems[j + 1] = temp;*/
            }

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
                //i = 0;
                _timer.Stop();
            }
        }
    }
}