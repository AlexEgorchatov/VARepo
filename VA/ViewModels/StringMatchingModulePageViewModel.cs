using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using VA.ViewModels.Animations;

namespace VA.ViewModels
{
    public class StringMatchingModulePageViewModel : BindableBase
    {
        #region Private Fields

        private Duration _animationDuration;
        private DelegateCommand _applyCommand;
        private DelegateCommand _backCommand;
        private Dictionary<int, int> _correspondingSliderDelay;
        private int _delayTime;
        private string _input;
        private bool _isAnimationPaused;
        private bool _isAnimationRunning;
        private bool _isApplied;
        private string _pattern;
        private DelegateCommand _pauseCommand;
        private Regex _regularExpression;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private string _selectedTab;
        private int _sliderValue;
        private ObservableCollection<StringMatchingModuleCharViewModel> _stringMatchingInput;
        private ObservableCollection<StringMatchingModuleCharViewModel> _stringMatchingPattern;
        private TaskCompletionSource<bool> _tcs;

        #endregion

        #region Public Properties

        public Duration AnimationDuration
        {
            get { return _animationDuration; }
            set { SetProperty(ref _animationDuration, value); }
        }

        public DelegateCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new DelegateCommand(() =>
                {
                    if (Input == null || Pattern == null) return;
                    FillSortItems();
                    IsApplied = true;
                }, () => !string.IsNullOrEmpty(Input) && !string.IsNullOrEmpty(Pattern)));
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
                if ((_regularExpression.IsMatch(value) || value.Length == 0) && value.Length <= 70)
                {
                    SetProperty(ref _input, value);
                    IsAnimationRunning = false;
                    IsAnimationPaused = false;
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
                    StringMatchingInput.Clear();
                }
            }
        }

        public string Pattern
        {
            get { return _pattern; }
            set
            {
                if ((_regularExpression.IsMatch(value) || value.Length == 0) && value.Length <= 70)
                {
                    SetProperty(ref _pattern, value);
                    IsAnimationRunning = false;
                    IsAnimationPaused = false;
                }
                ApplyCommand.RaiseCanExecuteChanged();
            }
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
                    await Task.Delay(_delayTime);
                    /*switch (SelectedTab)
                    {
                        case "Bubble Sort":
                            await BubbleSort();
                            break;

                        case "Quick Sort":
                            await QuickSort(0, SortItems.Count - 1);
                            break;

                        default:
                            break;
                    }*/
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
                if (_delayTime < 400)
                {
                    AnimationDuration = new Duration(TimeSpan.FromMilliseconds(_delayTime));
                }
                else
                {
                    AnimationDuration = new Duration(TimeSpan.FromMilliseconds(400));
                }
            }
        }

        public ObservableCollection<StringMatchingModuleCharViewModel> StringMatchingInput
        {
            get { return _stringMatchingInput; }
            set { SetProperty(ref _stringMatchingInput, value); }
        }

        public ObservableCollection<StringMatchingModuleCharViewModel> StringMatchingPattern
        {
            get { return _stringMatchingPattern; }
            set { SetProperty(ref _stringMatchingPattern, value); }
        }

        public List<string> StringMatchingTabs { get; set; }

        #endregion

        #region Public Constructors

        public StringMatchingModulePageViewModel()
        {
            _regularExpression = new Regex(@"^[a-z]+(\s[a-z]+)*(\s)?$");
            StringMatchingTabs = new List<string>() { "Naive Algorithm", "Knuth Morris Pratt" };
            StringMatchingInput = new ObservableCollection<StringMatchingModuleCharViewModel>();
            StringMatchingPattern = new ObservableCollection<StringMatchingModuleCharViewModel>();
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
            AnimationDuration = new Duration(TimeSpan.FromMilliseconds(400));
        }

        #endregion

        #region Private Methods

        private void FillSortItems()
        {
            StringMatchingInput.Clear();
            StringMatchingPattern.Clear();

            for (int i = 0; i < Input.Length; i++)
            {
                StringMatchingInput.Add(new StringMatchingModuleCharViewModel(Input[i]));
            }

            for (int i = 0; i < Pattern.Length; i++)
            {
                StringMatchingPattern.Add(new StringMatchingModuleCharViewModel(Pattern[i]));
            }
        }

        private void ResetColors()
        {
            /*for (int k = 0; k < SortItems.Count; k++)
            {
                SortItems[k].State = SortItemsState.Inactive;
            }*/
        }

        #endregion
    }
}