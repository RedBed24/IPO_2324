using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace hospital_gui_wpf
{
    public class GeneroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Genero genero) || !(parameter is string generoParametro))
                return false;

            return genero.ToString() == generoParametro;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool isChecked) || !(parameter is string generoParametro))
                return Genero.Otro;  // O retorna lo que necesites cuando no sea válido

            return isChecked ? Enum.Parse(typeof(Genero), generoParametro) : Binding.DoNothing;
        }
    }
}
