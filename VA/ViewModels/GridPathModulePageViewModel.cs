using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using VA.ViewModels.Animations;

namespace VA.ViewModels
{
    public class GridPathModulePageViewModel : BindableBase
    {
        #region Private Fields

        private DelegateCommand _backCommand;
        private Dictionary<int, int> _correspondingSliderDelay;
        private int _delayTime;
        private List<GridPathModuleCellViewModel> _grid;
        private bool _isAnimationPaused;
        private bool _isAnimationRunning;
        private double _panelWidth;
        private DelegateCommand _pauseCommand;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private string _selectedTab;
        private int _sliderValue;
        private TaskCompletionSource<bool> _tcs;

        #endregion

        #region Public Properties

        public static double Rows = 18;
        public static double Columns = 45;

        public DelegateCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new DelegateCommand(() =>
                {
                    //ResetAnimation();
                    var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                    navigationService?.GoBack();
                }));
            }
        }

        public List<GridPathModuleCellViewModel> Grid
        {
            get { return _grid; }
            set { SetProperty(ref _grid, value); }
        }

        public List<string> GridPathTabs { get; set; }

        public bool IsAnimationPaused
        {
            get { return _isAnimationPaused; }
            set
            {
                SetProperty(ref _isAnimationPaused, value);
                if (value)
                {
                    _tcs = new TaskCompletionSource<bool>();
                }
                else if (_tcs != null)
                {
                    _tcs.SetResult(false);
                    _tcs = null;
                }
            }
        }

        public bool IsAnimationRunning
        {
            get { return _isAnimationRunning; }
            set { SetProperty(ref _isAnimationRunning, value); }
        }

        public double PanelWidth
        {
            get { return _panelWidth; }
            set { SetProperty(ref _panelWidth, value); }
        }

        public DelegateCommand PauseCommand
        {
            get
            {
                return _pauseCommand ?? (_pauseCommand = new DelegateCommand(() =>
                {
                    IsAnimationPaused = true;
                }));
            }
        }

        public DelegateCommand ResumeCommand
        {
            get
            {
                return _resumeCommand ?? (_resumeCommand = new DelegateCommand(() =>
                {
                    IsAnimationPaused = false;
                }));
            }
        }

        public DelegateCommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand = new DelegateCommand(async () =>
                {
                    IsAnimationRunning = true;
                    switch (SelectedTab)
                    {
                        case "Naive Algorithm":
                            //await NaiveAlgorithm();
                            break;

                        case "Knuth Morris Pratt":
                            //await KnuthMorrisPrattAlgorithm();
                            break;

                        default:
                            break;
                    }
                    IsAnimationRunning = false;
                }));
            }
        }

        public string SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (IsAnimationRunning) return;
                SetProperty(ref _selectedTab, value);
            }
        }

        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                SetProperty(ref _sliderValue, value);
                _delayTime = _correspondingSliderDelay[value];
            }
        }

        #endregion

        #region Public Constructors

        public GridPathModulePageViewModel()
        {
            GridPathTabs = new List<string>() { "Breadth First Search" };
            Grid = new List<GridPathModuleCellViewModel>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Grid.Add(new GridPathModuleCellViewModel(i, j, 40));
                }
            }
            _correspondingSliderDelay = new Dictionary<int, int>()
            {
                { 1, 5000},
                { 2, 4000},
                { 3, 3000},
                { 4, 2000},
                { 5, 1000},
                { 6, 500},
                { 7, 333},
                { 8, 250},
                { 9, 200},
            };
            SliderValue = 5;
        }

        #endregion

        public void SetCellSize(double size)
        {
            PanelWidth = size * Columns + GridPathModuleCellViewModel.Margin.Left * 2;
            Grid.ForEach(i => i.SetSize(size));
        }
    }
}