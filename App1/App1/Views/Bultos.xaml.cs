using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App1.Models;
using App1.Views;
using App1.ViewModels;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BultosPage : ContentPage
    {
        BultosViewModel viewModel;

       

        public BultosPage(IEnumerable<Bulto> bultos)
        {
            BindingContext = viewModel = new BultosViewModel(bultos);
            InitializeComponent();
            
        }

        public BultosPage()
        {
           
            BindingContext = viewModel = new BultosViewModel();
            viewModel.Ubicacion  = "001";
            InitializeComponent();


        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            viewModel.SelectedBulto= args.SelectedItem as Bulto;
            await Navigation.PopModalAsync();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConsumoPage() );
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Bultos.Count()==0)
                viewModel.LoadBultosByUbicacionCommand.Execute(null);
        }


    }
}