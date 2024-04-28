using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaTecno.WebServicesJava;

namespace TiendaTecno.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrarse()
        {
            return View();
        }
        public ActionResult Autenticacion()
        {
            return View();
        }
        public ActionResult Recuperacion()
        {
            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        //web service java
        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            try
            {
                WebServiceC.WebServicesSoapClient login = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient login = new WebServicesJava.WSJavaClient();

                bool resultado = login.Login(correo, clave);

       
                if (resultado)
                {
                
                    JsonResult result = ObtenerCredenciales(correo);

                    dynamic jsonResult = result.Data;

         
                    if (jsonResult.mensaje != null && jsonResult.mensaje == "Credenciales obtenidas exitosamente")
                    {
                        TempData["Mensaje"] = "¡Bienvenido de nuevo!";
                        return Json(new { redirectTo = Url.Action("Autenticacion", "Login") });
                    }
                    else
                    {
                        TempData["Error"] = "Error al obtener las credenciales.";
                        return Json(new { error = "Error al obtener las credenciales." });
                    }
                }
                else
                {
             
                    TempData["Error"] = "Credenciales incorrectas.";
                    return Json(new { error = "Credenciales incorrectas." });
                }
            }
            catch (Exception ex)
            {
         
                TempData["Error"] = "Error en el servidor: " + ex.Message;
                return Json(new { error = "Error en el servidor: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ObtenerCredenciales(string correo)
        {
            try
            {
                int idUsuario = 0;
                string correoUsuario = null;
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string queryUsuario = "SELECT idUsuario, Correo FROM Usuario WHERE Correo = @Correo";
                    SqlCommand commandUsuario = new SqlCommand(queryUsuario, connection);
                    commandUsuario.Parameters.AddWithValue("@Correo", correo);
                    SqlDataReader reader = commandUsuario.ExecuteReader();
                    if (reader.Read())
                    {
                        idUsuario = reader.GetInt32(0);
                        correoUsuario = reader.GetString(1);

                  
                        Session["idUsuario"] = idUsuario;
                        Session["correo"] = correoUsuario;
                        System.Diagnostics.Debug.WriteLine("idUsuario: " + idUsuario);
                        System.Diagnostics.Debug.WriteLine("correoUsuario: " + correoUsuario);
                        
                    }
                    reader.Close();
                }

                if (idUsuario == 0)
                {
                    return Json(new { mensaje = "El correo proporcionado no está registrado" });
                }
                else
                {
                    return Json(new { mensaje = "Credenciales obtenidas exitosamente", idUsuario = idUsuario, correo = correoUsuario });
                }
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error: " + ex.Message });
            }
        }





        [HttpPost]
        public JsonResult RegistrarUsuario(Usuario obj)
        {
            int idautogenerado = 0;
            string Mensaje;
            int TipoUsuario=2;
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Tipo", TipoUsuario);


                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return Json(new { idAutogenerado = idautogenerado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        //web service java
        [HttpPost]
       
        public JsonResult VerificarRespuesta(string correo, string respuesta)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
                //WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

           
                string resultado = clienteWebService.VerificarRespuesta(correo, respuesta);

               
                return Json(new { mensaje = resultado });
            }
            catch (Exception ex)
            {
               
                return Json(new { mensaje = "Error: " + ex.Message });
            }
        }

        //web service java
        [HttpPost]
        public JsonResult LoginEspecial(string correo, string nuevaClave)
        {
            try
            {
                WebServiceC.WebServicesSoapClient clienteWebService = new WebServiceC.WebServicesSoapClient();
               // WebServicesJava.WSJavaClient clienteWebService = new WebServicesJava.WSJavaClient();

              
                string resultado = clienteWebService.loginEspecial(correo, nuevaClave);

              
                if (resultado.Contains("success"))
                {
                    JsonResult result = ObtenerCredenciales(correo);
                 
                    return Json(new { success = true, redirectTo = Url.Action("Autenticacion", "Login") });
                }
                else
                {
                   
                    return Json(new { success = false, message = "Correo no registrado como cliente." });
                }
            }
            catch (Exception ex)
            {
              
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }


    }

}
