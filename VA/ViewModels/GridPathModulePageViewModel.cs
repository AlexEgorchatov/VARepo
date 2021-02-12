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
        private GridPathModuleCellViewModel _destination;
        private List<GridPathModuleCellViewModel> _grid;
        private bool _isAnimationPaused;
        private bool _isAnimationRunning;
        private double _panelWidth;
        private DelegateCommand _pauseCommand;
        private DelegateCommand _resetAnimationCommand;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private string _selectedTab;
        private DelegateCommand<GridPathModuleCellViewModel> _setDestinationCommand;
        private DelegateCommand<GridPathModuleCellViewModel> _setStartCommand;
        private int _sliderValue;
        private GridPathModuleCellViewModel _start;
        private TaskCompletionSource<bool> _tcs;

        #endregion

        #region Public Fields

        public static double Columns = 45;
        public static double Rows = 18;

        #endregion

        #region Public Properties

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

        public GridPathModuleCellViewModel Destination
        {
            get { return _destination; }
            set
            {
                SetProperty(ref _destination, value);
                if (Destination != null)
                {
                    Destination.ColorType = ColorType.Destination;
                }
            }
        }

        public List<GridPathModuleCellViewModel> Grid
        {
            get { return _grid; }
            set { SetProperty(ref _grid, value); }
        }

        public List<string> GridPathTabs { get; set; }

        public Queue<GridPathModuleCellViewModel> GridQueue { get; set; }

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

        public DelegateCommand ResetAnimationCommand
        {
            get
            {
                return _resetAnimationCommand ?? (_resetAnimationCommand = new DelegateCommand(() =>
                {
                    GridQueue.Clear();
                    for (int i = 0; i < Grid.Count; i++)
                    {
                        Grid[i].ColorType = ColorType.Neutral;
                        Grid[i].Distance = -1;
                        Grid[i].IsVisited = false;
                    }
                    Start = null;
                    Destination = null;
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
                        case "Breadth First Search":
                            await BFSAlgorithm();
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

        public DelegateCommand<GridPathModuleCellViewModel> SetDestinationCommand
        {
            get
            {
                return _setDestinationCommand ?? (_setDestinationCommand = new DelegateCommand<GridPathModuleCellViewModel>(e =>
                {
                    if (Destination != null)
                    {
                        Destination.ColorType = ColorType.Neutral;
                        Destination = null;
                    }
                    if (Start == e)
                    {
                        Start = null;
                    }

                    Destination = e;
                    if (Destination != null && Start != null)
                    {
                        RunCommand.Execute();
                    }
                }));
            }
        }

        public DelegateCommand<GridPathModuleCellViewModel> SetStartCommand
        {
            get
            {
                return _setStartCommand ?? (_setStartCommand = new DelegateCommand<GridPathModuleCellViewModel>(e =>
                {
                    if (Start != null)
                    {
                        Start.ColorType = ColorType.Neutral;
                        Start = null;
                    }
                    if (Destination == e)
                    {
                        Destination = null;
                    }

                    Start = e;
                    if (Destination != null && Start != null)
                    {
                        RunCommand.Execute();
                    }
                }));
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

        public GridPathModuleCellViewModel Start
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value);
                if (Start != null)
                {
                    Start.ColorType = ColorType.Start;
                }
            }
        }

        #endregion

        #region Public Constructors

        public GridPathModulePageViewModel()
        {
            GridPathTabs = new List<string>() { "Breadth First Search" };
            Grid = new List<GridPathModuleCellViewModel>();
            GridQueue = new Queue<GridPathModuleCellViewModel>();
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

        #region Private Methods

        private async Task BFSAlgorithm()
        {
            GridQueue.Enqueue(Start);
            Start.IsVisited = true;
            int currentDistance = -1;

            while (GridQueue.Count != 0)
            {
                GridPathModuleCellViewModel cell = GridQueue.Peek();
                GridQueue.Dequeue();

                //moving up
                if (cell.Row - 1 >= 0 && !Grid[Grid.IndexOf(cell) - (int)Columns].IsVisited)
                {
                    Grid[Grid.IndexOf(cell) - (int)Columns].IsVisited = true;
                    Grid[Grid.IndexOf(cell) - (int)Columns].Distance = cell.Distance + 1;
                    if (Grid[Grid.IndexOf(cell) - (int)Columns] == Destination)
                    {
                        await Task.Delay(_delayTime);
                        DrawPath(Destination, Start);
                        /*await Task.Delay(_delayTime);
                        ResetModuleAnimation();
                        GridQueue.Enqueue(Start);
                        Start.IsVisited = true;
                        currentDistance = 0;
                        await Task.Delay(_delayTime);*/
                        break;
                    }
                    if (cell.Distance - currentDistance == 1)
                    {
                        currentDistance = cell.Distance;
                        await Task.Delay(_delayTime);
                    }
                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) - (int)Columns]);
                    Grid[Grid.IndexOf(cell) - (int)Columns].ColorType = ColorType.Search;
                }

                //movding down
                if (cell.Row + 1 < (int)Rows && !Grid[Grid.IndexOf(cell) + (int)Columns].IsVisited)
                {
                    Grid[Grid.IndexOf(cell) + (int)Columns].IsVisited = true;
                    Grid[Grid.IndexOf(cell) + (int)Columns].Distance = cell.Distance + 1;
                    if (Grid[Grid.IndexOf(cell) + (int)Columns] == Destination)
                    {
                        await Task.Delay(_delayTime);
                        DrawPath(Destination, Start);
                        /*await Task.Delay(_delayTime);
                        ResetModuleAnimation();
                        GridQueue.Enqueue(Start);
                        Start.IsVisited = true;
                        currentDistance = 0;
                        await Task.Delay(_delayTime);*/
                        break;
                    }
                    if (cell.Distance - currentDistance == 1)
                    {
                        currentDistance = cell.Distance;
                        await Task.Delay(_delayTime);
                    }
                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) + (int)Columns]);
                    Grid[Grid.IndexOf(cell) + (int)Columns].ColorType = ColorType.Search;
                }

                //moving left
                if (cell.Column - 1 >= 0 && !Grid[Grid.IndexOf(cell) - 1].IsVisited)
                {
                    Grid[Grid.IndexOf(cell) - 1].IsVisited = true;
                    Grid[Grid.IndexOf(cell) - 1].Distance = cell.Distance + 1;
                    if (Grid[Grid.IndexOf(cell) - 1] == Destination)
                    {
                        await Task.Delay(_delayTime);
                        DrawPath(Destination, Start);
                        /*await Task.Delay(_delayTime);
                        ResetModuleAnimation();
                        GridQueue.Enqueue(Start);
                        Start.IsVisited = true;
                        currentDistance = 0;
                        await Task.Delay(_delayTime);*/
                        break;
                    }
                    if (cell.Distance - currentDistance == 1)
                    {
                        currentDistance = cell.Distance;
                        await Task.Delay(_delayTime);
                    }
                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) - 1]);
                    Grid[Grid.IndexOf(cell) - 1].ColorType = ColorType.Search;
                }

                //moving right
                if (cell.Column + 1 < (int)Columns && !Grid[Grid.IndexOf(cell) + 1].IsVisited)
                {
                    Grid[Grid.IndexOf(cell) + 1].IsVisited = true;
                    Grid[Grid.IndexOf(cell) + 1].Distance = cell.Distance + 1;
                    if (Grid[Grid.IndexOf(cell) + 1] == Destination)
                    {
                        await Task.Delay(_delayTime);
                        DrawPath(Destination, Start);
                        /*await Task.Delay(_delayTime);
                        ResetModuleAnimation();
                        GridQueue.Enqueue(Start);
                        Start.IsVisited = true;
                        currentDistance = 0;
                        await Task.Delay(_delayTime);*/
                        break;
                    }
                    if (cell.Distance - currentDistance == 1)
                    {
                        currentDistance = cell.Distance;
                        await Task.Delay(_delayTime);
                    }
                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) + 1]);
                    Grid[Grid.IndexOf(cell) + 1].ColorType = ColorType.Search;
                }
            }
        }

        private void DrawPath(GridPathModuleCellViewModel destination, GridPathModuleCellViewModel start)
        {
            GridPathModuleCellViewModel cell = destination;

            while (cell.Distance != 0)
            {
                if (Grid.IndexOf(cell) - (int)Columns >= 0 && Grid[Grid.IndexOf(cell) - (int)Columns].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) - (int)Columns];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (Grid.IndexOf(cell) + (int)Columns < Rows * Columns && Grid[Grid.IndexOf(cell) + (int)Columns].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) + (int)Columns];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (/*Grid.IndexOf(cell) - 1 >= 0 && */Grid[Grid.IndexOf(cell) - 1].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) - 1];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (/*Grid.IndexOf(cell) + 1 < Columns && */Grid[Grid.IndexOf(cell) + 1].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) + 1];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
            }
        }

        #endregion

        #region Public Methods

        public void SetCellSize(double size)
        {
            PanelWidth = size * Columns + GridPathModuleCellViewModel.Margin.Left * 2;
            Grid.ForEach(i => i.SetSize(size));
        }

        #endregion
    }
}