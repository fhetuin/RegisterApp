using RegisterApp.Tools;
using RegisterApp.View;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RegisterApp
{
    public partial class App : Application
    {
        public App()
        {


            InitializeComponent();

            database = new RegisterDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegisterDB.db3"));

            //Je récupére le JSON
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("RegisterApp.service.json");
            string json = string.Empty;
            using (var reader = new System.IO.StreamReader(stream))
            {
                //On le lit
                json = reader.ReadToEnd();
            }
            if (json != string.Empty)
            {
                //On le parse selon notre model
                JsonParse.ParseJson(json);
            }
            MainPage = new MainView();
        }



        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        static RegisterDatabase database;

        public static RegisterDatabase Database
        {
            get
            {

                return database;
            }
        }

    }
}
