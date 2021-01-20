using System.Windows;
using System.Windows.Controls;
using VA.Interfaces;
using VA.Resources;

namespace VA.Views
{
    public class ModuleViewTemplateSelector : DataTemplateSelector
    {
        #region Public Methods

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is IModule)
            {
                switch ((item as IModule).ModuleType)
                {
                    case ModuleType.SortModule:
                        return element.FindResource("SortModuleViewTemplate") as DataTemplate;

                    case ModuleType.StringMatchingModule:
                        return element.FindResource("StringMatchingViewTemplate") as DataTemplate;

                    case ModuleType.GridPathModule:
                        return element.FindResource("GridPathViewTemplate") as DataTemplate;

                    default:
                        return null;
                }
            }

            return null;
        }

        #endregion
    }
}