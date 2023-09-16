using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Task_Managment.ViewModel.Converters
{
    public class BooltoVisOpposite : IValueConverter
    {
        //This conversion is used for the main task lists
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Convert Bool to vis
            if(value is bool)
            {
                if ((value as bool?) == false)
                {
                    return Visibility.Visible;
                }
                else return Visibility.Hidden;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Convert vis to bool
            if (value is Visibility)
            {
                if ((value as Visibility?) == Visibility.Hidden)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
