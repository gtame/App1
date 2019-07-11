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

        public ICommand DeleteCommand { protected set; get; }

        public ICommand GetOrdenCommand { protected set; get; }

        public ICommand GetUbicacionCommand { protected set; get; }

        public bool IsNewMode { get; private set; }

        public bool IsViewMode { get { return !IsNewMode; } }

        private bool _isrunning;
        public bool IsRunning { get { return _isrunning; } protected set { SetProperty(ref _isrunning, value); } }


        private Movimiento _movimiento;

        public Movimiento Item {
            get { return _movimiento; }
            set{  SetProperty(ref _movimiento, value); }
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
            SubmitCommand = new Command(OnSubmit);
            CleanCommand = new Command(OnClean);
            GetOrdenCommand = new Command<string>(OnGetOrden);
            GetUbicacionCommand = new Command<string>(OnGetUbicacion);

            DeleteCommand = new Command(OnDelete);
            if (item == null)
            {
                Item = new Movimiento();
                IsNewMode = true;
                Title = "Registrar Consumo";
            }
            else
            {
                Item = item;
                Title = "Eliminar consumo";
            }
        }


        private bool _ordenIsFocused;
        public bool OrdenIsFocused {
            get { return _ordenIsFocused; }
            set {

                    if (value)
                    {
                        CantidadIsFocused = false;
                        UbicacionIsFocused = false;
                    }
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

                if (value)
                {
                    CantidadIsFocused = false;
                    OrdenIsFocused = false;
                }
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

                    if (value)
                    {
                        UbicacionIsFocused = false;
                        OrdenIsFocused = false;
                    }
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
        public async void OnSubmit()
        {
            IsRunning = true;

            try
            {
                bool result = await DataStore.AddMovimientoAsync(Item);


                Item = new Movimiento();
                //await Task.Delay(2000);
                OrdenIsFocused = true;
                
            }
            catch(System.Exception ex)
            {
                await AlertHelper.ShowError(ex.Message);
            }
            finally
            {
                IsRunning = false;
            }
        }



        public async void OnDelete()
        {
            bool ok = await AlertHelper.DisplayAlert("Deshacer", "¿Esta seguro que desea deshacer el consumo?", "Ejecutar", "Cancelar");



            if (ok)
            {




                IsRunning = true;

                try
                {
                    bool result = await DataStore.DeleteMovimientoAsync(Item.CodMov);
                    //model delete
                    await Navigation.PopAsync();

                }
                catch (System.Exception ex)
                {
                    await AlertHelper.ShowError(ex.Message);
                }
                finally
                {
                    IsRunning = false;
                }
            }
        }

        public async void OnClean()
        {
            IsRunning = true;
            Item = new Movimiento();

            //await Task.Delay(2000);
            //view?.Focus();
            OrdenIsFocused = true;
            IsRunning = false;
        }



    }
}
