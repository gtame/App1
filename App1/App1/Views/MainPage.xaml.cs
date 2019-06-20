using App1.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
            
            // MenuPages.Add((int)MenuItemType.Historico, (NavigationPage)Detail);
        }
        //protected override void OnAppearing()
        //{
        //    //if (!App1.App.IsUserLoggedIn)
        //    //{
        //    //    base.OnAppearing();
        //    //    Navigation.PushModalAsync(new LoginPage());
        //    //}
        //    base.OnAppearing();
        //}
        public async Task NavigateFromMenu(int id)
        {

            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Historico:
                        MenuPages.Add(id, new NavigationPage( new ItemsPage()));
                        break;
                    case (int)MenuItemType.Acercade:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Consumo:
                        MenuPages.Add(id, new NavigationPage(new ConsumoPage()));
                        break;
                    case (int)MenuItemType.Logout:
                        {

                            MessagingCenter.Send<object>(this, App.EVENT_LAUNCH_LOGIN_PAGE);
                            return;
                        }
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
           
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}