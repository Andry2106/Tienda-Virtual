using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminTecno.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CerrarSesion()
        {

            Session.Clear();


            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Login(string correo, string clave)
        {
            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    oconexion.Open();

                    string query = "SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo AND Clave = @Clave AND Tipo = 1";

                    using (SqlCommand cmd = new SqlCommand(query, oconexion))
                    {
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@Clave", clave);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                          
                            return Json(new { success = true, redirectTo = Url.Action("Index", "Home") });
                        }
                        else
                        {
                            
                            return Json(new { success = false, message = "Credenciales incorrectas." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }

    }
}