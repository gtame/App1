using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App1.Models;

namespace App1.Services
{
    public interface IDataStore<T>
    {

        Task<Of> GetOfAsync(string orden);

        Task<Articulo> GetArticuloAsync(string articulo);

        Task<IEnumerable<Bulto>> GetBultosByUbicacionAsync(string ubicacion);

        Task<bool> LogonAsync(string username, string password);

        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);



        Task<bool> AddMovimientoAsync(Movimiento item);
        Task<bool> UpdateMovimientoAsync(Movimiento item);
        Task<bool> DeleteMovimientoAsync(long id);
        Task<Movimiento> GetMovimientoAsync(long id);

        Task<IEnumerable<Movimiento>> GetMovimientosAsync(bool forceRefresh = false);
    }
}
