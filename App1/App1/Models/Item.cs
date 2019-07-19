using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App1.Models
{

    public class BaseModel: INotifyPropertyChanged
    {

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class BaseItem:BaseModel
    {
        private string _codigo;
        private string _descripcion;
        public string Codigo { get { return _codigo; } set { SetProperty(ref _codigo, value); } }
        public string Descripcion { get { return _descripcion; } set { SetProperty(ref _descripcion, value); } }
    }


    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class Of : BaseItem
    {

           public string Articulo { get; set; }

           public Ubicacion Ubicacion { get; set; }

    }

    public class Articulo : BaseItem
    {
        public string Clase { get; set; }

        public string Tipo { get; set; }

        public string UM { get; set; }
    }


    public class Ubicacion : BaseItem
    {

    }



    public class Bulto : BaseItem
    {
        private int _cantidad;
        private string _lote;
        private Articulo _articulo;
        private Ubicacion _ubicacion;
        private string _um;

        public Articulo Articulo
        {

            get
            {
                if (_articulo == null)
                    _articulo = new Articulo();

                return _articulo;
            }
            set { SetProperty(ref _articulo, value); }
        }
        public Ubicacion Ubicacion
        {
            get
            {
                if (_ubicacion == null)
                    _ubicacion = new Ubicacion();
                return _ubicacion;
            }
            set { SetProperty(ref _ubicacion, value); }
        }
        public string Lote
        {
            get
            {
                return _lote;
            }
            set { SetProperty(ref _lote, value); }
        }

        public int Cantidad
        {
            get
            {
                return _cantidad;
            }
            set { SetProperty(ref _cantidad, value); OnPropertyChanged("TotalQty"); }
        }

        public string UM
        {

            get
            {
                return _um;
            }
            set { SetProperty(ref _um, value); OnPropertyChanged("TotalQty"); }
        }


        public string TotalQty
        {
            get
            {
                return string.Format("{0} {1}", Cantidad, Articulo.UM);
            }

        }
    }



    public class Movimiento: BaseModel
    {
        private Of _ofs;
        private Bulto _bulto;
        private Ubicacion _ubicacion;
        private int? _cantidad;
        private string _um;

        [JsonProperty(PropertyName = "MovimientoId")]
        public long CodMov { get; set; }

         
        public Bulto Bulto
        {
            get
            {
                if (_bulto == null)
                    _bulto = new Bulto();
                return _bulto;
            }
            set { SetProperty(ref _bulto, value);
                //Cantidad = _bulto.Cantidad;
                UM = _bulto.UM;
            }

        }


        public Ubicacion Ubicacion
        {
            get
            {
                if (_ubicacion == null)
                    _ubicacion = new Ubicacion();
                return _ubicacion;
            }
            set
            {
                SetProperty(ref _ubicacion, value);
             
            }
        }




        #region JsonProperties - para comunicar con API REST

        /*
                 {
  "movimientoId": 0,
  "bultoId": 0,
  "orden": 0,
  "articulo": "string",
  "lote": "string",
  "descripcion": "string",
  "cantidad": 0,
  "fecha": "2019-07-08T08:12:21.988Z",
  "userName": "string"
}
             
             */
        [JsonProperty(PropertyName = "BultoId")]
        public string BultoCode { get { return Bulto?.Codigo; } }


        [JsonProperty(PropertyName = "Orden")]
        public string OrdenCode { get { return Ofs?.Codigo; } }

        public string Articulo { get { return Bulto?.Articulo?.Codigo; }  }


        public string Descripcion { get { return Bulto?.Articulo?.Descripcion; } }



        public string Lote { get { return Bulto?.Lote; }  }
        #endregion

         
        public Of Ofs { get {
                if (_ofs == null)
                    _ofs = new Of();
                return _ofs; }
            set { SetProperty(ref _ofs, value); }

        }

        public string UM
        {
            get
            {
                return _um;
            }
            set { SetProperty(ref _um, value); }
        }

        public int? Cantidad
        {
            get
            {
                return _cantidad;
            }
            set { SetProperty(ref _cantidad, value); }
        }

        public string UserName { get; set; }

       public System.DateTime Fecha { get; set; }

    }

    public class User
    {
        public string Name { get; set; }
    }
}