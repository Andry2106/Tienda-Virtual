using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entidades;

namespace WS
{
    /// <summary>
    /// Descripción breve de WebServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServices : System.Web.Services.WebService
    {
        private const string BD = "Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8;";

        private const string BD2 = "Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\\MSSQLSERVER2019;Initial Catalog=BancoTecnoCr;User ID=BancoTecnoCr;Password=BancoTecnoCr;";


        [WebMethod]

        public List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT idProducto, Nombre, Descripcion, Precio, stock, RutaImagen FROM Producto";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                idProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                stock = Convert.ToInt32(dr["stock"]),
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
            return lista;
        }



        [WebMethod]
        public List<Producto> FiltrarProductos(string filtro)
        {
            List<Producto> productosFiltrados = new List<Producto>();
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT idProducto, Nombre, Descripcion, Precio, stock, RutaImagen FROM Producto WHERE Nombre LIKE @filtro";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            productosFiltrados.Add(new Producto()
                            {
                                idProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                stock = Convert.ToInt32(dr["stock"]),
                                RutaImagen = dr["RutaImagen"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al filtrar productos: " + ex.Message);
                // Puedes lanzar una excepción aquí si lo prefieres
            }
            return productosFiltrados;
        }





        [WebMethod]
        public Producto ObtenerDetallesProducto(int id)
        {
            Producto producto = null;
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT idProducto, Nombre, Descripcion, Precio, stock, RutaImagen FROM Producto WHERE idProducto = @idProducto";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idProducto", id);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto()
                            {
                                idProducto = Convert.ToInt32(reader["idProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                stock = Convert.ToInt32(reader["stock"]),
                                RutaImagen = reader["RutaImagen"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener detalles del producto: " + ex.Message);
          
            }

            return producto;
        }





        [WebMethod]
        public List<ProductoCarrito> ProductosEnCarrito(int idUsuario)
        {
            List<ProductoCarrito> productosEnCarrito = new List<ProductoCarrito>();

            string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT P.RutaImagen, P.Nombre, P.Precio, C.cantidad " +
                               "FROM Carrito AS C " +
                               "INNER JOIN Producto AS P ON C.idProducto = P.idProducto " +
                               "WHERE C.idUsuario = @idUsuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductoCarrito productoEnCarrito = new ProductoCarrito
                            {
                                RutaImagen = reader["RutaImagen"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                cantidad = Convert.ToInt32(reader["cantidad"])
                            };

                            productosEnCarrito.Add(productoEnCarrito);
                        }
                    }
                }
            }

            return productosEnCarrito;
        }







        [WebMethod]
        public object ObtenerUsuario(int idUsuario)
        {
            Usuario usuario = null;
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    string query = "SELECT Nombres, Apellidos, Correo, Clave FROM Usuario WHERE idUsuario = @idUsuario";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuario()
                            {
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener usuario: " + ex.Message);
            }

            return new
            {
                Nombres = usuario?.Nombres,
                Apellidos = usuario?.Apellidos,
                Correo = usuario?.Correo,
                Clave = usuario?.Clave
            };
        }








        [WebMethod]
        public List<Venta> ObtenerHistorialCompras(int idUsuario)
        {
            List<Venta> historial = new List<Venta>();

            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "CargarDetallesVentaPorUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idVenta = Convert.ToInt32(reader["idVenta"]);
                                Venta ventaActual = historial.FirstOrDefault(v => v.idVenta == idVenta);
                                if (ventaActual == null)
                                {
                                    ventaActual = new Venta
                                    {
                                        idVenta = idVenta,
                                        MontoTotal = Convert.ToDecimal(reader["MontoTotal"]),
                                        Detalles = new List<DetalleVenta>()
                                    };
                                    historial.Add(ventaActual);
                                }
                                DetalleVenta detalleVenta = new DetalleVenta
                                {
                                    idVenta = idVenta,
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    Nombre = reader["NombreProducto"].ToString(),
                                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                    Total = Convert.ToDecimal(reader["Total"])
                                };
                                ventaActual.Detalles.Add(detalleVenta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw; 
            }

            return historial;
        }



