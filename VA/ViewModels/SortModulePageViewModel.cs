using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
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
        private bool _isAnimationRunning;
        private bool _isApplied;
        private double _panelWidth;
        private DelegateCommand _pauseCommand;
        private Regex _regularExpression;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private int _selectedTab;
        private int _sliderValue;
        private ObservableCollection<SortModuleItemViewModel> _sortItems;
        private TaskCompletionSource<bool> _tcs;
        private int _quickI;

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
                }, () => !string.IsNullOrEmpty(Input)));
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
                    IsAnimationRunning = false;
                    IsAnimationPaused = false;
                    if (IsApplied) IsApplied = false;
                }
                ApplyCommand.RaiseCanExecuteChanged();
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

        private Duration _animationDuration;
        public Duration AnimationDuration
        {
            get { return _animationDuration; }
            set { SetProperty(ref _animationDuration, value); }
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
                return _runCommand ?? (_runCommand = new DelegateCommand(() =>
                {
                    IsAnimationRunning = true;
                    switch (SelectedTab)
                    {
                        case 0:
                            BubbleSort();
                            break;

                        case 1:
                            //QuickSort(0, SortItems.Count - 1);
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
            AnimationDuration = new Duration(TimeSpan.FromMilliseconds(1000));
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
                }

                if (!isSwapped)
                {
                    break;
                }
            }

            ResetColors();
            IsAnimationRunning = false;
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
        }

        //Flashsort
        private void QuickSort(int low, int high)
        {
            if(low < high)
            {
                Partition(low, high);
                QuickSort(low, _quickI - 1);
                QuickSort(_quickI + 1, high);
            }
        }

        private async void Partition(int low, int high)
        {
            int pivot = SortItems[SortItems.Count - 1].Value;
            _quickI = low - 1;

            for (int j = low; j < high; j++)
            {
                ResetColors();
                if (SortItems[j].Value < pivot)
                {
                    _quickI++;
                    var tempHeight = SortItems[_quickI].Height;
                    SortItems[_quickI].Height = SortItems[j].Height;
                    SortItems[j].Height = tempHeight;
                    var tempValue = SortItems[_quickI].Value;
                    SortItems[_quickI].Value = SortItems[j].Value;
                    SortItems[j].Value = tempValue;
                }
                SortItems[_quickI].IsActive = true;
                SortItems[j].IsActive = true;
                await Task.Delay(_delayTime);
            }

            var temp = SortItems[_quickI + 1].Height;
            SortItems[_quickI + 1].Height = SortItems[high].Height;
            SortItems[high].Height = temp;
            var temp1 = SortItems[_quickI + 1].Value;
            SortItems[_quickI + 1].Value = SortItems[high].Value;
            SortItems[high].Value = temp1;

            SortItems[_quickI + 1].IsActive = true;
            SortItems[high].IsActive = true;
            await Task.Delay(_delayTime);
            ResetColors();

            _quickI++;
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