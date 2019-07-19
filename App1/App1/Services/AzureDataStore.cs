using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App1.Models;
using Xamarin.Forms;

namespace App1.Services
{
    public class AzureDataStore : IDataStore 
    {

        public static string UBICACION_SETTINGS = "LAST_UBICACION";


        HttpClient client;
        IEnumerable<Movimiento> movimientos;

        private string currentUser;
        private string currentUbicacion;

        public AzureDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

            if (Application.Current.Properties.ContainsKey(UBICACION_SETTINGS))
                currentUbicacion = (string)Application.Current.Properties[UBICACION_SETTINGS]; 
        }

        public async Task<IEnumerable<Bulto>> GetBultosByUbicacionAsync(string ubicacion)
        {


            HttpResponseMessage response = await client.GetAsync($"bultos?codubi={ubicacion}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Bulto>();
            }
            else

            {
                string json = await response.Content.ReadAsStringAsync();

                var bultos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Bulto>>(json));

                if (currentUbicacion != ubicacion)
                {
                    currentUbicacion = ubicacion;
                    //For Saving Value
                    Application.Current.Properties[UBICACION_SETTINGS] = currentUbicacion;
                    await Application.Current.SavePropertiesAsync();
                }
                return bultos;
            }

        }

        public async Task<IEnumerable<Bulto>> GetBultosByUbicacionAsync(string ubicacion,string articulo,string lote)
        {

            //bultosxlote?codubi=33044610&articulo=0206084&lote=CON0544581
            HttpResponseMessage response = await client.GetAsync($"bultosxlote?codubi={ubicacion}&articulo={articulo}&lote={lote}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Bulto>();
            }
            else

            {
                string json = await response.Content.ReadAsStringAsync();

                var bultos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Bulto>>(json));

                if (currentUbicacion != ubicacion)
                {
                    currentUbicacion = ubicacion;
                    //For Saving Value
                    Application.Current.Properties[UBICACION_SETTINGS] = currentUbicacion;
                    await Application.Current.SavePropertiesAsync();
                }
                return bultos;
            }

        }

        public async Task<Articulo> GetArticuloAsync(string articulo)
        {
            HttpResponseMessage response  =await client.GetAsync($"articulo?sku={articulo}");


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            else

            {
                string json = await response.Content.ReadAsStringAsync();
                var artDB = await Task.Run(() => JsonConvert.DeserializeObject<Articulo>(json));
                return artDB;

            }
           
        }

        public async Task<Of> GetOfAsync(string ofs)
        {
            HttpResponseMessage response = await client.GetAsync($"ofs?orden={ofs}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                string json = await response.Content.ReadAsStringAsync();
                var ofsDB = await Task.Run(() => JsonConvert.DeserializeObject<Of>(json));
                return ofsDB;
            }
        }


        public async Task<bool> LogonAsync(string username, string password)
        {

            var response = await client.GetAsync($"logon?username={username}&password={password}");
            if (response.IsSuccessStatusCode)
                currentUser = username;
            return response.IsSuccessStatusCode;
        }



        #region Movimiento

        public async Task<IEnumerable<Movimiento>> GetMovimientosAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"movimientos/{currentUbicacion}");
                movimientos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Movimiento>>(json));
            }

            return movimientos; 
        }

        public async Task<Movimiento> GetMovimientoAsync(long id)
        {
            
            var json = await client.GetStringAsync($"movimiento/{id}");
            return await  Task.Run
            (
                    () =>
                    {
                        List<Movimiento> movimientos = JsonConvert.DeserializeObject<List<Movimiento>>(json);
                        return movimientos[0];
                    }
             );

            


        }

        public async Task<bool> AddMovimientoAsync(Movimiento Movimiento)
        {
            if (Movimiento == null)
                return false;

            var serializedMovimiento = JsonConvert.SerializeObject(Movimiento);
            

            var response = await client.PostAsync($"addmovimiento/{currentUser}", new StringContent(serializedMovimiento, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string exception = await response.Content.ReadAsStringAsync();
                    throw new System.Exception(exception);
                }

                return false;
            }
            else
                return response.IsSuccessStatusCode;

     
        }
        
        public async Task<bool> Reubicar(string ubicacion, int bulto)
        {
            if (bulto == 0)
                return false;

            var response = await client.GetAsync($"reubicar/{ubicacion}/{bulto}/{currentUser}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string exception = await response.Content.ReadAsStringAsync();
                    throw new System.Exception(exception);
                }

                return false;
            }
            else
                return response.IsSuccessStatusCode;
        }
        //public async Task<bool> UpdateMovimientoAsync(Movimiento Movimiento)
        //{
        //    if (Movimiento == null || Movimiento.CodMov == 0)
        //        return false;

        //    var serializedMovimiento = JsonConvert.SerializeObject(Movimiento);
        //    var buffer = Encoding.UTF8.GetBytes(serializedMovimiento);
        //    var byteContent = new ByteArrayContent(buffer);

        //    var response = await client.PutAsync(new Uri($"api/Movimiento/{Movimiento.CodMov}"), byteContent);

        //    return response.IsSuccessStatusCode;
        //}

        public async Task<bool> DeleteMovimientoAsync(long id)
        {
            if (id==0)
                return false;

            var response = await client.DeleteAsync($"delmovimiento/{id}?username={currentUser}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string exception = await response.Content.ReadAsStringAsync();
                    throw new System.Exception(exception);
                }

                return false;
            }
            else
                return response.IsSuccessStatusCode;
        }
        #endregion  
    }
}