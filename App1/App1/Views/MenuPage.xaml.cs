﻿using App1.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Consumo, Title="Consumo" },
                new HomeMenuItem {Id= MenuItemType.ConsumoXArticulo , Title="ConsumoxArtic"},
                new HomeMenuItem {Id = MenuItemType.Reubicar, Title="Reubicar" },
                new HomeMenuItem {Id = MenuItemType.Historico, Title="Historico" },
                new HomeMenuItem {Id = MenuItemType.Acercade, Title="Acerca de" },
                new HomeMenuItem {Id = MenuItemType.Logout, Title="Logout" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}