using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class BultosViewModel: BaseViewModel
    {
        public IEnumerable<Bulto> Bultos { get; set; }

        public ICommand LoadBultosByUbicacionCommand { get; }

        public BultosViewModel()
        {
            LoadBultosByUbicacionCommand = new Command<string>(LoadBultosByUbicacion); ; 
        }

        public BultosViewModel(IEnumerable<Bulto> bultos)
        {
           
        }

        public Bulto SelectedBulto { get; set; }

        public async void LoadBultosByUbicacion(string ubicacion)
        {
            
        }

    }
}
