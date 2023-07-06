using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Repository
{
    public class ADO_ProductoVendido
    {
        #region metodos sql

        private static string connectionStringMetodo()
        {
            string connectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
            return connectionString;
        }

        public static int EliminarProductoVendido(int productoVendidoId)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM ProductoVendido WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = productoVendidoId });

                    try
                    {
                        conectionSql.Open();
                        filasAfectadasSql = comandoSql.ExecuteNonQuery(); //devuelve el número de filas sql afectadas
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return filasAfectadasSql;
        }


        public static int EliminarProductoVendidoXIdProducto(int idProducto)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM ProductoVendido WHERE IdProducto=@IdProducto";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.BigInt) { Value = idProducto });

                    try
                    {
                        conectionSql.Open();
                        filasAfectadasSql = comandoSql.ExecuteNonQuery(); //devuelve el número de filas sql afectadas
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return filasAfectadasSql;
        }

        public static ProductoVendido CrearProductoVendido(ProductoVendido productovendido)
        {
            int IdProductoVendidoNuevo = 0;
            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "INSERT INTO ProductoVendido (Stock,IdProducto,IdVenta) " +
                                "VALUES (@Stock,@IdProducto,@IdVenta );" +
                                " select @@IDENTITY";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = productovendido.Stock });
                    comandoSql.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.BigInt) { Value = productovendido.IdProducto });
                    comandoSql.Parameters.Add(new SqlParameter("@IdVenta", SqlDbType.BigInt) { Value = productovendido.IdVenta });

                    try
                    {
                        conectionSql.Open();
                        IdProductoVendidoNuevo = Convert.ToInt32(comandoSql.ExecuteScalar()); // importante incluir en la consulta SQL el ";select @@IDENTITY"
                        productovendido.Id = IdProductoVendidoNuevo;
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return productovendido;
        }

        public static int ModificarProductoVendido(ProductoVendido productovendido)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "UPDATE ProductoVendido" +
                                " SET [Stock] = @Stock," +
                                    "[IdProducto] = @IdProducto," +
                                    "[IdVenta] = @IdVenta" +
                                " WHERE Id = @Id ";


                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = productovendido.Stock });
                    comandoSql.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.BigInt) { Value = productovendido.IdProducto });
                    comandoSql.Parameters.Add(new SqlParameter("@IdVenta", SqlDbType.BigInt) { Value = productovendido.IdVenta });
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = productovendido.Id });

                    try
                    {
                        conectionSql.Open();
                        filasAfectadasSql = comandoSql.ExecuteNonQuery(); //devuelve el número de filas sql afectadas
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return filasAfectadasSql;
        }

        public static List<ProductoVendido> TraerProductoVendidos() //devuelve todos los productovendidos en la base de datos
        {
            var listaProductoVendido = new List<ProductoVendido>();

            string consultaSQL = "SELECT [Id],[Stock],[IdProducto],[IdVenta] FROM ProductoVendido";

            string connectionString = connectionStringMetodo();
            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    try
                    {
                        conectionSql.Open();

                        using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    var productoVendidoAuxiliar = new ProductoVendido();
                                    productoVendidoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    productoVendidoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                    productoVendidoAuxiliar.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                    productoVendidoAuxiliar.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                    listaProductoVendido.Add(productoVendidoAuxiliar);
                                }
                            }
                        }
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return listaProductoVendido;
        }

        public static List<ProductoVendido> TraerProductoVendidosXIdUsuario(int IdUsuario) //devuelve todos los productovendidos en la base de datos de un IdUsuario
        {
            List<Producto> listaProductosIdUsuario = new List<Producto>();
            listaProductosIdUsuario = ADO_Producto.TraerProductoXIdUsuario(IdUsuario);

            var listaProductoVendido = new List<ProductoVendido>();
            string consultaSQL = "SELECT [Id],[Stock],[IdProducto],[IdVenta] FROM ProductoVendido WHERE IdProducto = @IdProducto";

            string connectionString = connectionStringMetodo();
            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    SqlParameter parametroIdUsuario = new SqlParameter("@IdProducto", SqlDbType.BigInt);
                    comandoSql.Parameters.Add(parametroIdUsuario);

                    foreach (Producto producto in listaProductosIdUsuario)
                    { //recorro todos los productos del usuario
                        parametroIdUsuario.Value = producto.Id;

                        try
                        {
                            conectionSql.Open();

                            using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                            {
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        var productoVendidoAuxiliar = new ProductoVendido();
                                        productoVendidoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                        productoVendidoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                        productoVendidoAuxiliar.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                        productoVendidoAuxiliar.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                        listaProductoVendido.Add(productoVendidoAuxiliar);
                                    }
                                }
                            }
                            conectionSql.Close();
                        }
                        catch (SqlException ex)
                        {
                            TratamientoCatchExcepcionSQL(ex);
                        }
                    }
                }
            }
            return listaProductoVendido;
        }


        public static ProductoVendido TraerProductoVendidoXIdProductoVendido(int idProductoVendido) //devuelve el productovendidos en la base de datos de un ID
        {
            var productoVendidoAuxiliar = new ProductoVendido();

            string consultaSQL = "SELECT [Id],[Stock],[IdProducto],[IdVenta] FROM ProductoVendido WHERE Id = @Id";

            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                { 
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = idProductoVendido });

                        try
                        {
                            conectionSql.Open();

                            using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                            {
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        
                                        productoVendidoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                        productoVendidoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                        productoVendidoAuxiliar.IdProducto = Convert.ToInt32(dataReader["IdProducto"]);
                                        productoVendidoAuxiliar.IdVenta = Convert.ToInt32(dataReader["IdVenta"]);

                                    }
                                }
                            }
                            conectionSql.Close();
                        }
                        catch (SqlException ex)
                        {
                            TratamientoCatchExcepcionSQL(ex);
                        }
                }
            }
            return productoVendidoAuxiliar;
        }

        private static void TratamientoCatchExcepcionSQL(SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();

            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + " of sql errors\n" +
                    "Error Message: " + ex.Errors[i].Message + "\n" +
                    "Error LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                    "Error Number: " + ex.Errors[i].Number + "\n\n");
            }
            //Console.WriteLine(errorMessages.ToString());
            //Console.WriteLine("Se finaliza la consola de aplicación debido a los errores capturados");
            //System.Environment.Exit(0);
        }

        #endregion metodos sql
    }
}