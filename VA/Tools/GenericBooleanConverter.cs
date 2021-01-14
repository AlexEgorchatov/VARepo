using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VA.Tools
{
    internal class GenericBooleanConverter : IValueConverter, IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = System.Convert.ToBoolean(value);
            val = parameter == null ? val : !val;
            if (targetType == typeof(Visibility))
            {
                return val ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (targetType == typeof(Brush))
            {
                return val ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            }
            else
            {
                return val;
            }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value == DependencyProperty.UnsetValue)
                {
                    return false;
                }
                bool val = System.Convert.ToBoolean(value);
                if (val == false)
                {
                    if (targetType == typeof(Visibility))
                    {
                        return Visibility.Collapsed;
                    }
                    else if (targetType == typeof(Brush))
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (targetType == typeof(Visibility))
            {
                return Visibility.Visible;
            }
            else if (targetType == typeof(Brush))
            {
                return new SolidColorBrush(Colors.Green);
            }
            else
            {
                return true;
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}