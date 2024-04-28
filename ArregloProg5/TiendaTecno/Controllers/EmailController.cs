using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Text;

namespace TiendaTecno.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            try
            {
                
                var fromAddress = new MailAddress("tecnocr4@gmail.com", "TecnoCr");
                var toAddress = new MailAddress("AguilarGerald272@gmail.com");
                const string subject = "Bienvenido a TecnoCr";
                const string body = "Agradecido con el de arriba";

                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, ConfigurationManager.AppSettings["SMTPPassword"])
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                ViewBag.Message = "Correo enviado correctamente.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                
                ViewBag.Message = "Error al enviar el correo: " + ex.Message;
            }

            return View();
        }
        [HttpPost]
        public ActionResult EnviarCorreoConToken(int idUsuario)
        {
            try
            {
              
                string token = GenerarToken();

               
                Session["Token"] = token;
                Session["idUsuario"] = idUsuario;
                    
                
                string correoUsuario;
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Correo FROM Usuario WHERE idUsuario = @idUsuario";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    correoUsuario = (string)command.ExecuteScalar();
                }

                
                var fromAddress = new MailAddress("tecnocr4@gmail.com", "TecnoCr");
                var toAddress = new MailAddress(correoUsuario);
                const string subject = "Bienvenido a TecnoCr";
                string body = $"Su token de validacion en TecnoCr es: {token}";

                
                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, ConfigurationManager.AppSettings["SMTPPassword"])
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Usuario_Token (idUsuario, Token) VALUES (@idUsuario, @Token)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                    insertCommand.Parameters.AddWithValue("@Token", token);
                    insertCommand.ExecuteNonQuery();
                }

                
                ViewBag.Token = token;
                return View("Autenticacion");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al enviar el correo o insertar el token: " + ex.Message });
            }
        }


        private string GenerarToken()
        {
            const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder tokenBuilder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 8; i++) 
            {
                tokenBuilder.Append(caracteresPermitidos[random.Next(caracteresPermitidos.Length)]);
            }
            return tokenBuilder.ToString();
        }
        public string ObtenerTokenPorIdUsuario(int idUsuario)
        {
            string token = null;
            string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Token FROM Usuario_Token WHERE idUsuario = @idUsuario";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    token = (string)result;
                }
            }
            return token;
        }
        public void EliminarTokensGenerales()
        {
            string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Usuario_Token";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        [HttpPost]
        public ActionResult EnviarCorreo(string correoUsuario)
        {
            try
            {
  
                string token = GenerarToken();

                var fromAddress = new MailAddress("tecnocr4@gmail.com", "TecnoCr");
                var toAddress = new MailAddress(correoUsuario);

                const string subject = "Bienvenido a TecnoCr";
                string body = $"Su token de registro en TecnoCr es: {token}";

                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, ConfigurationManager.AppSettings["SMTPPassword"])
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }


                return Json(new { success = true, token = token, message = "Correo enviado exitosamente" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Error al enviar el correo: " + ex.Message });
            }
        }




    }
}