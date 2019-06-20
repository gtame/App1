using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App1.Models;
using Xamarin.Forms;

namespace App1.ViewModels
{
 

    public class MovimientoViewModel: BaseViewModel
    {

        public ICommand SubmitCommand { protected set; get; }
        public ICommand CleanCommand { protected set; get; }

        public ICommand GetOrdenCommand { protected set; get; }

        public ICommand GetUbicacionCommand { protected set; get; }

        public bool IsNewMode { get; private set; }

        public bool IsViewMode { get { return !IsNewMode; } }

        private bool _isrunning;
        public bool IsRunning { get { return _isrunning; } protected set { SetProperty(ref _isrunning, value); } }


        private Movimiento _movimiento;

        public Movimiento Item { get { return _movimiento; }
            set
            {
                SetProperty(ref _movimiento, value);
            }

        }


        public async Task<Of> GetOfAsync(string orden)
        {

            IsRunning = true;
            var ofs = await DataStore.GetOfAsync(orden);
            Item.Ofs = ofs;
            UbicacionIsFocused = true;
            IsRunning = false;
            return ofs;
        }

        public async Task<List<Bulto>> GetBultosByUbicacionAsync(string ubicacion)
        {
            IsRunning = true;

            var bultos = await DataStore.GetBultosByUbicacionAsync(ubicacion);
            List<Bulto> items = new List<Bulto>();
            foreach (var b in bultos)
                items.Add(b);

            if (items.Count == 1)
            {
                Item.Bulto = items[0];
                Item.Cantidad = Item.Bulto.Cantidad;
                CantidadIsFocused = true;
            }
            else
                MessagingCenter.Send<IEnumerable<Bulto>>(bultos, App.EVENT_LAUNCH_BULTOS_PAGE);


            IsRunning = false;
            return items;
        }

      

        public MovimientoViewModel(Movimiento item = null)
        {
            SubmitCommand = new Command<View>(OnSubmit);
            CleanCommand = new Command<View>(OnClean);
            GetOrdenCommand = new Command<string>(OnGetOrden);
            GetUbicacionCommand = new Command<string>(OnGetUbicacion);
            if (item == null)
            {
                Item = new Movimiento();
                IsNewMode = true;
            }
            else
                Item = item;
        }


        private bool _ordenIsFocused;
        public bool OrdenIsFocused {
            get { return _ordenIsFocused; }
            set {
           
                    
                    _ordenIsFocused = value;
                    OnPropertyChanged();
                
            }
        }


        private bool _ubicacionIsFocused;
        public bool UbicacionIsFocused
        {
            get { return _ubicacionIsFocused; }
            set
            {
                 

                    _ubicacionIsFocused = value;
                    OnPropertyChanged();
                
            }
        }


        private bool _cantidadIsFocused;
        public bool CantidadIsFocused
        {
            get { return _cantidadIsFocused; }
            set
            {

                    _cantidadIsFocused = value;
                    OnPropertyChanged();
             
            }
        }


        public async void OnGetUbicacion(string ubicacion)
        {
            List<Bulto> bultos = await GetBultosByUbicacionAsync(ubicacion);
            
        }

        public async void OnGetOrden(string orden)
        {
            Of ordenDB = await GetOfAsync(orden);
            
        }
        public async void OnSubmit(View view)
        {
            IsRunning = true;
            await Task.Delay(2000);
            OrdenIsFocused = true;
            IsRunning = false;
        }



        public async void OnClean(View view)
        {
            IsRunning = true;
            Item = new Movimiento();

            await Task.Delay(2000);
            //view?.Focus();
            OrdenIsFocused = true;
            IsRunning = false;
        }



    }
}
