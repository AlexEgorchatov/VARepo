using System.Windows;
using System.Windows.Controls;
using VA.ViewModels;

namespace VA.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        #region Public Constructors

        public HomePage()
        {
            InitializeComponent();
            DataContext = new HomePageViewModel();
        }

        #endregion

        #region Private Methods

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if ((DataContext as HomePageViewModel).Modules == null)
            {
                await (DataContext as HomePageViewModel).Initialize();
                (DataContext as HomePageViewModel).InitializeModules();
            }
        }

        #endregion
    }
}