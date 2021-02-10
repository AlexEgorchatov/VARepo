using System;
using System.Windows;
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
        #region Public Constructors

        private double _previousSize;

        public GridPathModulePage(GridPathModulePageViewModel gridPathViewModel)
        {
            InitializeComponent();
            DataContext = gridPathViewModel;
        }

        #endregion

        private void CanvasSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var cellHeight = (e.NewSize.Height - GridPathModuleCellViewModel.Margin.Top * 2) / GridPathModulePageViewModel.Rows;
            var cellWidth = (e.NewSize.Width - GridPathModuleCellViewModel.Margin.Left * 2) / GridPathModulePageViewModel.Columns;
            var newSize = Math.Round(Math.Min(cellWidth, cellHeight));
            if(Math.Abs(newSize - _previousSize) > 1)
            {
                _previousSize = newSize;
                (DataContext as GridPathModulePageViewModel).SetCellSize(newSize);
            }
        }
    }
}