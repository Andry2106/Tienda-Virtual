using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;


namespace AdminTecno.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }
        public ActionResult Productos()
        {
            return View();
        }
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult CerrarSesion()
        {

            Session.Clear();


            return RedirectToAction("Index","Login");
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT idUsuario, Nombres, Apellidos, Correo, Clave,Tipo FROM Usuario";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Usuario()
                            {
                                idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Tipo = Convert.ToInt32(dr["Tipo"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al listar usuarios: " + ex.Message);
            }

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistrarUsuario(Usuario obj)
        {
            int idautogenerado = 0;
            string Mensaje;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Tipo", obj.Tipo);
                   

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

        [HttpPost]
        public JsonResult EditarUsuario(Usuario obj)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idUsuario", obj.idUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Tipo", obj.Tipo);


                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

                return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj)
        {
            if (obj.idUsuario == 0)
            {
                return RegistrarUsuario(obj);
            }
            else if (obj.idUsuario > 0)
            {
                return EditarUsuario(obj);
            }
            else
            {
                return Json(new { resultado = false, mensaje = "El id de usuario no es válido." });
            }
        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM usuario WHERE idUsuario = @id", oconexion);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        //Zona de Categorias
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT idCategoria, Descripcion FROM Categoria";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaCategorias.Add(new Categoria()
                            {
                                idCategoria = Convert.ToInt32(reader["idCategoria"]),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar categorías: " + ex.Message);
            }

            return Json(new { data = listaCategorias }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarCategoria(Categoria objCategoria)
        {
            string mensaje;
            int idAutogenerado = 0;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", objCategoria.Descripcion);
   
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                mensaje = ex.Message;
            }

            return Json(new { idAutogenerado, mensaje });
        }

        [HttpPost]
        public JsonResult EditarCategoria(Categoria objCategoria)
        {
            string mensaje;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", oconexion);
                    cmd.Parameters.AddWithValue("idCategoria", objCategoria.idCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", objCategoria.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objCategoria)
        {
            string mensaje;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;

                    if (objCategoria.idCategoria == 0)
                    {

                        cmd = new SqlCommand("sp_RegistrarCategoria", oconexion);
                    }
                    else
                    {

                        cmd = new SqlCommand("sp_EditarCategoria", oconexion);
                        cmd.Parameters.AddWithValue("idCategoria", objCategoria.idCategoria);
                    }

                    cmd.Parameters.AddWithValue("Descripcion", objCategoria.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado, mensaje });
        }


        [HttpPost]
        public JsonResult EliminarCategoria(int idCategoria)
        {
            bool resultado = false;
            string mensaje = "";

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Categoria WHERE idCategoria = @idCategoria";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idCategoria", idCategoria);

                    connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        resultado = true;
                        mensaje = "Categoría eliminada correctamente";
                    }
                    else
                    {
                        mensaje = "No se encontró la categoría o no se realizó la eliminación";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        //Productos
        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> listaProductos = new List<Producto>();
            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT idProducto, Nombre, Descripcion, Precio, stock, idCategoria, RutaImagen FROM Producto";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaProductos.Add(new Producto()
                            {
                                idProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                stock = Convert.ToInt32(dr["stock"]),
                                idCategoria = Convert.ToInt32(dr["idCategoria"]),
                                RutaImagen = dr["RutaImagen"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar productos: " + ex.Message);
            }

            return Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarProducto(Producto obj)
        {
            int idAutogenerado = 0;
            string mensaje;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("stock", obj.stock);
                    cmd.Parameters.AddWithValue("idCategoria", obj.idCategoria);
                    cmd.Parameters.AddWithValue("RutaImagen", obj.RutaImagen);

                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                mensaje = ex.Message;
            }

            return Json(new { idAutogenerado = idAutogenerado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditarProducto(Producto obj)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("idProducto", obj.idProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("stock", obj.stock);
                    cmd.Parameters.AddWithValue("idCategoria", obj.idCategoria);
                    cmd.Parameters.AddWithValue("RutaImagen", obj.RutaImagen);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

                return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarProducto(Producto obj)
        {
            if (obj.idProducto == 0)
            {
                return RegistrarProducto(obj);
            }
            else if (obj.idProducto > 0)
            {
                return EditarProducto(obj);
            }
            else
            {
                return Json(new { resultado = false, mensaje = "El id del producto no es válido." });
            }
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            try
            {
                string connectionString = @"Data Source=(local);Initial Catalog=Programacion5TecnoCr;Integrated Security=True";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE idProducto = @id", conexion);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }


}
