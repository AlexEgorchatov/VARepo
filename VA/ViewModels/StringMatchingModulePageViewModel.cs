using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using VA.Resources;
using VA.ViewModels.Animations;

namespace VA.ViewModels
{
    public class StringMatchingModulePageViewModel : BindableBase
    {
        #region Private Fields

        private const int _maxElements = 80;
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
        private string _result;
        private DelegateCommand _resumeCommand;
        private DelegateCommand _runCommand;
        private string _selectedTab;
        private int _sliderValue;
        private ObservableCollection<StringMatchingModuleCharViewModel> _stringMatchingInput;
        private ObservableCollection<StringMatchingModuleCharViewModel> _stringMatchingPattern;
        private TaskCompletionSource<bool> _tcs;

        #endregion

        #region Public Properties

        public DelegateCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new DelegateCommand(() =>
                {
                    FillSortItems();
                    Result = "";
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
                    ResetAnimation();
                    var navigationService = Application.Current.Properties["NavigationService"] as NavigationService;
                    navigationService?.GoBack();
                }));
            }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                if ((_regularExpression.IsMatch(value) || value.Length == 0) && value.Length <= _maxElements)
                {
                    SetProperty(ref _input, value);
                    ResetAnimation();
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
                    StringMatchingPattern.Clear();
                }
            }
        }

        public string Pattern
        {
            get { return _pattern; }
            set
            {
                if ((_regularExpression.IsMatch(value) || value.Length == 0) && value.Length <= _maxElements)
                {
                    SetProperty(ref _pattern, value);
                    ResetAnimation();
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

        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
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
                    Result = "";
                    switch (SelectedTab)
                    {
                        case "Naive Algorithm":
                            await NaiveAlgorithm();
                            break;

                        case "Knuth Morris Pratt":
                            await KnuthMorrisPrattAlgorithm();
                            break;

                        default:
                            break;
                    }
                    if (Result == "") Result = "No match is found";
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
        }

        #endregion

        #region Private Methods

        private void FillSortItems()
        {
            StringMatchingInput.Clear();
            StringMatchingPattern.Clear();

            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == ' ')
                {
                    StringMatchingInput.Add(new StringMatchingModuleCharViewModel('_') { State = StringMatchingItemsState.Space });
                }
                else
                {
                    StringMatchingInput.Add(new StringMatchingModuleCharViewModel(Input[i]));
                }
            }

            for (int i = 0; i < Pattern.Length; i++)
            {
                if (Pattern[i] == ' ')
                {
                    StringMatchingPattern.Add(new StringMatchingModuleCharViewModel('_') { State = StringMatchingItemsState.Space });
                }
                else
                {
                    StringMatchingPattern.Add(new StringMatchingModuleCharViewModel(Pattern[i]));
                }
            }
        }

        //hslssdfsdsksfdsdss
        private async Task KnuthMorrisPrattAlgorithm()
        {
            int[] lps = new int[StringMatchingPattern.Count];
            int j = 0, i = 0;
            bool isResultFound = false, isMatch = false, isFirstMatch = true;

            LongerProperPrefixSuffix(lps);

            while (i < StringMatchingInput.Count)
            {
                StringMatchingInput[i].State = StringMatchingItemsState.Active;
                StringMatchingPattern[j].State = StringMatchingItemsState.Active;
                if (StringMatchingPattern[j].Character == StringMatchingInput[i].Character)
                {
                    j++;
                    i++;
                    isMatch = true;
                }

                if (j == StringMatchingPattern.Count)
                {
                    if (isFirstMatch)
                    {
                        Result = (i - j).ToString();
                        isFirstMatch = false;
                    }
                    else
                    {
                        Result = Result + ", " + (i - j).ToString();
                    }

                    j = lps[j - 1];
                    isResultFound = true;
                }
                await Task.Delay(_delayTime);
                await PauseAnimationIfRequired();

                if (isResultFound)
                {
                    ResetColors();
                }

                if (i < StringMatchingInput.Count && StringMatchingPattern[j].Character != StringMatchingInput[i].Character && !isResultFound)
                {
                    if (isMatch)
                    {
                        StringMatchingInput[i].State = StringMatchingItemsState.Active;
                        StringMatchingPattern[j].State = StringMatchingItemsState.Active;
                        await Task.Delay(_delayTime);
                        await PauseAnimationIfRequired();
                    }

                    if (j != 0)
                    {
                        j = lps[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                    ResetColors();
                }

                if (isResultFound) isResultFound = false;
                if (isMatch) isMatch = false;
            }

            ResetColors();
        }

        private void LongerProperPrefixSuffix(int[] lps)
        {
            int length = 0, i = 1;
            lps[0] = 0;

            while (i < StringMatchingPattern.Count)
            {
                if (StringMatchingPattern[i].Character == StringMatchingPattern[length].Character)
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = length;
                        i++;
                    }
                }
            }
        }

        private async Task NaiveAlgorithm()
        {
            bool isFirstMatch = true;

            for (int i = 0; i <= StringMatchingInput.Count - StringMatchingPattern.Count; i++)
            {
                for (int j = 0; j < StringMatchingPattern.Count; j++)
                {
                    if (j == 0)
                    {
                        ResetColors();
                    }

                    StringMatchingInput[i + j].State = StringMatchingItemsState.Active;
                    StringMatchingPattern[j].State = StringMatchingItemsState.Active;

                    if (StringMatchingInput[i + j].Character != StringMatchingPattern[j].Character)
                    {
                        await Task.Delay(_delayTime);
                        await PauseAnimationIfRequired();
                        break;
                    }

                    if (j == StringMatchingPattern.Count - 1)
                    {
                        if (isFirstMatch)
                        {
                            Result += i;
                            isFirstMatch = false;
                        }
                        else
                        {
                            Result = Result + ", " + i;
                        }
                    }

                    await Task.Delay(_delayTime);
                    await PauseAnimationIfRequired();
                }
            }

            ResetColors();
        }

        private async Task PauseAnimationIfRequired()
        {
            if (IsAnimationPaused)
            {
                await _tcs.Task;
            }
        }

        private void ResetAnimation()
        {
            IsAnimationRunning = false;
            IsAnimationPaused = false;
            Result = "";
            if (IsApplied) IsApplied = false;
        }

        private void ResetColors()
        {
            for (int i = 0; i < StringMatchingInput.Count; i++)
            {
                if (StringMatchingInput[i].Character == '_')
                {
                    StringMatchingInput[i].State = StringMatchingItemsState.Space;
                }
                else
                {
                    StringMatchingInput[i].State = StringMatchingItemsState.Inactive;
                }
            }
            for (int i = 0; i < StringMatchingPattern.Count; i++)
            {
                if (StringMatchingPattern[i].Character == '_')
                {
                    StringMatchingPattern[i].State = StringMatchingItemsState.Space;
                }
                else
                {
                    StringMatchingPattern[i].State = StringMatchingItemsState.Inactive;
                }
            }
        }

        #endregion
    }
}