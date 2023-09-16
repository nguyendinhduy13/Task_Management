using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Task_Managment.ViewModel.Converters
{
    public class BooltoVisSubtasksPane : IValueConverter
    {
        //converts boolean to visibility for subtasks pane
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((value as bool?) == false)
                {
                    return Visibility.Collapsed;
                }
                else return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                if ((value as Visibility?) == Visibility.Collapsed)
                {
                    return false;
                }
                else return true;
            }
            return false;
        }
    }
}
