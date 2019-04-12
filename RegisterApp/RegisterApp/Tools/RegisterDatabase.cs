using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RegisterApp.Model;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace RegisterApp
{
    public class RegisterDatabase

        //La classe de gestion de la base de données SQLITE avec les méthode de sauvegarde et de récupération, de création de table ...
    {
        private readonly SQLiteAsyncConnection database;

        public RegisterDatabase(string dbPath)
        {
            try
            {

                database = new SQLiteAsyncConnection(dbPath);
                if (ClearDBAsync())// ON clear la base parcequ'on va reparser le json au cas où il a changé
                {
                    database.CreateTableAsync<User>().Wait();
                    database.CreateTableAsync<Section>().Wait();
                    database.CreateTableAsync<Service>().Wait();
                    database.CreateTableAsync<Element>().Wait();
                    database.CreateTableAsync<Result>().Wait();
                    database.CreateTableAsync<Value>().Wait();
                    User newUser = new User()
                    {
                        Id = 1,
                        Login = "login",
                        Password = "password",
                        Results = new List<Result>()
                    };
                    database.InsertOrReplaceAsync(newUser).Wait();
                    ProprietesGlobales.CurrentUser = newUser;



                }

                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        

        public Task<User> GetUserAsync(int id)
        {
            return database.GetWithChildrenAsync<User>(id);
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            List<Service> services =  await database.GetAllWithChildrenAsync<Service>();
            foreach(Service service in services)
            {
                Debug.WriteLine(service.Title);
                foreach(Section section in service.Sections)
                {
                    // Comme on peut pas récupérer les petits enfants directements on les récupéres à la main :
                    //Debug.WriteLine(section.Name);
                    section.Elements = await GetElementsFromSectionsAsync(section);
                }
            }
            return services;
        }

        public Task<List<Section>> GetSectionsAsync()
        {
            return database.GetAllWithChildrenAsync<Section>();
        }

        public Task<List<Result>> GetResultsFromUserAsync(User user)
        {
            return database.GetAllWithChildrenAsync<Result>(r => r.UserId == user.Id);
        }

        public Task<Section> GetSectionFromId(int id)
        {
            return database.GetWithChildrenAsync<Section>(id);
        }

        public Task<Service> GetServiceFromId(int id)
        {
            return database.GetWithChildrenAsync<Service>(id);
        }

        public Task<Element> GetElementFromId(int id)
        {
            return database.GetWithChildrenAsync<Element>(id);
        }

        public async Task<Section> GetSectionFromElementId(int id)
        {
            Element element = await GetElementFromId(id);
            Section section = await GetSectionFromId(element.SectionId);
            return section;
        }

        public Task<List<Element>> GetElementsFromSectionsAsync(Section section)
        {
            return database.GetAllWithChildrenAsync<Element>(e => e.SectionId == section.Id);
        }



        public Task SaveUserAsync(User user)
        {
            return database.InsertOrReplaceWithChildrenAsync(user, true);
        }


        public Task SaveResultsAsync(List<Result> results)
        {
            return database.InsertOrReplaceAllWithChildrenAsync(results);
        }


        public Task SaveServiceAsync(Service item)
        {
            if (item.Id != 0)
            {
                return database.UpdateWithChildrenAsync(item);
            }
            else
            {
                return database.InsertOrReplaceWithChildrenAsync(item);
            }
        }



        public Task SaveServicesAsync(List<Service> services)
        {
           return database.InsertAllWithChildrenAsync(services, true);
        }

        public bool ClearDBAsync()
        {
            try
            {
                //database.DropTableAsync<User>().Wait();
                //database.DropTableAsync<Result>().Wait();
                database.DropTableAsync<Section>().Wait();
                database.DropTableAsync<Service>().Wait();
                database.DropTableAsync<Element>().Wait();
                database.DropTableAsync<Value>().Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}