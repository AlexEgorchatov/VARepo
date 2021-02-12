using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VA.Interfaces;

namespace VA.ViewModels.Animations
{
    public class GridPathModuleAnimationViewModel : BindableBase, IAnimation
    {
        #region Private Fields

        private const int _delayTime = 500;
        private const int _gridSize = 5;
        private readonly Mutex _mutex;
        private GridPathModuleCellViewModel _destination;
        private List<GridPathModuleCellViewModel> _grid;
        private bool _isCanceled;
        private GridPathModuleCellViewModel _start;
        private DelegateCommand _startAnimation;
        private DelegateCommand _stopAnimation;

        #endregion

        #region Public Properties

        public GridPathModuleCellViewModel Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                Destination.ColorType = ColorType.Destination;
            }
        }

        public List<GridPathModuleCellViewModel> Grid
        {
            get { return _grid; }
            set { SetProperty(ref _grid, value); }
        }

        public Queue<GridPathModuleCellViewModel> GridQueue { get; set; }

        public GridPathModuleCellViewModel Start
        {
            get { return _start; }
            set
            {
                _start = value;
                Start.ColorType = ColorType.Start;
            }
        }

        public DelegateCommand StartAnimation
        {
            get
            {
                return _startAnimation ?? (_startAnimation = new DelegateCommand(async () =>
                {
                    _isCanceled = false;

                    GridQueue.Enqueue(Start);
                    Start.IsVisited = true;
                    int currentDistance = -1;

                    while (GridQueue.Count != 0)
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
                                GridPathModuleCellViewModel cell = GridQueue.Peek();
                                GridQueue.Dequeue();

                                //moving up
                                if (cell.Row - 1 >= 0 && !Grid[Grid.IndexOf(cell) - _gridSize].IsVisited)
                                {
                                    Grid[Grid.IndexOf(cell) - _gridSize].IsVisited = true;
                                    Grid[Grid.IndexOf(cell) - _gridSize].Distance = cell.Distance + 1;
                                    if (Grid[Grid.IndexOf(cell) - _gridSize] == Destination)
                                    {
                                        await Task.Delay(_delayTime);
                                        DrawPath(Destination, Start);
                                        await Task.Delay(_delayTime);
                                        ResetModuleAnimation();
                                        GridQueue.Enqueue(Start);
                                        Start.IsVisited = true;
                                        currentDistance = -1;
                                        await Task.Delay(_delayTime);
                                        continue;
                                    }
                                    if (cell.Distance - currentDistance == 1)
                                    {
                                        currentDistance = cell.Distance;
                                        await Task.Delay(_delayTime);
                                    }
                                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) - _gridSize]);
                                    Grid[Grid.IndexOf(cell) - _gridSize].ColorType = ColorType.Search;
                                }

                                //movding down
                                if (cell.Row + 1 < _gridSize && !Grid[Grid.IndexOf(cell) + _gridSize].IsVisited)
                                {
                                    Grid[Grid.IndexOf(cell) + _gridSize].IsVisited = true;
                                    Grid[Grid.IndexOf(cell) + _gridSize].Distance = cell.Distance + 1;
                                    if (Grid[Grid.IndexOf(cell) + _gridSize] == Destination)
                                    {
                                        await Task.Delay(_delayTime);
                                        DrawPath(Destination, Start);
                                        await Task.Delay(_delayTime);
                                        ResetModuleAnimation();
                                        GridQueue.Enqueue(Start);
                                        Start.IsVisited = true;
                                        currentDistance = -1;
                                        await Task.Delay(_delayTime);
                                        continue;
                                    }
                                    if (cell.Distance - currentDistance == 1)
                                    {
                                        currentDistance = cell.Distance;
                                        await Task.Delay(_delayTime);
                                    }
                                    GridQueue.Enqueue(Grid[Grid.IndexOf(cell) + _gridSize]);
                                    Grid[Grid.IndexOf(cell) + _gridSize].ColorType = ColorType.Search;
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
                                        await Task.Delay(_delayTime);
                                        ResetModuleAnimation();
                                        GridQueue.Enqueue(Start);
                                        Start.IsVisited = true;
                                        currentDistance = -1;
                                        await Task.Delay(_delayTime);
                                        continue;
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
                                if (cell.Column + 1 < _gridSize && !Grid[Grid.IndexOf(cell) + 1].IsVisited)
                                {
                                    Grid[Grid.IndexOf(cell) + 1].IsVisited = true;
                                    Grid[Grid.IndexOf(cell) + 1].Distance = cell.Distance + 1;
                                    if (Grid[Grid.IndexOf(cell) + 1] == Destination)
                                    {
                                        await Task.Delay(_delayTime);
                                        DrawPath(Destination, Start);
                                        await Task.Delay(_delayTime);
                                        ResetModuleAnimation();
                                        GridQueue.Enqueue(Start);
                                        Start.IsVisited = true;
                                        currentDistance = -1;
                                        await Task.Delay(_delayTime);
                                        continue;
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

                            _mutex.ReleaseMutex();
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

        public GridPathModuleAnimationViewModel()
        {
            _mutex = new Mutex();
            GridQueue = new Queue<GridPathModuleCellViewModel>();
            Grid = new List<GridPathModuleCellViewModel>();
            for (int i = 0; i < _gridSize; i++)
            {
                for (int j = 0; j < _gridSize; j++)
                {
                    Grid.Add(new GridPathModuleCellViewModel(i, j, 60));
                }
            }

            Start = Grid[15];
            Destination = Grid[8];
        }

        #endregion

        #region Private Methods

        private void DrawPath(GridPathModuleCellViewModel destination, GridPathModuleCellViewModel start)
        {
            GridPathModuleCellViewModel cell = destination;

            while (cell.Distance != 0)
            {
                if (Grid[Grid.IndexOf(cell) - _gridSize].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) - _gridSize];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (Grid[Grid.IndexOf(cell) + _gridSize].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) + _gridSize];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (Grid[Grid.IndexOf(cell) - 1].Distance == cell.Distance - 1)
                {
                    cell = Grid[Grid.IndexOf(cell) - 1];
                    if (cell == start)
                    {
                        break;
                    }
                    cell.ColorType = ColorType.Path;
                }
                else if (Grid[Grid.IndexOf(cell) + 1].Distance == cell.Distance - 1)
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

        private void ResetModuleAnimation()
        {
            GridQueue.Clear();
            for (int i = 0; i < Grid.Count; i++)
            {
                Grid[i].ColorType = ColorType.Neutral;
                Grid[i].Distance = -1;
                Grid[i].IsVisited = false;
            }
            Start = Grid[15];
            Destination = Grid[8];
        }

        #endregion
    }
}