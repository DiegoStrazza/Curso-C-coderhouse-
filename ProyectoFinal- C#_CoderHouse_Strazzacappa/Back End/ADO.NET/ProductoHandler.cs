using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class ProductoHandler
    {
        //public static string ConnectionString = "Data Source=(localdb)\\Servidor Ejercicio; Initial Catalog = SistemaGestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ConnectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";

        public static List<Producto> GetProductos() //devuelve todos los productos en la base de datos
        {
            var listaProducto = new List<Producto>();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM [SistemaGestion].[dbo].Producto";

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
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


        public static Producto GetProductoByIdProducto(int IdProducto) //devuelve los datos del producto
        {
            Producto productoAuxiliar = new Producto();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM [SistemaGestion].[dbo].[Producto] WHERE Id = @Id";

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
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


        public static List<Producto> GetProductoByIdUsuario(int IdUsuario) //devuelve todos los productos en la base de datos asociado a un IdUsuario
        {
            var listaProducto = new List<Producto>();

            string consultaSQL = "SELECT [Id],[Descripciones],[Costo],[PrecioVenta],[Stock],[IdUsuario] FROM [SistemaGestion].[dbo].Producto WHERE IdUsuario = @IdUsuario";

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
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

        public static bool UpdateProductos(Producto producto)
        {
            bool modificado = false;

            if (producto.Descripciones == null ||
                producto.Descripciones == "" ||
                producto.IdUsuario == 0)
            {
                return modificado;
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @" UPDATE [SistemaGestion].[dbo].Producto
                                                SET 
                                                   Descripciones = @Descripciones,
                                                   Costo = @Costo,
                                                   PrecioVenta = @PrecioVenta,
										           Stock = @Stock
                                                WHERE id = @ID";

                        sqlCommand.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
                        sqlCommand.Parameters.AddWithValue("@Costo", producto.Costo);
                        sqlCommand.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                        sqlCommand.Parameters.AddWithValue("@ID", producto.Id);


                        int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente UPDATE
                        sqlCommand.Connection.Close();

                        if (recordsAffected == 0)
                        {
                            return modificado;
                            throw new Exception("El registro a modificar no existe.");
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        public static int UpdateStockProducto(Producto producto)
        {
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
            {
                var querySql = "UPDATE [SistemaGestion].[dbo].Producto" +
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

        public static bool InsertProducto(Producto producto)
        {
            bool alta = false;
            
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.CommandText = @"INSERT INTO [SistemaGestion].[dbo].Producto
                                ([Descripciones]
                                ,[Costo]
                                ,[PrecioVenta]
								,[Stock]
                                ,[IdUsuario])
                                VALUES
                                (@Descripciones,
                                    @Costo,
                                    @PrecioVenta,
									@Stock,
                                    @IdUsuario)";

            sqlCommand.Parameters.AddWithValue("@Descripciones", producto.Descripciones);
            sqlCommand.Parameters.AddWithValue("@Costo", producto.Costo);
            sqlCommand.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
            sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
            sqlCommand.Parameters.AddWithValue("@IdUsuario", producto.IdUsuario);

            sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
            producto.Id = GetId.Get(sqlCommand);

            alta = producto.Id != 0 ? true : false;
            sqlCommand.Connection.Close();
            return alta;

            
        }

        public static bool DeleteProducto(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    //Antes de eliminar el producto, se eliminan los productos vendidos asociados al producto
                    sqlCommand.CommandText = @" DELETE 
                                                    [SistemaGestion].[dbo].ProductoVendido
                                                WHERE 
                                                    IdProducto = @ID
                                            ";

                    sqlCommand.Parameters.AddWithValue("@ID", id);
                    

                    int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE

                    sqlCommand.CommandText = @" DELETE 
                                                    [SistemaGestion].[dbo].Producto
                                                WHERE 
                                                    Id = @ID
                                            ";

                    recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el DELETE
                    sqlCommand.Connection.Close();

                    if (recordsAffected != 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
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
    }

}

