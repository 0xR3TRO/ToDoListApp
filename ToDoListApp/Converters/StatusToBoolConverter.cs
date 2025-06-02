using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ToDoListApp.Converters
{
    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is TaskStatus status) && status == TaskStatus.Completed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TaskStatus.Completed : TaskStatus.NotStarted;
        }
    }
}
