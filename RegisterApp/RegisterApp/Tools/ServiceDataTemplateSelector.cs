using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using RegisterApp.Model;
using Element = RegisterApp.Model.Element;
using System.Linq;
using System.Diagnostics;

namespace RegisterApp.Tools
{
    public class ServiceDataTemplateSelector : DataTemplateSelector
    {

        public Dictionary<string, DataTemplate> DataTemplates = new Dictionary<string, DataTemplate>();
        
        //Le fameux switch permettant la création du formulaire
        public ServiceDataTemplateSelector(List<Service> services)
        {
            foreach(Service service in services)
            {

                //ProprietesGlobales.CurrentUser.Results.Clear();

                var serviceDataTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid() { RowSpacing = 15, ColumnSpacing = 15, BackgroundColor = Color.Green, VerticalOptions = LayoutOptions.StartAndExpand};
                    //Nouvelle grille avec un rowspacing et un background vert pour afficher la ligne verte entre chaque section

                    int i = 0;
                    foreach (Section section in service.Sections)
                    {
                        Label labelSection = new Label() { HorizontalTextAlignment = TextAlignment.Center, FontSize = 24, TextColor = Color.Green };
                        //Label pour la section

                        labelSection.Text = section.Name;
                        Debug.WriteLine(section.Name);
                        i++;
                        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        //Une nouvelle ligne pour la grille


                        var stackLayoutSection = new StackLayout() { BackgroundColor = Color.White}; // LA couleur blanche c'est pour différencier de la grille qui est verte pour afficher la ligne entre chaque section
                        //Le stacklayout contenant les controls de la section
                        stackLayoutSection.Children.Add(labelSection);
                        foreach (Element element in section.Elements)
                        {
                            Debug.WriteLine(element.Values[0].StrValue);
                            Result result = new Result() { ElementId = element.Id, ServiceId = service.Id, UserId = ProprietesGlobales.CurrentUser.Id };
                            
                            switch (element.Type)
                            {
                                case "image":
                                    Image image = new Image();
                                    image.Source = element.Values[0].StrValue;
                                    result.Resultat = element.Values[0].StrValue;
                                    //GetOldResult(result); On enregistre pas dans les résultats de l'utilisateur puisque c'est un élément par défaut
                                    stackLayoutSection.Children.Add(image);
                                    break;
                                case "edit":
                                    Entry entry = new Entry();
                                    entry.Placeholder = element.Values[0].StrValue;
                                    entry.BindingContext = GetOldResult(result);
                                    entry.SetBinding<Result>(Entry.TextProperty, res => res.Resultat); //On definit le binding du control a notre element result
                                    stackLayoutSection.Children.Add(entry);
                                    break;
                                case "label":
                                    Label label = new Label() {};
                                    label.Text = element.Values[0].StrValue;
                                    result.Resultat = element.Values[0].StrValue;
                                    //GetOldResult(result); On enregistre pas dans les résultats de l'utilisateur puisque c'est un élément par défaut
                                    stackLayoutSection.Children.Add(label);
                                    break;
                                case "radioGroup":
                                    Picker picker = new Picker();
                                    picker.ItemsSource = element.Values;
                                    picker.Title = element.Values[0].StrValue;
                                    picker.BindingContext = GetOldResult(result);
                                    //On definit le binding du control a notre element result
                                    picker.SetBinding<Result>(Picker.SelectedItemProperty, res => res.Resultat, BindingMode.Default, new SwitchPropertyConverter());
                                    stackLayoutSection.Children.Add(picker);
                                    break;
                                case "switch":
                                    Xamarin.Forms.Switch sw = new Xamarin.Forms.Switch();
                                    sw.BindingContext = GetOldResult(result);
                                    //On definit le binding du control a notre element result
                                    sw.SetBinding<Result>(Xamarin.Forms.Switch.IsToggledProperty, res => res.Resultat, BindingMode.Default, new SwitchPropertyConverter());
                                    stackLayoutSection.Children.Add(sw);
                                    break;
                                case "button":
                                    XLabs.Forms.Controls.CheckBox checkBox = new XLabs.Forms.Controls.CheckBox();
                                    checkBox.DefaultText = element.Values[0].StrValue;
                                    result.Resultat = element.Values[0].StrValue;
                                    result.IsChecked = false;
                                    checkBox.BindingContext = GetOldResult(result);
                                    //On definit le binding du control a notre element result
                                    checkBox.SetBinding<Result>(XLabs.Forms.Controls.CheckBox.CheckedProperty, res => res.IsChecked);
                                    stackLayoutSection.Children.Add(checkBox);
                                    break;
                            }
                          
                        }
                        grid.Children.Add(stackLayoutSection, 0, i);

                    }
                    return new ViewCell { View = grid};
                });

                DataTemplates.Add(service.Title, serviceDataTemplate);
            }

        }

        //recupere l'ancien resultat de l'utilisateur pour cet element, si il existe, sinon retourne le nouveau resultat vide
        private Result GetOldResult(Result result)
        {
            //On regarde si le resultat existe déjà
            Result oldResult = ProprietesGlobales.CurrentUser.Results.Where(r => r.ElementId == result.ElementId && r.ServiceId == result.ServiceId).FirstOrDefault();
            if (oldResult != null)
            {
                return oldResult; // On renvoie l'ancien résultat
            }
            else
            {
                ProprietesGlobales.CurrentUser.Results.Add(result); // Il s'agit d'un nouveau résultat il faut l'ajouter aux résultat de l'utilisateur
                return result; // On renvoie le nouveau resultat
            }
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Service)
            {
                return DataTemplates[((Service)item).Title];
            }

            return null;
        }
    }
}
