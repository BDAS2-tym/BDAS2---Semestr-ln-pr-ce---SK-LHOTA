using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BDAS2_Sem_Prace_Cincibus_Tluchor.Class.Text_Converters
{
    public class SponzorovaneSoutezeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Soutez soutez)
                return $"{soutez.TypSouteze}    ({soutez.StartDatum:dd.MM.yyyy} -- {soutez.KonecDatum:dd.MM.yyyy})";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
