using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App1.Models;
using App1.Services;
using Xamarin.Forms;

namespace App1.ViewModels
{
 
    public enum MovementType
    {
        Reubicar,
        Consumo,
 
    }


    public class MovimientoViewModel: BaseViewModel, ICodeReader
    {


        #region Properties

        #region Commands
        public ICommand ReubicarCommand { protected set; get; }

        public ICommand SubmitCommand { protected set; get; }

        public ICommand CleanCommand { protected set; get; }

        public ICommand DeleteCommand { protected set; get; }

        public ICommand GetOrdenCommand { protected set; get; }

        public ICommand GetUbicacionCommand { protected set; get; }

        public ICommand GetLoteCommand { protected set; get; }


        public ICommand GetArticuloCommand { protected set; get; }
        #endregion

        #region Modess
        public bool IsReubicarMode { get; private set; }

        public bool IsNewMode { get; private set; }

        public bool IsViewMode { get; private set; }


        public MovementType Type { get; private set; }

        #endregion


        #region isFocused


        private bool _loteIsFocused;

        public bool LoteIsFocused
        {
            get { return _loteIsFocused; }


            protected set
            {

                if (value)
                {
                    CantidadIsFocused = false;
                    UbicacionIsFocused = false;
                    OrdenIsFocused = false;
                    ArticuloIsFocused = false;
                    NewUbicacionIsFocused = false;
                }
                _loteIsFocused = value;
                OnPropertyChanged();

            }
        }

        private bool _newubicacionIsFocused;
        public bool NewUbicacionIsFocused
        {
            get { return _newubicacionIsFocused; }


            protected set
            {

                if (value)
                {
                    CantidadIsFocused = false;
                    UbicacionIsFocused = false;
                    OrdenIsFocused = false;
                    ArticuloIsFocused = false;
                    LoteIsFocused = false;
                }
                _newubicacionIsFocused = value;
                OnPropertyChanged();

            }
        }


        private bool _ordenIsFocused;
        public bool OrdenIsFocused
        {
            get { return _ordenIsFocused; }
            set
            {

                if (value)
                {
                    CantidadIsFocused = false;
                    UbicacionIsFocused = false;
                    NewUbicacionIsFocused = false;
                    ArticuloIsFocused = false;
                    LoteIsFocused = false;
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
                    NewUbicacionIsFocused = false;
                    ArticuloIsFocused = false;
                    LoteIsFocused = false;
                }
                _ubicacionIsFocused = value;
                OnPropertyChanged();

            }
        }



        private bool _articuloIsFocused;
        public bool ArticuloIsFocused
        {
            get { return _articuloIsFocused; }
            set
            {

                if (value)
                {
                    UbicacionIsFocused = false;
                    OrdenIsFocused = false;
                    NewUbicacionIsFocused = false;
                    CantidadIsFocused = false;
                    LoteIsFocused = false;
                }
                _articuloIsFocused = value;
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
                    NewUbicacionIsFocused = false;
                    ArticuloIsFocused = false;
                    LoteIsFocused = false;
                }
                _cantidadIsFocused = value;
                OnPropertyChanged();

            }
        }
        #endregion







        private bool _isrunning;
        public bool IsRunning { get { return _isrunning; } protected set { SetProperty(ref _isrunning, value); } }



        #region  BindingProperties

        
        public string NullableQty
        {
            get
            {
                if (!Item.Cantidad.HasValue) return "";
                else return Item.Cantidad.ToString();
                
            }
            set
            {
                int result;
                var success = int.TryParse(value, out result);
                Item.Cantidad = success ? (int?)result : null;
                OnPropertyChanged();
            }


        }
        
        private string _newubicacion;

        public string NewUbicacion { get { return _newubicacion; } set { SetProperty(ref _newubicacion, value); } }

        private Movimiento _movimiento;

        public Movimiento Item {
            get { return _movimiento; }
            set{  SetProperty(ref _movimiento, value); OnPropertyChanged("NullableQty"); }
        }



      
        #endregion  


        #endregion



