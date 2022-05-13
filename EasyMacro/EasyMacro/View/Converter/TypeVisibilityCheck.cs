using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasyMacro.View.Converter
{
    /// <summary>
    /// 객체 o와 유형 t가 주어지면 o가 유형 t이면 Visible을 반환하고 그렇지 않으면 Collapsed를 반환합니다.
    /// </summary>
    public class TypeVisibilityCheck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Type)parameter).IsInstanceOfType(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
