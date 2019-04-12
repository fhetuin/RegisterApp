using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using RegisterApp.Model;

namespace RegisterApp.Tools
{
 //Petite bidouille pour transformer un service en une liste contenant celui-ci pour le binding 
    class ServiceToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Service)
            {
                List<Service> services = new List<Service>();
                services.Add((Service)value);
                return services;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
