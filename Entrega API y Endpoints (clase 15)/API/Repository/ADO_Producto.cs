using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Repository
{
    public class ADO_Producto
    {
        #region metodos Sql
        private static string connectionStringMetodo()
        {
            string connectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
            return connectionString;
        }

        public static int EliminarProducto(int productoId)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM Producto WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = productoId });
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

        public static Producto CrearProducto(Producto producto)
        {
            int IdProductoNuevo = 0;
            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "INSERT INTO Producto (Descripciones,Costo,PrecioVenta,Stock,IdUsuario) " +
                                "VALUES (@Descripciones,@Costo,@PrecioVenta,@Stock,@IdUsuario );" +
                                " select @@IDENTITY";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Descripciones", SqlDbType.VarChar) { Value = producto.Descripciones });
                    comandoSql.Parameters.Add(new SqlParameter("@Costo", SqlDbType.Money) { Value = producto.Costo });
                    comandoSql.Parameters.Add(new SqlParameter("@PrecioVenta", SqlDbType.Money) { Value = producto.PrecioVenta });
                    comandoSql.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario });
                    try
                    {
                        conectionSql.Open();
                        IdProductoNuevo = Convert.ToInt32(comandoSql.ExecuteScalar()); // importante incluir en la consulta SQL el ";select @@IDENTITY"
                        producto.Id = IdProductoNuevo;
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return producto;
        }

        public static int ModificarProducto(Producto producto)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "UPDATE Producto" +
                                " SET [Descripciones] = @Descripciones," +
                                    "[Costo] = @Costo," +
                                    "[PrecioVenta] = @PrecioVenta," +
                                    "[Stock] = @Stock," +
                                    "[IdUsuario] = @IdUsuario" +
                                " WHERE Id = @Id ";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Descripciones", SqlDbType.VarChar) { Value = producto.Descripciones });
                    comandoSql.Parameters.Add(new SqlParameter("@Costo", SqlDbType.Money) { Value = producto.Costo });
                    comandoSql.Parameters.Add(new SqlParameter("@PrecioVenta", SqlDbType.Money) { Value = producto.PrecioVenta });
                    comandoSql.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = producto.Id });

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


        public static int ModificarStockProducto(Producto producto)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "UPDATE Producto" +
                                " SET [Stock] = @Stock," +
                                     "[IdUsuario] = @IdUsuario" +
                                " WHERE Id = @Id ";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Stock", SqlDbType.Int) { Value = producto.Stock });
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = producto.Id });

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

        public static List<Producto> TraerProductos() //devuelve todos los productos en la base de datos
        {
            var listaProducto = new List<Producto>();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM Producto";

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
                                    var productoAuxiliar = new Producto();
                                    productoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    productoAuxiliar.Descripciones = Convert.ToString(dataReader["Descripciones"]);
                                    productoAuxiliar.Costo = Convert.ToDouble(dataReader["Costo"]);
                                    productoAuxiliar.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                    productoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                    productoAuxiliar.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                    listaProducto.Add(productoAuxiliar);
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
            return listaProducto;
        }


        public static List<Producto> TraerProductoXIdUsuario(int IdUsuario) //devuelve todos los productos en la base de datos asociado a un IdUsuario
        {
            var listaProducto = new List<Producto>();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM Producto WHERE IdUsuario = @IdUsuario";

            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = IdUsuario });
                    try
                    {
                        conectionSql.Open();

                        using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    var productoAuxiliar = new Producto();
                                    productoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    productoAuxiliar.Descripciones = Convert.ToString(dataReader["Descripciones"]);
                                    productoAuxiliar.Costo = Convert.ToDouble(dataReader["Costo"]);
                                    productoAuxiliar.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                    productoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                    productoAuxiliar.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                    listaProducto.Add(productoAuxiliar);
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
            return listaProducto;
        }


        public static Producto TraerProductoXIdProducto(int IdProducto) //devuelve los datos del producto
        {
            Producto productoAuxiliar = new Producto();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @Id";

            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = IdProducto });
                    try
                    {
                        conectionSql.Open();

                        using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    productoAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    productoAuxiliar.Descripciones = Convert.ToString(dataReader["Descripciones"]);
                                    productoAuxiliar.Costo = Convert.ToDouble(dataReader["Costo"]);
                                    productoAuxiliar.PrecioVenta = Convert.ToDouble(dataReader["PrecioVenta"]);
                                    productoAuxiliar.Stock = Convert.ToInt32(dataReader["Stock"]);
                                    productoAuxiliar.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

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
            return productoAuxiliar;
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
        #endregion metodos SQL
    }
}