        public MovimientoViewModel(Movimiento item = null, MovementType movement= MovementType.Consumo)
        {
            SubmitCommand = new Command(OnSubmit);
            CleanCommand = new Command(OnClean);
            GetOrdenCommand = new Command<string>(OnGetOrden);
            GetUbicacionCommand = new Command<string>(OnGetUbicacion);
            ReubicarCommand = new Command(OnReubicar);
            DeleteCommand = new Command(OnDelete);
            GetArticuloCommand = new Command<string>(OnGetArticulo);
            GetLoteCommand = new Command<string>(OnGetLote);


            Type = movement;
            if (movement == MovementType.Consumo)
            {
                if (item == null)
                {
                    Item = new Movimiento();
                    IsNewMode = true;
                    Title = "Registrar Consumo";
                    OrdenIsFocused = true;
                }
                else
                {
                    Item = item;
                    Title = "Eliminar consumo";
                    IsViewMode = true;
                }
            }
            else if (movement == MovementType.Reubicar)
            {
                IsReubicarMode = true;
                if (item == null)
                {
                    Item = new Movimiento();
                    IsNewMode = true;
                    Title = "Reubicar";
                    IsNewMode = true;
                    UbicacionIsFocused = true;
                }
                else
                {
                    Item = item;
                    Title = "Visualizar reubicacion";
                    IsViewMode = true;
                }
            }
        }


   

        #region CommandMethods




        public async Task<Of> GetOfAsync(string orden)
        {

            IsRunning = true;
            var ofs = await DataStore.GetOfAsync(orden);
            Item.Ofs = ofs;
            if (ofs == null)
            {
                await AlertHelper.ShowError("La orden seleccionada no existe");

                OrdenIsFocused = true;
            }
            else
            {
                if (ofs.Ubicacion == null)
                {

                    await AlertHelper.ShowError("Debe especificar una ubicación");
                    UbicacionIsFocused = true;
                }
                else
                {
                    Item.Ubicacion = ofs.Ubicacion;
                    ArticuloIsFocused = true;

                }
            }

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
                //Item.Cantidad = Item.Bulto.Cantidad;
                CantidadIsFocused = true;
            }
            else
                MessagingCenter.Send<IEnumerable<Bulto>>(bultos, App.EVENT_LAUNCH_BULTOS_PAGE);


            IsRunning = false;
            return items;
        }



        public async void OnGetLote(string lote)
        {
            await OnGetLotes(  lote);
        }

        public async Task<List<Bulto>> OnGetLotes(string lote)
        {
            IsRunning = true;
            
            var bultos = await DataStore.GetBultosByUbicacionAsync(Item.Ubicacion.Codigo,Item.Bulto.Articulo.Codigo,lote);
            List<Bulto> items = new List<Bulto>();
            foreach (var b in bultos)
                items.Add(b);


            if (items.Count == 0)
            {
                await AlertHelper.ShowError("El lote seleccionado no se encuentra en la ubicacion");
                LoteIsFocused = true;
            }
            else if (items.Count == 1)
            {
                Item.Bulto = items[0];
                //Item.Cantidad = Item.Bulto.Cantidad;
                CantidadIsFocused = true;
            }
            else
                MessagingCenter.Send<IEnumerable<Bulto>>(bultos, App.EVENT_LAUNCH_BULTOS_PAGE);


            IsRunning = false;
            return items;
            
        }


        public async void OnGetUbicacion(string ubicacion)
        {
            List<Bulto> bultos = await GetBultosByUbicacionAsync(ubicacion);
            
        }

        public async void OnGetArticulo(string articulo)
        {
            IsRunning = true;
            Articulo artDb = await DataStore.GetArticuloAsync(articulo);
            Item.Bulto.Articulo = artDb;
            if (artDb == null)
            {
                await AlertHelper.ShowError($"El articulo {articulo} indicado no existe");
                ArticuloIsFocused = true;
            }
            else
            {

                if (string.IsNullOrEmpty(Item.Bulto.Lote))
                    LoteIsFocused = true;
                else
                    CantidadIsFocused = true;
            }

            IsRunning = false;
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


        public async void OnReubicar()
        {
            IsRunning = true;

            try
            {
                bool result = await DataStore.Reubicar(NewUbicacion, int.Parse( Item.Bulto.Codigo));

                Item = new Movimiento();
                //await Task.Delay(2000);
                UbicacionIsFocused = true;

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
            NewUbicacion = null;
            //await Task.Delay(2000);
            //view?.Focus();

            if (IsReubicarMode)
                UbicacionIsFocused = true;
            else
                OrdenIsFocused = true;

            IsRunning = false;
        }

        async void ICodeReader.OnReadCodeBar(string text)
        {
            if (OrdenIsFocused)
            {
                Of ofs=await GetOfAsync(text);
            }
        }

        #endregion




    }
}
