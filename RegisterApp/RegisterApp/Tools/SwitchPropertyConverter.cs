using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using RegisterApp.Model;

namespace RegisterApp.Tools
{
    //Converter pour transformer les valeurs des éléments en string pour notre objet resultat
    public class SwitchPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Value)
            {
                return ((Value)value).StrValue;
            }
            else if(value is bool)
            {
                if ((bool)value == true)
                    return "Vrai";
                else
                    return "Faux";
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals("Faux"))
            {
                return false;
            }
            else if(value.Equals("Vrai"))
            {
                return true;
            }
            else if(value.Equals(string.Empty))
            {
                return null;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
