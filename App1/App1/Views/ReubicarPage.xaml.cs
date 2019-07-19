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
	public partial class ReubicarPage : ContentPage
	{
     

        MovimientoViewModel model;

        public ReubicarPage()
		{
            model = new MovimientoViewModel(null, MovementType.Reubicar);
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
                txtNewUbicacion.Focus();
            });
        }


        public ReubicarPage(Movimiento movimiento)
        {
            model = new MovimientoViewModel(movimiento);
            this.BindingContext = model;
            InitializeComponent();

        }

   
        private async void TxtUbicacion_Completed(object sender, EventArgs e)
        {
             await model.GetBultosByUbicacionAsync(txtUbicacion.Text);
        }

    }
}