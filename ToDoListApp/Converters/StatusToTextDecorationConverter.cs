using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ToDoListApp.Converters
{
    public class StatusToTextDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskStatus status && status == TaskStatus.Completed)
                return TextDecorations.Strikethrough;

            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TaskStatus.NotStarted;
        }
    }
}
