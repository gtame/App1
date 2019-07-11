using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App1.Models;

namespace App1.Services
{
    public class MockDataStore : IDataStore
    {

        List<Movimiento> movimientos;
        List<Item> items;
        List<Articulo> articulos;
        List<Of> ofs;
        List<Bulto> bultos;
        List<Ubicacion> ubicaciones;
        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };


            articulos = new List<Articulo>();
            var mockArticulos = new List<Articulo>
            {
                new Articulo { Codigo="1111111" , Descripcion="Omepra 20 - 28" },
                new Articulo { Codigo="2222222" , Descripcion="Aluminio....12mm" },
                new Articulo { Codigo="3333333" , Descripcion="Aluminio....20mm" },
                new Articulo { Codigo="4444444" , Descripcion="Aluminio....40mm" },
            };

            ubicaciones = new List<Ubicacion>();
            var mockUbicaciones = new List<Ubicacion>
            {
                new Ubicacion { Codigo="001", Descripcion="Ubicacion 001" },
                new Ubicacion {  Codigo="002", Descripcion="Ubicacion 002"},
                new Ubicacion {  Codigo="003", Descripcion="Ubicacion 003"},
                new Ubicacion {  Codigo="004", Descripcion="Ubicacion 004"},
            }; 

            

            ofs = new List<Of>();
            var mockOfs = new List<Of>
            {
                new Of {Codigo="1", Descripcion=$"Ofs Codigo 1" },
                new Of {Codigo="2", Descripcion=$"Ofs Codigo 2" }

            };


            bultos = new List<Bulto>();
            var mockBultos = new List<Bulto>()
            {
                new Bulto { Codigo="1", Articulo =mockArticulos[0],Ubicacion=mockUbicaciones[0],  Descripcion=mockArticulos[0].Descripcion, Cantidad=1, Lote="LOT-001", UM="M" },
                new Bulto { Codigo="2", Articulo =mockArticulos[1],Ubicacion=mockUbicaciones[1], Descripcion=mockArticulos[0].Descripcion, Cantidad=1, Lote="LOT-002", UM="M" },
                new Bulto { Codigo="2", Articulo =mockArticulos[1],Ubicacion=mockUbicaciones[1], Descripcion=mockArticulos[0].Descripcion, Cantidad=1, Lote="LOT-003", UM="M" },

            };


            movimientos = new List<Movimiento>();
            var mockMovimientos = new List<Movimiento>()
            {
                new Movimiento{ CodMov=1, Bulto =mockBultos[0], Cantidad=2, UM="M", Fecha=System.DateTime.Now, Ofs=mockOfs[0], UserName="Auto" },
                new Movimiento{ CodMov=2, Bulto =mockBultos[1], Cantidad=3, UM="M", Fecha=System.DateTime.Now, Ofs=mockOfs[1], UserName="Auto" },
                new Movimiento{ CodMov=3, Bulto =mockBultos[2], Cantidad=4, UM="M", Fecha=System.DateTime.Now, Ofs=mockOfs[0], UserName="Auto" },
                new Movimiento{ CodMov=4, Bulto =mockBultos[0], Cantidad=5, UM="M", Fecha=System.DateTime.Now, Ofs=mockOfs[1], UserName="Auto" },
                new Movimiento{ CodMov=5, Bulto =mockBultos[1], Cantidad=26, UM="M", Fecha=System.DateTime.Now, Ofs=mockOfs[0], UserName="Auto" },
            };


            foreach (var ubicacion in mockUbicaciones)
            {
                ubicaciones.Add(ubicacion);
            }


            foreach (var item in mockItems)
            {
                items.Add(item);
            }

            foreach (var articulo in mockArticulos)
            {
                articulos.Add(articulo);
            }


            foreach (var of in mockOfs)
            {
                ofs.Add(of);
            }



            foreach (var bulto in mockBultos)
            {
                bultos.Add(bulto);
            }

            foreach (var movimiento in mockMovimientos)
                movimientos.Add(movimiento);
        }



        public async Task<IEnumerable<Bulto>> GetBultosByUbicacionAsync(string ubicacion)
        {
            await Task.Delay(2000);
            //arg.Ubicacion.Codigo == ubicacion
            return await Task.FromResult< IEnumerable < Bulto >>( bultos.Where((Bulto arg) => arg.Ubicacion.Codigo==ubicacion));

        }

        public async Task<Articulo> GetArticuloAsync(string articulo)
        {
            
            await Task.Delay(2000);
            return articulos.Where((Articulo arg) => arg.Codigo == articulo).FirstOrDefault();
     
        }


        public async Task<Of> GetOfAsync(string  orden)
        {

            await Task.Delay(2000);
            return ofs.Where((Of arg) => arg.Codigo == orden).FirstOrDefault();

        }


        public async Task<bool> LogonAsync(string username, string password)
        {
            await Task.Delay(2000);
            return await Task.FromResult(username == "admin" && password == "admin");

        }

        #region Item

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
        #endregion

        #region Movimiento

        public async Task<bool> AddMovimientoAsync(Movimiento Movimiento)
        {
            movimientos.Add(Movimiento);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateMovimientoAsync(Movimiento Movimiento)
        {
            var oldMovimiento = movimientos.Where((Movimiento arg) => arg.CodMov == Movimiento.CodMov).FirstOrDefault();
            movimientos.Remove(oldMovimiento);
            movimientos.Add(Movimiento);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteMovimientoAsync(long codMov)
        {
            var oldMovimiento = movimientos.Where((Movimiento arg) => arg.CodMov == codMov).FirstOrDefault();
            movimientos.Remove(oldMovimiento);

            return await Task.FromResult(true);
        }

        public async Task<Movimiento> GetMovimientoAsync(long id)
        {
            return await Task.FromResult(movimientos.FirstOrDefault(s => s.CodMov == id));
        }

        public async Task<IEnumerable<Movimiento>> GetMovimientosAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(movimientos);
        }
        #endregion
    }
}