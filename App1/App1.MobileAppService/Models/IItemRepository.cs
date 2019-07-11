using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace App1.Models
{
    public interface IItemRepository
    {
        void Add(Item item);
        void Update(Item item);
        Item Remove(string key);
        Item Get(string id);
        IEnumerable<Item> GetAll();
    }





    public class Ubicacion  
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }


        public static Ubicacion Parse(LogiData.Data.dsAlmacen.UBICACIONESRow row)
        {
            Ubicacion b = new Ubicacion()
            {

                Codigo = row.CODUBI.ToString(),
                Descripcion = row.ToString()
            };
            return b;
        }
    }



    public class Movimiento
    {
        public long MovimientoId { get; set; }

        public int BultoId { get; set; }

        public string Orden { get; set; }

        public string Articulo { get; set; }
        public string Lote { get; set; }


        

        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public System.DateTime Fecha { get; set; }

        [JsonIgnore]
        public string UbicacionCode { get; set; }

        public Ubicacion Ubicacion { get; set; }


        public Orden Ofs { get; set; }

        public Bulto Bulto { get; set; }

        public string UserName { get; set; }


        public static Movimiento Parse(LogiData.Data.dsAlmacen.MOVIMIENTOSRow  row)
        {
            Movimiento b = new Movimiento()
            {

                MovimientoId = row.CODMOV,
                BultoId = row.CODBULTO,
                Orden =( row.IsOFSNull()?string.Empty:row.OFS),
                Cantidad = (int) row.CANTIDAD,
                Fecha = row.F_MOV,
                UbicacionCode = row.CODUBI,
                
                UserName = row.TERMINAL
                

            };
            return b;
        }

    }

    public class Orden
    {
        public string Codigo { get; set; }
        public string Articulo { get; set; }
        public string Descripcion { get; set; }



        public static Orden Parse(LogiData.Data.dsOfs.OFSRow row)
        {
            Orden b = new Orden()
            {

                Codigo = row.OFS,
                Descripcion = row.DESCRIPTION,
                Articulo= row.ARTICULO

            };
            return b;
        }

    }


    public class Articulo
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public string Clase { get; set; }
        public string Tipo { get; set; }

        public string UM { get; set; }


        public static Articulo Parse(LogiData.Data.dsAlmacen.ARTICULOSRow row)
        {
            Articulo b = new Articulo()
            {

                Codigo = row.ORACLE_CODE.ToString(),
                Descripcion = row.DESCRIPTION,
                Clase = row.CLASE,
                Tipo = row.TIPO,
                UM = row.UM
                
                
            };
            return b;
        }
    }

    public class Bulto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public string UM { get; set; }

        public string Lote { get; set; }


        public Ubicacion Ubicacion
        {
            get;set;
        }

        public Articulo Articulo
        {

            get;set;
        }
     
        public static Bulto Parse(LogiData.Data.dsAlmacen.BULTOSRow row)
        {
            Bulto b = new Bulto()
            {

                Codigo = row.CODBULTO.ToString(),
                Descripcion = row.TEXT,
                Cantidad = Decimal.ToInt32(row.CANTIDAD),
                Lote = (row.IsLOTENull() ? string.Empty: row.LOTE),
                
            };
            b.Articulo = new Articulo();
            b.Articulo.Codigo = row.ARTICULO;

            b.Ubicacion = new Ubicacion();
            b.Ubicacion.Codigo = row.CODUBI;

            return b;
        }
    }
}
