using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class BultosViewModel: BaseViewModel
    {
        private Bulto _selected;
        private List<Bulto> _bultos;
        public List<Bulto> Bultos { get { return _bultos; } private set { _bultos = value; OnPropertyChanged(); } }

        public ICommand LoadBultosByUbicacionCommand { get; private set; }

        public string Ubicacion { get; set; }
        public BultosViewModel()
        {
            LoadBultosByUbicacionCommand = new Command<string>(async (s) => { Bultos = await LoadBultosByUbicacionAsync(s); });
            _bultos = new List<Bulto>();
        }

        public BultosViewModel(IEnumerable<Bulto> bultos)
        {
            _bultos = new List<Bulto>();
            foreach (var b in bultos)
                _bultos.Add(b);

            LoadBultosByUbicacionCommand = new Command<string>( async (s) => { Bultos = await LoadBultosByUbicacionAsync(s); }); 
        }


        
        public Bulto SelectedBulto { get { return _selected; }
            set
            {
                _selected = value;
                MessagingCenter.Send<Bulto>(_selected, App.EVENT_SELECTED_BULTO);

            }
        }

        public async Task<List<Bulto>> LoadBultosByUbicacionAsync(string ubicacion)
        {
             
            IsBusy = true;

            var bultos = await DataStore.GetBultosByUbicacionAsync(ubicacion);

            List<Bulto> _mbulto = new List<Bulto>();
            foreach (var b in bultos)
                _mbulto.Add(b);
            

            IsBusy = false;

            return _mbulto;
        }

    }
}
