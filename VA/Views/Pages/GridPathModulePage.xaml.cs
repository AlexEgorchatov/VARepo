using System;
using System.Windows.Controls;
using VA.ViewModels;
using VA.ViewModels.Animations;

namespace VA.Views.Pages
{
    /// <summary>
    /// Interaction logic for GridPathModulePage.xaml
    /// </summary>
    public partial class GridPathModulePage : Page
    {
        #region Private Fields

        private double _previousSize;

        #endregion

        #region Public Constructors

        public GridPathModulePage(GridPathModulePageViewModel gridPathViewModel)
        {
            InitializeComponent();
            DataContext = gridPathViewModel;
        }

        #endregion

        #region Private Methods

        private void CanvasSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var cellHeight = (e.NewSize.Height - GridPathModuleCellViewModel.Margin.Top * 2) / GridPathModulePageViewModel.Rows;
            var cellWidth = (e.NewSize.Width - GridPathModuleCellViewModel.Margin.Left * 2) / GridPathModulePageViewModel.Columns;
            var newSize = Math.Round(Math.Min(cellWidth, cellHeight));
            if (Math.Abs(newSize - _previousSize) > 1)
            {
                _previousSize = newSize;
                (DataContext as GridPathModulePageViewModel).SetCellSize(newSize);
            }
        }

        #endregion
    }
}