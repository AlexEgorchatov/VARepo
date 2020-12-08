using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    public class GridPathModuleAnimationViewModel : BindableBase, IAnimation
    {
        private bool _isCanceled;
        private readonly Mutex _mutex;
        private const int _delayTime = 600;
        private List<GridPathModuleCellViewModel> _grid;
        public List<GridPathModuleCellViewModel> Grid
        {
            get { return _grid; }
            set { SetProperty(ref _grid, value); }
        }

        private DelegateCommand _startAnimation;
        public DelegateCommand StartAnimation
        {
            get
            {
                return _startAnimation ?? (_startAnimation = new DelegateCommand(async () =>
                {

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

                }));
            }
        }

        public GridPathModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            Grid = new List<GridPathModuleCellViewModel>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Grid.Add(new GridPathModuleCellViewModel(60, 60, 30 + 60 * i, 15 + 60 * j));
                }
            }

        }

        private void ResetModuleAnimation()
        {
        }
    }
}
