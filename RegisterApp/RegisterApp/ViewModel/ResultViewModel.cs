using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using RegisterApp.Model;
using RegisterApp.Tools;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using Element = RegisterApp.Model.Element;

namespace RegisterApp.ViewModel
{
    public class ResultViewModel : RegisterViewModelBase
    {

        //La commande en doublon du mainviewModel, pas propre mais c'était la méthode la plus rapide pour créer la navbar du haut sans trop s'embêter
        public ICommand ShowInformationCommand { get; private set; }

        public ResultViewModel()
        {
            ShowInformationCommand = new Command(ShowInformations);
            Refresh();
        }

        
        public async void Refresh()
        {
            //Refresh l'affichage des résultats, on fait appel à cette methode lors de l'enregistrement d'un nouveau formulaire
            ServiceResults.Clear();
            List<Result> resultsList = await App.Database.GetResultsFromUserAsync(ProprietesGlobales.CurrentUser);
            List<Service> services = await App.Database.GetServicesAsync();
            foreach(Service service in services)
            {

                if(resultsList.Where(r => r.ServiceId == service.Id).Count() != 0) // Si il existe au moins un résultat pour ce service, LINQ c'est génial
                {
                    ServiceResult serviceResult = new ServiceResult();
                    serviceResult.Results.Clear(); 
                    serviceResult.Service = service; // On définit le service

                    foreach (Result result in resultsList.Where(r => r.ServiceId == service.Id))  //Pour chaque résultats correspondant à ce service
                    {
                        serviceResult.Results.Add(result); // ON les ajoute dans la liste
                        Debug.WriteLine(result.ToString());
                    }

                    serviceResult.Init(); // On initialise notre classe d'affichage de resultats
                    ServiceResults.Add(serviceResult); // On incrémente la observablecollection qui est bindé à notre vue
                }
                else
                {

                }
            }
           

        }

        private void ShowInformations()
        {
            var device = Resolver.Resolve<IDevice>();
            float totalMemory = (float)device.TotalMemory / 1000000000f;
            App.Current.MainPage.DisplayAlert("Informations", "Total RAM = " + totalMemory + " Go\n Screen = \n" + device.Display.ToString(), "OK");
        }

        private ObservableCollection<ServiceResult> serviceResults = new ObservableCollection<ServiceResult>();

        public ObservableCollection<ServiceResult> ServiceResults
        {
            get
            {
                return serviceResults;
            }

            set
            {
                serviceResults = value;
                OnPropertyChanged("ServiceResults");

            }
        }

        //Une classe supplémentaire pour afficher de manière décente les résultats de l'utilisateur et pouvoir profiter du binding
        public class ServiceResult : RegisterViewModelBase
        {
            private List<Result> results = new List<Result>();

            private ObservableCollection<ElementResult> elementResults = new ObservableCollection<ElementResult>();

            public ObservableCollection<ElementResult> ElementResults
            {
                get
                {
                    return elementResults;
                }

                set
                {
                    elementResults = value;
                    OnPropertyChanged("ElementResults");

                }
            }

            public Service Service { get => service; set => service = value; }
            public List<Result> Results { get => results; set => results = value; }

            private Service service;



            //J'initialise les éléments de la classe à l'aide des résultats de l'utilisateur
            public void Init()
            {
                string str = string.Empty;

                foreach (Section section in service.Sections)
                {
                    foreach (Element element in section.Elements)
                    {
                        Result result = Results.Where(r => r.ElementId == element.Id).FirstOrDefault();
                        if (result != null)
                        {
                            if (element.Values.Count == 1)
                            {// Si le nombre de valeurs est égal à 1 on va afficher celle-ci avant le résultat :
                                if (!(element.Values[0].StrValue.Equals("true")) && !(element.Values[0].StrValue.Equals("false")))
                                { // si la valeur de l'élément n'est pas "true ou false" on peut afficher la valeur
                                    if (result.IsChecked != null)
                                    {
                                        if (result.IsChecked == true)
                                        {
                                            ElementResults.Add(new ElementResult(element.Values[0].StrValue + " : ", "Oui"));
                                        }
                                        else
                                        {
                                            ElementResults.Add(new ElementResult(element.Values[0].StrValue + " : ", "Non"));
                                        }
                                    }
                                    else
                                    {
                                        ElementResults.Add(new ElementResult(element.Values[0].StrValue + " : ", result.Resultat));
                                    }

                                } // Sinon on affiche le nom de la section pour que le resultat est quand même du sens
                                else
                                {
                                    ElementResults.Add(new ElementResult(section.Name + " : ", result.Resultat));
                                }
                            }// Sinon on affiche le nom de la section pour que le resultat est quand même du sens
                            else
                            {
                                ElementResults.Add(new ElementResult(section.Name + " : ", result.Resultat));
                            }
                        }
                    }
                }

            }
        }
      // Encore une classe pour la même utilité que ServiceResult
        public class ElementResult
        {
            private string elementValue;
            private string resultValue;

            public string ResultValue { get => resultValue; set => resultValue = value; }
            public string ElementValue { get => elementValue; set => elementValue = value; }

            public ElementResult(string elem, string res)
            {
                this.elementValue = elem;
                this.resultValue = res;
            }
        }


    }
}
