using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStoreAdmin
{
    /* Esta clase tiene como finalidad el establecer un límite de longitud para los nombres de los productos y así, 
     no afear ni desajustar la vista de la página.*/
    public class TruncateTextConverter : IValueConverter
    {
        public int MaxLength { get; set; } = 35;

        public object Convert(object value, Type typeTarget, object parameter, CultureInfo culture)
        {
            if (value is string texto && texto.Length > MaxLength)
            {
                return texto.Substring(0, MaxLength) + "..."; 
            }
            return value;
        }

        public object ConvertBack(object value, Type typeTarget, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
