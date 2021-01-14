using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace VA.ViewModels
{
    public class SortModulePageViewModel : BindableBase
    {
        #region Private Fields

        private DelegateCommand _applyCommand;
        private DelegateCommand _backCommand;
        private Dictionary<int, int> _correspondingSliderDelay;
        private int _delayTime;
        private string _input;
        private bool _isAnimationPaused;
        private bool _isInitialState;
        private bool _isApplied;
        private bool _isCommandPressed;
        private double _panelWidth;
        private DelegateCommand _pauseCommand;
        private Regex _regularExpression;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private int _selectedTab;
        private int _sliderValue;
        private ObservableCollection<SortModuleItemViewModel> _sortItems;
        private TaskCompletionSource<bool> _tcs;

        #endregion

        #region Public Properties

        public DelegateCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new DelegateCommand(() =>
                {
                    if (Input == null) return;
                    FillSortItems();
                    IsApplied = true;
                }));
            }
        }

        public DelegateCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new DelegateCommand(() =>
                {
                    var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                    navigationService?.GoBack();
                }));
            }
        }

        public int DelayTime
        {
            get { return _delayTime; }
            set { SetProperty(ref _delayTime, value); }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                if (_regularExpression.IsMatch(value) || value.Length == 0)
                {
                    SetProperty(ref _input, value);
                    if (IsApplied) IsApplied = false;
                }
            }
        }

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
                else
                {
                    _tcs.SetResult(false);
                    _tcs = null;
                }
            }
        }

        public bool IsApplied
        {
            get { return _isApplied; }
            set
            {
                SetProperty(ref _isApplied, value);
                if (!value)
                {
                    SortItems.Clear();
                    //IsAnimationPaused = false;
                }
            }
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

        //60 10 20 30 50 40
        public DelegateCommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand = new DelegateCommand(() =>
                {
                    if (!_isInitialState)
                    {
                        _isCommandPressed = true;
                        FillSortItems();
                    }
                    _isInitialState = false;
                    Task.Delay(_delayTime);

                    switch (SelectedTab)
                    {
                        case 0:
                            BubbleSort();
                            break;

                        case 1:
                            //QuickSort();
                            break;

                        default:
                            break;
                    }
                }));
            }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set { SetProperty(ref _selectedTab, value); }
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

        public ObservableCollection<SortModuleItemViewModel> SortItems
        {
            get { return _sortItems; }
            set { SetProperty(ref _sortItems, value); }
        }

        public List<string> SortTabs { get; set; }

        #endregion

        #region Public Constructors

        public SortModulePageViewModel()
        {
            _regularExpression = new Regex(@"^[0-9]{1,2}(\s[0-9]{1,2}){0,24}(\s)?$");
            SortTabs = new List<string>() { "Bubble Sort", "Quick Sort" };
            SortItems = new ObservableCollection<SortModuleItemViewModel>();
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
            _isInitialState = true;
        }

        #endregion

        #region Private Methods

        private async void BubbleSort()
        {
            for (int i = 0; i < SortItems.Count - 1; i++)
            {
                bool isSwapped = false;
                for (int j = 0; j < SortItems.Count - i - 1; j++)
                {
                    ResetColors();
                    if (SortItems[j].Height > SortItems[j + 1].Height)
                    {
                        var tempHeight = SortItems[j].Height;
                        SortItems[j].Height = SortItems[j + 1].Height;
                        SortItems[j + 1].Height = tempHeight;
                        var tempValue = SortItems[j].Value;
                        SortItems[j].Value = SortItems[j + 1].Value;
                        SortItems[j + 1].Value = tempValue;
                        isSwapped = true;
                    }
                    SortItems[j].IsActive = true;
                    SortItems[j + 1].IsActive = true;
                    await Task.Delay(_delayTime);

                    if (IsAnimationPaused)
                    {
                        await _tcs.Task;
                    }
                    if (_isCommandPressed)
                    {
                        _isInitialState = true;
                        return;
                    }
                }

                if (!isSwapped)
                {
                    break;
                }
            }

            ResetColors();
        }

        private void QuickSort()
        {
        }

        private void FillSortItems()
        {
            SortItems.Clear();
            string[] numbers = Input.Split(' ');
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] != "")
                {
                    SortItems.Add(new SortModuleItemViewModel(30, Convert.ToInt32(numbers[i]) * 3, 270, 15 + 45 * i, Convert.ToInt32(numbers[i])));
                }
            }
            PanelWidth = 15 + (45 * numbers.Length);
            _isInitialState = true;
        }

        private void ResetColors()
        {
            for (int k = 0; k < SortItems.Count; k++)
            {
                SortItems[k].IsActive = false;
            }
        }

        #endregion
    }
}