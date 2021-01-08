using System.Windows.Controls;
using VA.ViewModels;

namespace VA.Views.Pages
{
    /// <summary>
    /// Interaction logic for SortModulePage.xaml
    /// </summary>
    public partial class SortModulePage : Page
    {
        #region Public Constructors

        public SortModulePage(SortModulePageViewModel sortViewModel)
        {
            InitializeComponent();
            DataContext = sortViewModel;
        }

        #endregion
    }
}