        [WebMethod]
        public Provincia[] ObtenerProvincias()
        {
            List<Provincia> provincias = new List<Provincia>();
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string queryProvincias = "SELECT idProvincia, Nombre FROM Provincia";
                    using (SqlCommand commandProvincias = new SqlCommand(queryProvincias, connection))
                    {
                        connection.Open();
                        using (SqlDataReader readerProvincias = commandProvincias.ExecuteReader())
                        {
                            while (readerProvincias.Read())
                            {
                                Provincia provincia = new Provincia
                                {
                                    idProvincia = Convert.ToInt32(readerProvincias["idProvincia"]),
                                    Nombre = readerProvincias["Nombre"].ToString()
                                };
                                provincias.Add(provincia);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
 
            }

            return provincias.ToArray();
        }



        [WebMethod]
        public List<Canton> ObtenerCantonesPorProvincia(int idProvincia)
        {
            List<Canton> cantones = new List<Canton>();

            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string queryCantones = "SELECT idCanton, Nombre FROM Canton WHERE idProvincia = @idProvincia";
                    using (SqlCommand commandCantones = new SqlCommand(queryCantones, connection))
                    {
                        commandCantones.Parameters.AddWithValue("@idProvincia", idProvincia);
                        connection.Open();
                        using (SqlDataReader readerCantones = commandCantones.ExecuteReader())
                        {
                            while (readerCantones.Read())
                            {
                                Canton canton = new Canton
                                {
                                    idCanton = Convert.ToInt32(readerCantones["idCanton"]),
                                    Nombre = readerCantones["Nombre"].ToString()
                                };
                                cantones.Add(canton);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return cantones;
        }
        [WebMethod]
        public List<Distrito> ObtenerDistritos(int idCanton)
        {
            List<Distrito> distritos = new List<Distrito>();

            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string queryDistritos = "SELECT idDistrito, Nombre FROM Distrito WHERE idCanton = @idCanton";
                    using (SqlCommand commandDistritos = new SqlCommand(queryDistritos, connection))
                    {
                        commandDistritos.Parameters.AddWithValue("@idCanton", idCanton);
                        connection.Open();
                        using (SqlDataReader readerDistritos = commandDistritos.ExecuteReader())
                        {
                            while (readerDistritos.Read())
                            {
                                Distrito distrito = new Distrito
                                {
                                    idDistrito = Convert.ToInt32(readerDistritos["idDistrito"]),
                                    Nombre = readerDistritos["Nombre"].ToString()
                                };
                                distritos.Add(distrito);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return distritos;
        }

        //nuevo web method
        [WebMethod]
        public string ObtenerEstadoUsuario(int idUsuario)
        {
            string estadoUsuario = string.Empty;
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_ComprobarNuevoUsuario", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    SqlParameter estadoParam = cmd.Parameters.Add("@EstadoUsuario", SqlDbType.VarChar, 100);
                    estadoParam.Direction = ParameterDirection.Output;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    estadoUsuario = estadoParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el estado del usuario: " + ex.Message);

            }

            return estadoUsuario;
        }
        [WebMethod]
        public string EliminarUsuarioPorIdSiExiste(int idUsuario)
        {
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarUsuarioPorIdSiExiste", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                return "Usuario eliminado correctamente.";
            }
            catch (Exception ex)
            {
                return "Error al eliminar el usuario: " + ex.Message;
            }
        }

        //cosas que estaban en java


        [WebMethod]
        public string ObtenerConexion()
        {
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8;";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    Console.WriteLine("Conectado");
                    return "Conectado exitosamente a prog5";
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(e.ToString());
                return "Error al conectar: " + e.Message;
            }
        }

        [WebMethod]
        public string ObtenerConexion2()
        {
            try
            {
                string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=BancoTecnoCr;User ID=BancoTecnoCr;Password=BancoTecnoCr;";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    Console.WriteLine("Conectado");
                    return "Conectado exitosamente a banco";
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(e.ToString());
                return "Error al conectar: " + e.Message;
            }
        }



        private SqlConnection obtenerConexion()
        {
            string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=TecnoCr;User ID=TecnoCr;Password=sr5W3@3c8;";
            return new SqlConnection(connectionString);
        }
        private SqlConnection obtenerConexion2()
        {
            string connectionString = @"Data Source=tiusr3pl.cuc-carrera-ti.ac.cr\MSSQLSERVER2019;Initial Catalog=BancoTecnoCr;User ID=BancoTecnoCr;Password=BancoTecnoCr;";
            return new SqlConnection(connectionString);
        }

        [WebMethod]
        public bool Login(string correo, string clave)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion())
                {
                    string query = "SELECT idUsuario FROM Usuario WHERE Correo = @correo AND Clave = @clave AND Tipo = 2";
                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        command.Parameters.AddWithValue("@correo", correo);
                        command.Parameters.AddWithValue("@clave", clave);
                        conexion.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            return reader.HasRows;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(ex.ToString());
             
                return false;
            }
        }







        [WebMethod]
        public string agregarAlCarrito(int idProducto, int idUsuario)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = obtenerConexion();
                if (conexion != null)
                {
                    string checkQuery = "SELECT COUNT(*) FROM Carrito WHERE idUsuario = @idUsuario AND idProducto = @idProducto";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, conexion))
                    {
                        checkCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                        checkCommand.Parameters.AddWithValue("@idProducto", idProducto);
                        conexion.Open();
                        int productCount = (int)checkCommand.ExecuteScalar();
                        if (productCount > 0)
                        {
                        
                            string updateQuery = "UPDATE Carrito SET cantidad = cantidad + 1 WHERE idUsuario = @idUsuario AND idProducto = @idProducto";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, conexion))
                            {
                                updateCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                                updateCommand.Parameters.AddWithValue("@idProducto", idProducto);
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    return "Cantidad del producto incrementada en el carrito";
                                }
                            }
                        }
                        else
                        {
                           
                            string insertQuery = "INSERT INTO Carrito (idUsuario, idProducto, cantidad) VALUES (@idUsuario, @idProducto, 1)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, conexion))
                            {
                                insertCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                                insertCommand.Parameters.AddWithValue("@idProducto", idProducto);
                                int rowsAffected = insertCommand.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    return "Producto agregado al carrito exitosamente";
                                }
                            }
                        }
                    }
                    return "No se pudo agregar el producto al carrito";
                }
                else
                {
                    return "No se pudo obtener la conexión";
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(e.ToString());
                return "Error al procesar la solicitud: " + e.Message;
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }







        [WebMethod]
        public string actualizarUsuario(int idUsuario, string nombres, string apellidos, string correo, string clave)
        {
            SqlConnection conexion = null;
            try
            {
                conexion = obtenerConexion();
                if (conexion != null)
                {
                    string sql = "UPDATE Usuario SET Nombres = @nombres, Apellidos = @apellidos, Correo = @correo, Clave = @clave WHERE idUsuario = @idUsuario";
                    using (SqlCommand statement = new SqlCommand(sql, conexion))
                    {
                        statement.Parameters.AddWithValue("@nombres", nombres);
                        statement.Parameters.AddWithValue("@apellidos", apellidos);
                        statement.Parameters.AddWithValue("@correo", correo);
                        statement.Parameters.AddWithValue("@clave", clave);
                        statement.Parameters.AddWithValue("@idUsuario", idUsuario);
                        conexion.Open();
                        int filasAfectadas = statement.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            return "Usuario actualizado correctamente";
                        }
                        else
                        {
                            return "No se pudo actualizar el usuario";
                        }
                    }
                }
                else
                {
                    return "No se pudo establecer la conexión";
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(e.ToString());
                return "Error al actualizar usuario: " + e.Message;
            }
            finally
            {
                if (conexion != null)
                {
                    conexion.Close();
                }
            }
        }




        [WebMethod]
        public string generarVenta(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion())
                {
                    string procedimientoAlmacenado = "GenerarVenta";
                    using (SqlCommand command = new SqlCommand(procedimientoAlmacenado, conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        conexion.Open();
                        command.ExecuteNonQuery();
                    }
                    return "Venta generada correctamente";
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(ex.ToString());
                return "Error al generar la venta: " + ex.Message;
            }
        }







        [WebMethod]
        public string AgregarPregunta(int idUsuario, string pregunta)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion())
                {
                   
                    string selectQuery = "SELECT COUNT(*) FROM Preguntas_Usuario WHERE idUsuario = @idUsuario";
                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, conexion))
                    {
                        selectCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                        conexion.Open();
                        int preguntaCount = (int)selectCommand.ExecuteScalar();
                        if (preguntaCount > 0)
                        {
                            
                            return "El usuario ya tiene una pregunta agregada.";
                        }
                    }

                    
                    string insertQuery = "INSERT INTO Preguntas_Usuario (idUsuario, Pregunta) VALUES (@idUsuario, @pregunta)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                        insertCommand.Parameters.AddWithValue("@pregunta", pregunta);
                        insertCommand.ExecuteNonQuery();
                    }

                    return "Pregunta agregada exitosamente.";
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(ex.ToString());
                return "Error al agregar la pregunta: " + ex.Message;
            }
        }





        [WebMethod]
        public string InsertarDireccion(int idUsuario, string nombreProvincia, string nombreCanton, string nombreDistrito)
        {
            try
            {
                using (SqlConnection connection = obtenerConexion())
                {
                    string call = "InsertarDireccion";
                    using (SqlCommand command = new SqlCommand(call, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        command.Parameters.AddWithValue("@nombreProvincia", nombreProvincia);
                        command.Parameters.AddWithValue("@nombreCanton", nombreCanton);
                        command.Parameters.AddWithValue("@nombreDistrito", nombreDistrito);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "Dirección insertada correctamente";
                        }
                        else
                        {
                            return "No se pudo insertar la dirección";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL");
                Console.WriteLine(ex.ToString());
                return "Error al insertar la dirección: " + ex.Message;
            }
        }

        [WebMethod]
        public string VerificarRespuesta(string correo, string respuesta)
        {
            try
            {
                int idUsuario = 0;
                string respuestaCorrecta = null;

                using (SqlConnection conexion = obtenerConexion())
                {
                    string queryUsuario = "SELECT idUsuario FROM Usuario WHERE Correo = @correo";
                    using (SqlCommand commandUsuario = new SqlCommand(queryUsuario, conexion))
                    {
                        commandUsuario.Parameters.AddWithValue("@correo", correo);
                        conexion.Open();
                        using (SqlDataReader readerUsuario = commandUsuario.ExecuteReader())
                        {
                            if (readerUsuario.Read())
                            {
                                idUsuario = readerUsuario.GetInt32(0);
                                readerUsuario.Close(); 

                                string queryRespuesta = "SELECT Pregunta FROM Preguntas_Usuario WHERE idUsuario = @idUsuario";
                                using (SqlCommand commandRespuesta = new SqlCommand(queryRespuesta, conexion))
                                {
                                    commandRespuesta.Parameters.AddWithValue("@idUsuario", idUsuario);
                                    using (SqlDataReader readerRespuesta = commandRespuesta.ExecuteReader())
                                    {
                                        if (readerRespuesta.Read())
                                        {
                                            respuestaCorrecta = readerRespuesta.GetString(0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (idUsuario == 0)
                {
                    return "El correo proporcionado no está registrado";
                }
                else if (respuesta == respuestaCorrecta)
                {
                    return "La respuesta proporcionada es correcta";
                }
                else
                {
                    return "La respuesta proporcionada es incorrecta";
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error: " + ex.Message;
            }
        }



        [WebMethod]
        public string loginEspecial(string correo, string nuevaClave)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion())
                {
                    string query = "SELECT idUsuario FROM Usuario WHERE Correo = @correo AND Tipo = 2";
                    using (SqlCommand statement = new SqlCommand(query, conexion))
                    {
                        statement.Parameters.AddWithValue("@correo", correo);
                        conexion.Open();
                        using (SqlDataReader resultSet = statement.ExecuteReader())
                        {
                            if (resultSet.Read())
                            {
                                int idUsuario = resultSet.GetInt32(0);
                                resultSet.Close(); 

                                query = "UPDATE Usuario SET Clave = @nuevaClave WHERE idUsuario = @idUsuario";
                                using (SqlCommand updateStatement = new SqlCommand(query, conexion))
                                {
                                    updateStatement.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                                    updateStatement.Parameters.AddWithValue("@idUsuario", idUsuario);
                                    updateStatement.ExecuteNonQuery();
                                }

                                return "success";
                            }
                            else
                            {
                                return "Correo no registrado como cliente.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString()); 
                return "Error en el servidor: " + ex.Message;
            }
        }



        [WebMethod]
        public string realizarPago(string numeroTarjeta, string correoElectronico, string fechaExpiracion, int cvv, double cantidad)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion2())
                {
                    string sql = "RealizarPago"; 
                    using (SqlCommand statement = new SqlCommand(sql, conexion))
                    {
                        statement.CommandType = CommandType.StoredProcedure; 

                      
                        statement.Parameters.AddWithValue("@numeroTarjeta", numeroTarjeta);
                        statement.Parameters.AddWithValue("@correoElectronico", correoElectronico);
                        statement.Parameters.AddWithValue("@fechaExpiracion", fechaExpiracion);
                        statement.Parameters.AddWithValue("@cvv", cvv);
                        statement.Parameters.AddWithValue("@cantidad", cantidad);

                        
                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
                        mensajeParam.Direction = ParameterDirection.Output;
                        statement.Parameters.Add(mensajeParam);

                        conexion.Open();
                        statement.ExecuteNonQuery();

                       
                        string mensaje = mensajeParam.Value.ToString();

                     
                        return mensaje;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error al procesar el pago: " + ex.Message;
            }
        }








        [WebMethod]
        public string realizarTransferencia(string correoElectronico, string password, double cantidad)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion2())
                {
                    string sql = "RealizarTransferencia"; 
                    using (SqlCommand statement = new SqlCommand(sql, conexion))
                    {
                        statement.CommandType = CommandType.StoredProcedure; 

                     
                        statement.Parameters.AddWithValue("@correoElectronico", correoElectronico);
                        statement.Parameters.AddWithValue("@password", password);
                        statement.Parameters.AddWithValue("@cantidad", cantidad);

                        
                        SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
                        mensajeParam.Direction = ParameterDirection.Output;
                        statement.Parameters.Add(mensajeParam);

                        conexion.Open();
                        statement.ExecuteNonQuery();

                        string mensaje = mensajeParam.Value.ToString();

                        return mensaje;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error al realizar la transferencia: " + ex.Message;
            }
        }





        [WebMethod]
        public decimal CalcularMontoTotalCompra(int idUsuario)
        {
            try
            {
                using (SqlConnection conexion = obtenerConexion())
                {
                    string procedimientoAlmacenado = "CalcularMontoTotalCompra";
                    using (SqlCommand command = new SqlCommand(procedimientoAlmacenado, conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);

                        conexion.Open();
                        command.ExecuteNonQuery();

                        return ObtenerMontoTotal(command);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception("Error al calcular el monto total de la compra: " + ex.Message);
            }
        }

        private decimal ObtenerMontoTotal(SqlCommand command)
        {

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows && reader.Read())
                {
                    return Convert.ToDecimal(reader[0]);
                }
                else
                {
                    throw new Exception("No se pudo obtener el resultado del procedimiento almacenado.");
                }
            }
        }

    }
}


