//using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using TiendaTecno.WebServicesJava;
using TiendaTecno.WebServiceC;
using System.Web.Services;
using System.Globalization;

namespace TiendaTecno.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            CompraBanco();
            VentaBanco();
            string valorCompra = CompraBanco();
            string valorVenta = VentaBanco();
            ViewBag.ValorCompra = valorCompra;
            ViewBag.ValorVenta = valorVenta;
            return View();
        }
        public ActionResult CarritoCompra()
        {
            return View();
        }
        public ActionResult DetalleProducto()
        {
            return View();
        }

        public ActionResult MantenimientoUsuario()
        {
            return View();
        }
        public ActionResult Historial()
        {
            return View();
        }
        public ActionResult información()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> lista = new List<Producto>(); 
            try
            {
                
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

                var productosArray = cliente.ListarProductos();

            
                lista = productosArray.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar productos: " + ex.Message);
            }
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FiltrarProductos(string filtro)
        {
            List<Producto> productosFiltrados = new List<Producto>();
            try
            {
                
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

           
                var productosArray = cliente.FiltrarProductos(filtro);

              
                productosFiltrados = productosArray.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al filtrar productos: " + ex.Message);
                return Json(new { error = "Error al filtrar productos. Consulte los registros de errores para obtener más detalles." });
            }
            return Json(new { data = productosFiltrados });
        }

        [HttpGet]
        public JsonResult ProductosEnCarrito(int idUsuario)
        {
            try
            {
      
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

           
                var productosArray = cliente.ProductosEnCarrito(idUsuario);

               
                List<ProductoCarrito> productosEnCarrito = productosArray.ToList();

                return Json(new { productos = productosEnCarrito }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener productos en el carrito: " + ex.Message);
                return Json(new { error = "Error al obtener productos en el carrito: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]  suprimida porque no recuerdo si se usa :)
        //public JsonResult ListarCategorias()
        //{
        //    List<Categoria> listaCategorias = new List<Categoria>();
        //    try
        //    {
        //        string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = "SELECT idCategoria, Descripcion FROM Categoria";
        //            SqlCommand command = new SqlCommand(query, connection);
        //            connection.Open();
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    listaCategorias.Add(new Categoria()
        //                    {
        //                        idCategoria = Convert.ToInt32(reader["idCategoria"]),
        //                        Descripcion = reader["Descripcion"].ToString()
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error al listar categorías: " + ex.Message);
        //    }

        //    return Json(new { data = listaCategorias }, JsonRequestBehavior.AllowGet);
        //}  







        //Webservice llamada java
        [HttpPost]
        public ActionResult AgregarAlCarrito(int idProducto, int idUsuario)
        {
            try
            {
                WebServiceC.WebServicesSoapClient carritoService = new WebServiceC.WebServicesSoapClient();
               // WSJavaClient carritoService = new WSJavaClient();

                string resultado = carritoService.agregarAlCarrito(idProducto, idUsuario);

                if (resultado.Contains("exitosamente"))
                {
                    TempData["Mensaje"] = "Producto agregado al carrito exitosamente";
                    return Json(new { success = true, message = "Producto agregado al carrito exitosamente" });
                }
                else
                {
                    TempData["Error"] = resultado;
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error en el servidor: " + ex.Message;
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }


        [HttpGet]
        public JsonResult ObtenerDetallesProducto(int id)
        {
            Producto producto = null;
            try
            {
                
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

              
                producto = cliente.ObtenerDetallesProducto(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener detalles del producto: " + ex.Message);
                return Json(new { error = "Error al obtener detalles del producto" }, JsonRequestBehavior.AllowGet);
            }

            if (producto != null)
            {
                return Json(producto, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Producto no encontrado" }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpGet]
        public JsonResult ObtenerUsuario(int idUsuario)
        {
            Entidades.Usuario usuario = new Entidades.Usuario();
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT Nombres, Apellidos, Correo, Clave, Tipo FROM Usuario WHERE idUsuario = @idUsuario AND Tipo = 2";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Entidades.Usuario()
                            {
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener usuario: " + ex.Message);
            }
            return Json(new { data = usuario }, JsonRequestBehavior.AllowGet);
        }

        //Web service Java
        [HttpPost]
        public ActionResult ActualizarUsuario(int idUsuario, string nombres, string apellidos, string correo, string clave)
        {
            try
            {
                WebServiceC.WebServicesSoapClient usuarioService = new WebServiceC.WebServicesSoapClient();
                //WSJavaClient usuarioService = new WSJavaClient();

               
                string resultado = usuarioService.actualizarUsuario(idUsuario, nombres, apellidos, correo, clave);

                if (resultado.Contains("correctamente"))
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente";
                    return Json(new { success = true, message = "Usuario actualizado correctamente" });
                }
                else
                {
                    TempData["Error"] = resultado;
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = "Error en el servidor: " + ex.Message;
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }
        //web service java
        [HttpPost]
        public ActionResult GenerarVenta(int idUsuario)
        {
            try
            {
                WebServiceC.WebServicesSoapClient ventaService = new WebServiceC.WebServicesSoapClient();

               // WSJavaClient ventaService = new WSJavaClient();

                string resultado = ventaService.generarVenta(idUsuario);

                if (resultado.Contains("correctamente"))
                {
                    TempData["Mensaje"] = "Venta generada correctamente";
                    return Json(new { success = true, message = "Venta generada correctamente" });
                }
                else
                {
                    TempData["Error"] = resultado;
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = "Error en el servidor: " + ex.Message;
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }
    //web service java
        public JsonResult AgregarPregunta(int idUsuario, string pregunta)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

            
                string resultado = clienteWebService.AgregarPregunta(idUsuario, pregunta);

                if (resultado.Contains("exitosamente"))
                {
                    TempData["Mensaje"] = "Pregunta agregada exitosamente";
                    return Json(new { success = true, message = "Pregunta agregada exitosamente" });
                }
                else
                {
                    TempData["Error"] = resultado;
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
               
                TempData["Error"] = "Error en el servidor: " + ex.Message;
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }




        [HttpGet]
        public JsonResult ObtenerHistorialCompras(int idUsuario)
        {
            WebServiceC.Venta[] historial = null; 
            try
            {
                
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

               
                historial = cliente.ObtenerHistorialCompras(idUsuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener historial de compras: " + ex.Message);
                return Json(new { error = "Error al obtener historial de compras" }, JsonRequestBehavior.AllowGet);
            }

            if (historial != null)
            {
                return Json(historial, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "Historial de compras no encontrado" }, JsonRequestBehavior.AllowGet);
            }
        }





        [HttpGet]
        public JsonResult ObtenerProvincias()
        {
            try
            {
               
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

                TiendaTecno.WebServiceC.Provincia[] provinciasWebService = cliente.ObtenerProvincias();

              
                Entidades.Provincia[] provincias = provinciasWebService
                    .Select(p => new Entidades.Provincia { idProvincia = p.idProvincia, Nombre = p.Nombre })
                    .ToArray();

                return Json(provincias, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { ErrorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult ObtenerCantonesPorProvincia(int idProvincia)
        {
            try
            {
               
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();


                var cantones = cliente.ObtenerCantonesPorProvincia(idProvincia);

                return Json(cantones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener cantones por provincia: " + ex.Message);
                return Json(new { ErrorMessage = "Error al obtener cantones por provincia" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult ObtenerDistritos(int idProvincia)
        {
            try
            {
              
                WebServiceC.WebServicesSoapClient cliente = new WebServiceC.WebServicesSoapClient();

                var distritosArray = cliente.ObtenerDistritos(idProvincia);

     
                var distritos = distritosArray.ToList();

                return Json(distritos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener distritos: " + ex.Message);
                return Json(new { ErrorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //web service java
        [HttpPost]
        public string InsertarDireccion(int idUsuario, string nombreProvincia, string nombreCanton, string nombreDistrito)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

           
                string resultado = clienteWebService.InsertarDireccion(idUsuario, nombreProvincia, nombreCanton, nombreDistrito);

              
                if (resultado.Contains("correctamente"))
                {
                    return "Dirección insertada correctamente";
                }
                else
                {
                    return "Error al insertar la dirección: " + resultado;
                }
            }
            catch (Exception ex)
            {
          
                return "Error en el servidor: " + ex.Message;
            }
        }
        //ws java
        [HttpPost]
        public decimal CalcularMontoTotalCompra(int idUsuario)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

               
                decimal montoTotal = clienteWebService.CalcularMontoTotalCompra(idUsuario);

                
                return montoTotal;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error al calcular el monto total de la compra: " + ex.Message);
            }
        }




        //ws java

        [HttpPost]
        public string RealizarPago(string numeroTarjeta, string correoElectronico, string fechaExpiracion, int cvv, double cantidad)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

              
                string mensaje = clienteWebService.realizarPago(numeroTarjeta, correoElectronico, fechaExpiracion, cvv, cantidad);

                
                return mensaje;
            }
            catch (Exception ex)
            {
                
                return "Error al procesar el pago: " + ex.Message;
            }
        }

        //ws java


        [HttpPost]
        public string ProcesarTransferencia(string correoElectronico, string password, double cantidad)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
               // WSJavaClient clienteWebService = new WSJavaClient();

                string mensaje = clienteWebService.realizarTransferencia(correoElectronico, password, cantidad);

                
                return mensaje;
                
            }
            catch (Exception ex)
            {
             
                return "Error al procesar el pago: " + ex.Message;
            }
        }

        private string CompraBanco()
        {
            try
            {
                var Cambio = new BancoTipoCambio.wsindicadoreseconomicosSoapClient("wsindicadoreseconomicosSoap");
                string fechaInicio = DateTime.Now.ToString("dd/MM/yyyy");
                string fechaFinal = DateTime.Now.ToString("dd/MM/yyyy");

                DataSet dsResultado = Cambio.ObtenerIndicadoresEconomicos(
                    "317",
                    fechaInicio,
                    fechaFinal,
                    "Gerald Andry Aguilar Carvajal",
                    "N",
                    "aguilargerald272@gmail.com",
                    "2L8R2ELRAM");

                if (dsResultado != null && dsResultado.Tables.Count > 0 && dsResultado.Tables[0].Rows.Count > 0)
                {
                    decimal valorNumerico = Convert.ToDecimal(dsResultado.Tables[0].Rows[0]["NUM_VALOR"]);
                    string valor = valorNumerico.ToString("F2", CultureInfo.InvariantCulture);
                    return valor;
                }
                else
                {
                    return "El valor no fue encontrado.";
                }
            }
            catch (Exception ex)
            {
                return $"Hubo un problema al encontrar el valor: {ex.Message}";
            }
        }
        private string VentaBanco()
        {
            try
            {
                var Cambio = new BancoTipoCambio.wsindicadoreseconomicosSoapClient("wsindicadoreseconomicosSoap");
                string fechaInicio = DateTime.Now.ToString("dd/MM/yyyy");
                string fechaFinal = DateTime.Now.ToString("dd/MM/yyyy");

                DataSet dsResultado = Cambio.ObtenerIndicadoresEconomicos(
                    "318",
                    fechaInicio,
                    fechaFinal,
                    "Gerald Andry Aguilar Carvajal",
                    "N",
                    "aguilargerald272@gmail.com",
                    "2L8R2ELRAM");

                if (dsResultado != null && dsResultado.Tables.Count > 0 && dsResultado.Tables[0].Rows.Count > 0)
                {
                    decimal valorNumerico = Convert.ToDecimal(dsResultado.Tables[0].Rows[0]["NUM_VALOR"]);
                    string valor = valorNumerico.ToString("F2", CultureInfo.InvariantCulture);
                    return valor;
                }
                else
                {
                    return "El valor no fue encontrado.";
                }
            }
            catch (Exception ex)
            {
                return $"Hubo un problema al encontrar el valor: {ex.Message}";
            }
        }


        //nueva area
        [HttpGet]
        public JsonResult ObtenerEstadoUsuario(int idUsuario)
        {
            try
            {
 
                WebServicesSoapClient cliente = new WebServicesSoapClient();

           
                string estadoUsuario = cliente.ObtenerEstadoUsuario(idUsuario);

             
                return Json(estadoUsuario, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Error al obtener el estado del usuario: " + ex.Message);
                return Json(new { ErrorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult EliminarUsuario(int idUsuario)
        {
            try
            {
              
                WebServicesSoapClient cliente = new WebServicesSoapClient();

               
                string resultado = cliente.EliminarUsuarioPorIdSiExiste(idUsuario);

                return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine("Error al eliminar el usuario: " + ex.Message);
                return Json(new { ErrorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }


}
