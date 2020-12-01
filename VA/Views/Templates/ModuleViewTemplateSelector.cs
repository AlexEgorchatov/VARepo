using System.Windows;
using System.Windows.Controls;
using VA.Interfaces;

namespace VA.Views
{
    public class ModuleViewTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is IModule)
            {
                switch ((item as IModule).ModuleType)
                {
                    case ModuleType.SortModule:
                        return element.FindResource("SortModuleViewTemplate") as DataTemplate;

                    case ModuleType.StringModule:
                        return element.FindResource("StringMatchingViewTemplate") as DataTemplate;

                    default:
                        return null;
                }
            }

            return null;
        }
    }
}