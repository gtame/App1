using System;
using Microsoft.AspNetCore.Mvc;
using LogiData;
using App1.Models;
using LogiData.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

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

            if (DataContext.ConnectionString ==null)
                DataContext.ConnectionString= Configuration.GetConnectionString("logiteva");

        }
         

        [HttpGet("logon")]
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


        [HttpGet("bultosxlote")]
        public IActionResult GetByCodUbicacion(string codubi,string articulo,string lote)
        {

            dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
            dtBultos.FillByUbicacion(codubi);
            if (dtBultos.Count == 0)
                return NotFound();
            else
            {

                List<Bulto> lstMov = new List<Bulto>();
                foreach (dsAlmacen.BULTOSRow row in dtBultos.Rows)
                {
                    //Buscamos los bultos de esa ubicacion que contengan ese articulo -- lote
                    if (!row.IsLOTENull()  && row.LOTE==lote && row.ARTICULO==articulo)
                            lstMov.Add(ParseBulto(row));
                }


                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(lstMov);
                return StatusCode(200, JSONresult);
            }

        }


        [HttpGet("bultos")]
        public IActionResult GetByCodUbicacion(string codubi)
        {

            dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
            dtBultos.FillByUbicacion(codubi);
            if (dtBultos.Count == 0)
                return NotFound();
            else
            {

                List<Bulto> lstMov = new List<Bulto>();
                foreach (dsAlmacen.BULTOSRow row in dtBultos.Rows)
                {
                    lstMov.Add(ParseBulto(row));
                }


                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(lstMov);
                return StatusCode(200, JSONresult);
            }
            
        }

        [HttpGet("articulo")]
        public IActionResult GetArticuloBySKU(string sku)
        {
            dsAlmacen.ARTICULOSDataTable dtArticulos = new dsAlmacen.ARTICULOSDataTable();
            dtArticulos.FillByORACLE_CODE(sku);
            if (dtArticulos.Count == 0)
                return NotFound();
            else
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(Articulo.Parse(dtArticulos[0]));
                return StatusCode(200, JSONresult);
            }
        }



        [HttpGet("ofs")]
        public  IActionResult GetOfsByCodigo(string orden)
        {
            dsOfs.OFSDataTable dtOfs = new dsOfs.OFSDataTable();
            dtOfs.FillByOfs(int.Parse(orden));
            if (dtOfs.Count == 0)
                return NotFound();
            else
            {
                string JSONresult;
                JSONresult =   JsonConvert.SerializeObject(ParseOrden(dtOfs[0]));




                return StatusCode(200, JSONresult);
            }
        }


        private   Orden ParseOrden (dsOfs.OFSRow row)
        {
            string centro = null;
            string ubicacion = null;
            Orden ofs = Orden.Parse(row);
            //Obtenemos maquina desde ESP Planning
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(Configuration.GetConnectionString("esp_data")))
            {

                sqlConnection.Open();
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandText = @"SELECT [CENTRO] FROM [dbo].[OFS_PLANIFICACION_ACOND] WHERE ARTICULO=@ARTICULO AND LOTE=@LOTE";
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("ARTICULO", SqlDbType.NVarChar, 50));
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("LOTE", SqlDbType.NVarChar, 50));
                    command.Parameters["ARTICULO"].Value = ofs.Articulo;
                    command.Parameters["LOTE"].Value = row.LOTE;

                    centro = (string)command.ExecuteScalar();
                }
            }

            //--Mapping entre ubicaciones y centros
            ubicacion = "M1";

            dsAlmacen.UBICACIONESDataTable dtUbicacion = new dsAlmacen.UBICACIONESDataTable();
            dtUbicacion.FillByCODUBI(ubicacion);

            if (dtUbicacion.Count == 1)
                ofs.Ubicacion = Ubicacion.Parse(dtUbicacion[0]);
            
            return ofs;
        }

        [HttpGet("movimiento/{movimiento}")]
        public IActionResult GetMovimientoByCodigo(int movimiento)
        {
            dsAlmacen.MOVIMIENTOSDataTable dtMovimientos = new dsAlmacen.MOVIMIENTOSDataTable();

            dtMovimientos.FillByCODMOV(movimiento);
            if (dtMovimientos.Count == 0)
                return NotFound();
            else
            {
                string JSONresult;
               

                JSONresult = JsonConvert.SerializeObject(ParseMovimiento(dtMovimientos[0]));
                return StatusCode(200, JSONresult);
            }
        }

       


         private Bulto ParseBulto(dsAlmacen.BULTOSRow row)
        {
            Bulto bulto = Bulto.Parse(row);

            dsAlmacen.ARTICULOSDataTable dtArticulo = new dsAlmacen.ARTICULOSDataTable();
            dtArticulo.FillByORACLE_CODE(row.ARTICULO);
            if (dtArticulo.Count == 1)
                bulto.Articulo = Articulo.Parse(dtArticulo[0]);

            dsAlmacen.UBICACIONESDataTable dtUbicacion = new dsAlmacen.UBICACIONESDataTable();
            dtUbicacion.FillByCODUBI(bulto.Ubicacion.Codigo);

            if (dtUbicacion.Count ==1 )
                bulto.Ubicacion = Ubicacion.Parse(dtUbicacion[0]);

            return bulto;
        }

        private Movimiento ParseMovimiento(dsAlmacen.MOVIMIENTOSRow row)
        {
            Movimiento mov = Movimiento.Parse(row);

            dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
            dtBultos.FillByCODBULTO(mov.BultoId);
            if (dtBultos.Count == 1)
            {
                mov.Articulo = dtBultos[0].ARTICULO;
                mov.Lote = dtBultos[0].LOTE;
                mov.Descripcion = dtBultos[0].TEXT;

                 
            }


            dsAlmacen.UBICACIONESDataTable dtUbicacion = new dsAlmacen.UBICACIONESDataTable();
            dtUbicacion.FillByCODUBI(mov.UbicacionCode);

            if (dtUbicacion.Count == 1)
                mov.Ubicacion = Ubicacion.Parse(dtUbicacion[0]);

            return mov;
        }



        private Movimiento ParseSQLRow(DataRow row)
        {
            Movimiento mov = new Movimiento()
            {
                Articulo = (string)row["ARTICULO"],
                Descripcion = (string)row["TEXT"],
                BultoId = (int)row["CODBULTO"],
                Cantidad = Decimal.ToInt32((decimal)row["CANTIDAD"]),
                Fecha = (System.DateTime)row["F_MOV"],
                Lote = (string)row["LOTE"],
                Orden = (row["OFS"] == System.DBNull.Value ? null : row["OFS"].ToString()),
                MovimientoId = (int)row["CODMOV"],
                UserName = (string)row["TERMINAL"],
                UbicacionCode = (string)row["CODUBI"]

            };

            dsAlmacen.UBICACIONESDataTable dtUbicacion = new dsAlmacen.UBICACIONESDataTable();
            dtUbicacion.FillByCODUBI(mov.UbicacionCode);

            if (dtUbicacion.Count == 1)
            {
                mov.Ubicacion = Ubicacion.Parse(dtUbicacion[0]);
            }

            if (mov.Orden != null)
            {
                dsOfs.OFSDataTable dtOfs = new dsOfs.OFSDataTable();
                dtOfs.FillByOfs(int.Parse(mov.Orden));

                if (dtOfs.Count == 1)
                {
                    mov.Ofs = Orden.Parse(dtOfs[0]);
                }
            }

            dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
            dtBultos.FillByCODBULTO(mov.BultoId);

            if (dtBultos.Count == 1)
            {
                mov.Bulto = ParseBulto(dtBultos[0]);

                mov.Bulto.Ubicacion = mov.Ubicacion;
            }


            return mov;
        }

        [HttpGet("movimientos/{ubicacion}")]
        public IActionResult GetMovimientos(string ubicacion, int toprows=10)
        {
            LogiData.Data.dsAlmacenTableAdapters.MOVIMIENTOSAdapter movimientoAdapter = new LogiData.Data.dsAlmacenTableAdapters.MOVIMIENTOSAdapter();

            System.Data.DataTable dtMovimientos = movimientoAdapter.GetMovimientosHistorico(toprows, ubicacion);
            if (dtMovimientos.Rows.Count > 0)
            {
                List<Movimiento> lstMov = new List<Movimiento>();
                foreach (DataRow row in dtMovimientos.Rows)
                {
                    lstMov.Add(ParseSQLRow(row));
                }

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(lstMov);
                return StatusCode(200, JSONresult);
            }
            else
                return NotFound();
        }

        [HttpPost("addmovimiento/{username}")]
        public IActionResult Create(string username,[FromBody]Movimiento item)
        {
            int codMov = 0;
            try
            {

                SourceDoc source = new SourceDoc();
                source.Id = item.Orden;
                source.Type = DocType.OFS;
                source["OFS"] = item.Orden;
                codMov = LogiData.GestionAlmacen.Salida(item.BultoId, item.Cantidad, username , false, "Consumo en linea", source);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
            return Ok(codMov);
        }



        [HttpGet("reubicar/{ubicacion}/{bulto}/{username}")]
        public IActionResult Reubicar(string ubicacion, int bulto,string username)
        {
            int codMov = 0;
            try
            {

                dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
                dtBultos.FillByCODBULTO(bulto);
                if (dtBultos.Count==1)
                {
                     LogiData.GestionAlmacen.Reubicar(bulto,ubicacion,dtBultos[0].CANTIDAD,  username);
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
            return Ok(codMov);
        }



        //[HttpPut]
        //public IActionResult Edit([FromBody] Item item)
        //{
        //    try
        //    {
        //        if (item == null || !ModelState.IsValid)
        //        {
        //            return BadRequest("Invalid State");
        //        }
        //        ItemRepository.Update(item);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Error while creating");
        //    }
        //    return NoContent();
        //}

        [HttpDelete("delmovimiento/{id}")]
        public IActionResult Delete(string id,string username)
        {
            int codMov = 0;
            try
            {

                dsAlmacen.MOVIMIENTOSDataTable dtMovimientos = new dsAlmacen.MOVIMIENTOSDataTable();
                dtMovimientos.FillByCODMOV(int.Parse(id));

                

                if (dtMovimientos.Count == 1)
                {

                    if (!dtMovimientos[0].ANULADO)
                    {
                        Movimiento movRetorno = ParseMovimiento(dtMovimientos[0]);

                        dsAlmacen.BULTOSDataTable dtBultos = new dsAlmacen.BULTOSDataTable();
                        dtBultos.FillByCODBULTO(dtMovimientos[0].CODBULTO);


                        //Hace el retorno contra esa orden..
                        codMov = LogiData.GestionAlmacen.Retorno(movRetorno.UbicacionCode, movRetorno.Articulo, movRetorno.Descripcion, movRetorno.Lote, (System.DateTime?)(dtBultos[0].IsCADUCIDADNull() ? null : (System.DateTime?)dtBultos[0].CADUCIDAD), movRetorno.Cantidad, true, false, username, movRetorno.Orden);

                        //Lo marca como anulado
                        dtMovimientos[0].ANULADO = true;
                        dtMovimientos.Update();
                    }
                    else
                        throw new System.Exception("El movimiento ya esta anulado");

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return Ok(codMov);
        }
    }
}
