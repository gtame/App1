using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App1.Models;

namespace App1.Services
{
    public class AzureDataStore : IDataStore<Item>
    {
        HttpClient client;
        IEnumerable<Item> items;
        IEnumerable<Movimiento> movimientos;

        public AzureDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

            items = new List<Item>();
        }

        public async Task<IEnumerable<Bulto>> GetBultosByUbicacionAsync(string ubicacion)
        {
            
            var json = await client.GetStringAsync($"bultos?codubi={ubicacion}");
            var bultos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Bulto>>(json));

            return bultos;

        }


        public async Task<Articulo> GetArticuloAsync(string articulo)
        {
            var json = await client.GetStringAsync($"articulo?sku={articulo}");
            var artDB = await Task.Run(() => JsonConvert.DeserializeObject<Articulo>(json));
            return artDB;
        }

        public async Task<Of> GetOfAsync(string ofs)
        {
            var json = await client.GetStringAsync($"ofs?orden={ofs}");
            var ofsDB = await Task.Run(() => JsonConvert.DeserializeObject<Of>(json));
            return ofsDB;
        }
        public async Task<bool> LogonAsync(string username, string password)
        {

            var response = await client.GetAsync($"logon?username={username}&password={password}");

            return response.IsSuccessStatusCode;
        }


        #region Item
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"api/item");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
            }

            return items;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null)
            {
                var json = await client.GetStringAsync($"api/item/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Item>(json));
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }


  
        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            var response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode;
        }

        #endregion


        #region Movimiento

        public async Task<IEnumerable<Movimiento>> GetMovimientosAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"api/Movimiento");
                movimientos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(json));
            }

            return movimientos; 
        }

        public async Task<Movimiento> GetMovimientoAsync(long id)
        {
            if (id != null)
            {
                var json = await client.GetStringAsync($"api/Movimiento/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Movimiento>(json));
            }

            return null;
        }

        public async Task<bool> AddMovimientoAsync(Movimiento Movimiento)
        {
            if (Movimiento == null)
                return false;

            var serializedMovimiento = JsonConvert.SerializeObject(Movimiento);

            var response = await client.PostAsync($"api/Movimiento", new StringContent(serializedMovimiento, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> UpdateMovimientoAsync(Movimiento Movimiento)
        {
            if (Movimiento == null || Movimiento.CodMov == 0)
                return false;

            var serializedMovimiento = JsonConvert.SerializeObject(Movimiento);
            var buffer = Encoding.UTF8.GetBytes(serializedMovimiento);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/Movimiento/{Movimiento.CodMov}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMovimientoAsync(long id)
        {
            if (id==0)
                return false;

            var response = await client.DeleteAsync($"api/Movimiento/{id}");

            return response.IsSuccessStatusCode;
        }
        #endregion  
    }
}