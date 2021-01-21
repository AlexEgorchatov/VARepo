using System.Windows.Controls;
using VA.ViewModels;

namespace VA.Views.Pages
{
    /// <summary>
    /// Interaction logic for StringMatchingModulePage.xaml
    /// </summary>
    public partial class StringMatchingModulePage : Page
    {
        #region Public Constructors

        public StringMatchingModulePage(StringMatchingModulePageViewModel stringMatchigViewModel)
        {
            InitializeComponent();
            DataContext = stringMatchigViewModel;
        }

        #endregion
    }
}