﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using App1.Models;
using App1.Views;
using System.Collections.Generic;

namespace App1.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        private ObservableCollection<Movimiento> _list;

        public ObservableCollection<Movimiento> Items
        {
            get { return _list; }
            set
            {
                _list = value;
                OnPropertyChanged();
            }
        }

        
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Ultimos movimientos";
            _list = new ObservableCollection<Movimiento>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Movimiento>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Movimiento;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetMovimientosAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}