using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace RegisterApp.ViewModel
{

    //ça c'était juste pour éviter de réimplémenter INotifyPropertyChanged dans nos ViewModel mais au final c'est pas fort utile comme le projet est assez légé
    public abstract class RegisterViewModelBase : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
