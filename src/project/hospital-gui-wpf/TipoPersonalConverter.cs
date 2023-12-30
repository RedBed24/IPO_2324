using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace hospital_gui_wpf
{
    public class TipoPersonalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TipoPersonal tipoPersonal) || !(parameter is string tipoParametro))
                return false;

            return tipoPersonal.ToString() == tipoParametro;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool isChecked) || !(parameter is string tipoParametro))
                return Binding.DoNothing;

            return isChecked ? Enum.Parse(typeof(TipoPersonal), tipoParametro) : Binding.DoNothing;
        }
    }
}
