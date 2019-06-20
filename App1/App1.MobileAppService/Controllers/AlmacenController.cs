using System;
using Microsoft.AspNetCore.Mvc;
using LogiData;
using App1.Models;
using LogiData.Data;
using Microsoft.Extensions.Configuration;

namespace App1.Controllers
{
    [Route("api/[controller]")]
    public class AlmacenController : Controller
    {

        private readonly IItemRepository ItemRepository;
        private readonly IConfigurationRoot Configuration;


        public AlmacenController(IItemRepository itemRepository, IConfigurationRoot configuration)
        {
            ItemRepository = itemRepository;
            Configuration = configuration;
            DataContext.ConnectionString= Configuration.GetConnectionString("logiteva");

        }



        //private   void LogonDB(string user, string pwd)
        //{

        //    using (System.Data.SqlClient.SqlConnection sql = new System.Data.SqlClient.SqlConnection("Data Source=eszazsharepro01;Initial Catalog=ALMACEN;Persist Security Info=True;User ID=dbalmacen;Password=dbalmacen12345.;Application Name=LogiTeva desktop;Connection Timeout=300"))
        //    {
        //        sql.Open();
        //        sql.Close();

        //    }
        //    string sqls = 
        //    LogiData.DataContext.ConnectionString = "";
        //    System.Data.SqlClient.SqlConnection sqlConnection = DataContext.Connection;

        //    dsUsuarios.USUARIOSDataTable dtUser = new dsUsuarios.USUARIOSDataTable();

        //    dtUser.FillByUSUARIO(user);
            
        //    if (dtUser.Rows.Count == 1)
        //    {
        //        if (!dtUser[0].CONTRASEÑA.Equals(pwd))
        //            throw new System.Exception("Contraseña incorrecta");
        //        else if (!dtUser[0].ACTIVO)
        //            throw new System.Exception("El usuario esta bloqueado");
        //    }
        //    else
        //        throw new System.Exception("Nombre de usuario incorrecto");

  
        //    dtUser = null;
        //}


        [HttpGet("Logon")]
        public IActionResult Logon(string username,string password)
        {
            try
            {
               GestionAlmacen.Logon(username, password);
                return Ok();
            }
            catch(System.Exception ex)
            {
                return NotFound();
            }

            
        }
        

        [HttpGet]
        public IActionResult List()
        {
            

            return Ok(ItemRepository.GetAll());
        }

        [HttpGet("{id}")]
        public Item GetItem(string id)
        {
            Item item = ItemRepository.Get(id);
            return item;
        }

        [HttpPost]
        public IActionResult Create([FromBody]Item item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }

                ItemRepository.Add(item);

            }
            catch (Exception)
            {
                return BadRequest("Error while creating");
            }
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] Item item)
        {
            try
            {
                if (item == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }
                ItemRepository.Update(item);
            }
            catch (Exception)
            {
                return BadRequest("Error while creating");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            ItemRepository.Remove(id);
        }
    }
}
