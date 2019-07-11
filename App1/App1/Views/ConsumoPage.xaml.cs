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
            MessagingCenter.Subscribe<IEnumerable<Bulto>>(this, App.EVENT_LAUNCH_BULTOS_PAGE, async b =>
            {

                if (b.Count() == 0)
                    await DisplayAlert("Bultos", "No hay bultos disponibles en la ubicación", "OK");
                else
                    await Navigation.PushModalAsync(new BultosPage(b));

            }
                      );


            MessagingCenter.Subscribe<Bulto>(this, App.EVENT_SELECTED_BULTO, async b =>
            {
                model.Item.Bulto = b;
                txtCantidad.Focus();
            });
        }


        public ConsumoPage(Movimiento movimiento)
        {
            model = new MovimientoViewModel(movimiento);
            this.BindingContext = model;
            InitializeComponent();

        }

        private async void Deshacer_Clicked(object sender, EventArgs e)
        {
            
        }

        private async void TxtUbicacion_Completed(object sender, EventArgs e)
        {
             await model.GetBultosByUbicacionAsync(txtUbicacion.Text);
        }

        private async void TxtOrden_Completed(object sender, EventArgs e)
        {
            await model.GetOfAsync(txtOrden.Text);
        }
    }
}