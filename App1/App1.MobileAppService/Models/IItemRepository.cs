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



    public interface IAlmacen
    {
        Movimiento RegistrarSalida(int orden, int codBulto, decimal quantity);


        IEnumerable<Movimiento> GetMovimientosByUbicacion(int topmov);

        bool Logon(string username, string password);

        void AnularMovimiento(long movimientoId);

    }

    public class Movimiento
    {
        public long MovimientoId { get; set; }
        public long Orden { get; set; }
        public string Articulo { get; set; }
        public string Lote { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public System.DateTime Fecha { get; set; }

        public string UserName { get; set; }

    }

    public class Orden
    {
        public long OrdenId { get; set; }
        public string Articulo { get; set; }
        public string Descripcion { get; set; }

    }


    public class Articulo
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public string Clase { get; set; }
        public string Tipo { get; set; }

        public string UM { get; set; }
    }

    public class Bulto
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string UM { get; set; }
    }
}
