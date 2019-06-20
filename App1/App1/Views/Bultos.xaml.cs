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

        public BultosPage(IEnumerable<Bulto> bultos )
        {
            InitializeComponent();
            BindingContext = viewModel = new BultosViewModel(bultos);
        }

        public BultosPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new BultosViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Movimiento;
            if (item == null)
                return;

            await Navigation.PushAsync(new ConsumoPage(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConsumoPage() );
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Bultos.Count() == 0)
                viewModel.LoadBultosByUbicacionCommand.Execute();
        }
    }
}