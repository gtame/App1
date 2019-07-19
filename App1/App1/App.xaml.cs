using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Services;
using App1.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App1
{
    public partial class App : Application, IApplicationHandler, ICodeReader
    {

        public static string EVENT_LAUNCH_LOGIN_PAGE = "EVENT_LAUNCH_LOGIN_PAGE";
        public static string EVENT_LAUNCH_MAIN_PAGE = "EVENT_LAUNCH_MAIN_PAGE";
        public static string EVENT_LAUNCH_BULTOS_PAGE = "EVENT_LAUNCH_SELECT_BULTOS_PAGE";
        public static string EVENT_SELECTED_BULTO = "EVENT_SELECTED_BULTO";
        public static string EVENT_CODEBAR_READ = "EVENT_CODEBAR_READED";


        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://192.168.1.7:5000/api/Almacen";
        public static bool UseMockDataStore = false;
        public static bool IsUserLoggedIn { get; set; }


        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();

            DependencyService.Register<AlertHelper>();


            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                System.Exception ex = (System.Exception)args.ExceptionObject;
                Console.WriteLine(ex);
            };

            MessagingCenter.Subscribe<object>(this, EVENT_LAUNCH_LOGIN_PAGE, SetLoginPageAsRootPage);
            MessagingCenter.Subscribe<object>(this, EVENT_LAUNCH_MAIN_PAGE, SetMainPageAsRootPage);


            MainPage = new LoginPage();
            //MainPage = new MainPage();
        }
        private void SetLoginPageAsRootPage(object sender)
        {
            MainPage = new LoginPage();
        }

        private void SetMainPageAsRootPage(object sender)
        {
            MainPage = new MainPage();
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

        public void Logout()
        {
            MessagingCenter.Send<object>(this, App.EVENT_LAUNCH_LOGIN_PAGE);
        }


        void ICodeReader.OnReadCodeBar(string text)
        {
            //envia mensaje a la aplicacion de lectura de codigo de barras ;)
            MessagingCenter.Send<string>(text, App.EVENT_CODEBAR_READ);

        }



    }
}
