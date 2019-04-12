using RegisterApp.Tools;
using RegisterApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading;
using Element = RegisterApp.Model.Element;
using System.Windows.Input;
using System.Linq;
using RegisterApp.View;
using XLabs.Platform.Device;
using XLabs.Ioc;

namespace RegisterApp.ViewModel
{
    public class MainViewModel : RegisterViewModelBase
    {
        // MAINVIEWMODEL : CONTEXT de MainView, FormView et ServiceView. En théorie, c'est un viewmodel par vue, mais ça reste compréhensible ici.

        //ObservableCollection pour pouvoir add et remove et que ça s'affiche en temps réel dans la vue là ou l'élément est bindé
        private ObservableCollection<Service> services = new ObservableCollection<Service>();

        //Là c'est la partie compliqué, dans un soucis de code management, nous ne voulions pas écrire dans le code behind !
        //Donc même le switch qui détermine le template à adopter (bouton, label ... ) est bindé grâce à ce dataTemplateSelector
        private ServiceDataTemplateSelector serviceDataTemplateSelector;

        //Notre viewModel pour les résultats
        private ResultViewModel resultViewModel = new ResultViewModel();

        private Service selectedService = null;

        //LES COMMANDES pour les boutons Enregistrer et la popup d'information sur le device du bouton en haut à droite (?)
        public ICommand RegisterCommand { get; private set; }
        public ICommand ShowInformationCommand { get; private set; }

        public MainViewModel()
        {
            init();

            RegisterCommand = new Command(Register);
            ShowInformationCommand = new Command(ShowInformations);
        }

        //La commande d'annulation, qui renvoie l'utilisateur à la page de choix des services.
        public ICommand CancelCommand
        {
            get
            {
                return new Command
                (
                    (parameter) =>
                    {
                        var page = parameter as ContentPage;
                        var parentPage = page.Parent as TabbedPage;
                        parentPage.CurrentPage = parentPage.Children[0];
                    }
                );
            }
        }


        // Les fonctions des commandes 

        private void ShowInformations()
        {
            var device = Resolver.Resolve<IDevice>();
            float totalMemory = (float)device.TotalMemory / 1000000000f;
            App.Current.MainPage.DisplayAlert("Informations","Total RAM = " + totalMemory + " Go\n Screen = \n" + device.Display.ToString(), "OK");
        }

        private void Register()
        {
            if(CanRegisterUser())
            {
                FieldsErrorVisibility = false;
                App.Database.SaveUserAsync(ProprietesGlobales.CurrentUser); // On sauvegarde l'utilisateur
                /*List<Result> results = ProprietesGlobales.CurrentUser.Results.Where(r => r.ServiceId == SelectedService.Id).ToList();
                App.Database.SaveResultsAsync(results).Wait();*/
            }
            else
            {
                FieldsErrorVisibility = true;
            }
                
            ResultViewModel.Refresh();
        }


        //La fonction qui permet de définir si tous les champs obligatoires sont remplis
        private bool CanRegisterUser()
        {
            bool canInsertOrReplace = true;
            foreach (Section section in SelectedService.Sections)
            {
                foreach (Element element in section.Elements)
                {
                    if (element.Mandatory == true && !(element.Type.Equals("image")) && !(element.Type.Equals("label")))
                    {
                        //Si ce sont des images ou des labels, on ne vérifie pas puisque je n'insére pas ces éléments dans les résultats utilisateurs
                        //Je n'insére dans les résultats, que les éléments saisis et ou choisis par l'utilisateur
                        Result result = ProprietesGlobales.CurrentUser.Results.Where(r => r.ElementId == element.Id).FirstOrDefault();

                        if (result == null)
                        {
                            canInsertOrReplace = false;
                        }
                        else if (result.Resultat == null)
                        {
                            canInsertOrReplace = false;
                        }
                        else if(result.Resultat.Trim() == string.Empty)
                        {
                            canInsertOrReplace = false;
                        }
                    }
                }
            }

            return canInsertOrReplace;
        }

        
        public async void init()
        {
            //Je récupere mes services dans la base de données, ceux-ci ont été parsé et inséré au démarrage de l'appli dans App.Xaml
            List<Service> serviceList = await App.Database.GetServicesAsync();
            Services = new ObservableCollection<Service>(serviceList);
            ServiceDataTemplateSelector = new ServiceDataTemplateSelector(serviceList); // On initialise les "pré-templates" qui s'afficheront dans la fenêtre formulaire
            //Si nous ne les préinitialisation pas et nous avons une erreur car il n'est possible que d'initialiser une 20ène de Datatemplates (par soucis de mémoire)
            if(Services != null && Services.Count() != 0)
                SelectedService = Services[0]; // On défini de base un service selectionné pour faire beau et ne pas avoir une page vide si il swipe sans selectionner
        }

        //La suite n'est pas intéressante, ce sont les accésseurs publiques avec OnPropertyChanged() qui permet le binding et l'affichage des nouvelles assignations en temps réel
        public ResultViewModel ResultViewModel
        {
            get
            {
                return resultViewModel;
            }

            set
            {
                resultViewModel = value;
                OnPropertyChanged("ResultViewModel");

            }
        }

        private bool fieldsErrorVisibility = false;

        public bool FieldsErrorVisibility
        {
            get
            {
                return fieldsErrorVisibility;
            }

            set
            {
                fieldsErrorVisibility = value;
                OnPropertyChanged("FieldsErrorVisibility");

            }
        }

        public ServiceDataTemplateSelector ServiceDataTemplateSelector
        {
            get
            {
                return serviceDataTemplateSelector;
            }

            set
            {
                serviceDataTemplateSelector = value;
                OnPropertyChanged("ServiceDataTemplateSelector");
            
            }
        }

        

        public Service SelectedService
        {
            get
            {
                return selectedService;
            }

            set
            {
                selectedService = value;
                FieldsErrorVisibility = false;
                OnPropertyChanged("SelectedService");

            }
        }


        public ObservableCollection<Service> Services
        {
            get
            {
                return services;
            }

            set
            {
                services = value;
                OnPropertyChanged("Services");

            }
        }


    }
}
