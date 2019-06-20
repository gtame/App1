using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsumoPage : ContentPage
	{
     

        MovimientoViewModel model;

        public ConsumoPage()
		{
            model = new MovimientoViewModel();
            this.BindingContext = model;
            InitializeComponent ();
 
		}


        public ConsumoPage(Movimiento movimiento)
        {
            model = new MovimientoViewModel(movimiento);
            this.BindingContext = model;
            InitializeComponent();

            MessagingCenter.Subscribe<IEnumerable<Bulto>>(this, App.EVENT_LAUNCH_BULTOS_PAGE, async b =>
            {

                if (b.Count() == 0)
                    await DisplayAlert("Bultos", "No hay bultos disponibles en la ubicación", "OK");
                else
                {
                 
                }
            }  
             );
       

        }

        private async void Deshacer_Clicked(object sender, EventArgs e)
        {
            bool ok=await DisplayAlert("Deshaceer", "¿Esta seguro que desea deshacer el consumo?","Ejecutar", "Cancelar");
            if (ok)
            {
                //model delete
                await Navigation.PopAsync();
                
            }
        }
    }
}