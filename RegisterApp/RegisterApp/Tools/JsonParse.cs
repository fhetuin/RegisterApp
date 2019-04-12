using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegisterApp.Model;
using Xamarin.Forms;
using Element = RegisterApp.Model.Element;

namespace RegisterApp.Tools
{
    public class JsonParse
    {

        public async static void ParseJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {

                List<Service> services = new List<Service>();
                dynamic objson = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                foreach (var service in objson.services)
                {
                    // la section title ob ne la compte pas comme une section mais on l'associe plutot au service directement
                    Service newService = new Service((string)service.title);

                    foreach (var element in service.elements)
                    {
                        // on parcourt tous les éléments

                        if (((string)element.section).Equals("title"))
                        {
                            newService.Logo = element.value[0]; // section title donc associé au service
                        }
                        else
                        {
                            Section section = (from sect in newService.Sections where sect.Name.Equals((string)element.section) select sect).FirstOrDefault();
                            //Commande LINQ permettant de récupérer la section du service en cours portant le même nom que l'élément en cours
                            //Si elle existe on implémente le nouvel élément dans celle-ci
                            //Sinon on en créé une nouvelle

                            if (section != null)
                            {
                                int index = newService.Sections.IndexOf(section);
                                List<Value> values = new List<Value>();
                                foreach (var val in element.value)
                                {
                                    Value value = new Value();
                                    value.StrValue = (string)val;
                                    values.Add(value);
                                }
                                Element newElement = new Element((bool)element.mandatory, values, (string)element.type);

                                newService.Sections[index].Elements.Add(newElement); // On associe l'élément à la section déjà existante dans le service
                            }
                            else
                            {

                                Section newSection = new Section((string)element.section);

                                List<Value> values = new List<Value>();
                                foreach (var val in element.value)
                                {
                                    Value value = new Value();
                                    value.StrValue = (string)val;
                                    values.Add(value);
                                }
                                Element newElement = new Element((bool)element.mandatory, values, (string)element.type);

                                newSection.Elements.Add(newElement); // On ajoute l'élement à la nouvelle section
                                newService.Sections.Add(newSection); // On ajoute la section au service en cours
                            }
                        }
                    }
                    services.Add(newService); // On ajoute le nouveau service
                }
                await App.Database.SaveServicesAsync(services); // On sauvegarde en base de données

            }

        }
    }
}
