using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class ProductoVendidoHandler
    {
        //public static string ConnectionString = "Data Source=(localdb)\\Servidor Ejercicio; Initial Catalog = SistemaGestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ConnectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";

        public static List<ProductoVendido> GetProductoVendidosByIdVenta(int IdVenta) //devuelve todos los productovendidos en la base de datos de un IdVenta
        {

            var listaProductoVendido = new List<ProductoVendido>();
            string consultaSQL = "SELECT [Id],[Stock],[IdProducto],[IdVenta] FROM [SistemaGestion].[dbo].ProductoVendido WHERE IdVenta = @IdVenta";

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@IdVenta", SqlDbType.BigInt) { Value = IdVenta });

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

        public static List<ProductoVendido> GetProductoVendidosByIdUsuario(int IdUsuario) //devuelve todos los productovendidos en la base de datos de un IdUsuario
        {
            List<Producto> listaProductosIdUsuario = new List<Producto>();
            listaProductosIdUsuario = ProductoHandler.GetProductoByIdUsuario(IdUsuario);

            var listaProductoVendido = new List<ProductoVendido>();
            string consultaSQL = "SELECT [Id],[Stock],[IdProducto],[IdVenta] FROM [SistemaGestion].[dbo].ProductoVendido WHERE IdProducto = @IdProducto";

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
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
                            conectionSql.Close();
                            TratamientoCatchExcepcionSQL(ex);
                        }
                    }
                }
            }
            return listaProductoVendido;
        }

        public static bool DeleteProductoVendidoByIdProductoVendido(int idProductoVendido)
        {
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
            {
                string querySql = "DELETE FROM [SistemaGestion].[dbo].ProductoVendido WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = idProductoVendido });

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
            if (filasAfectadasSql > 0)
            {
                return true;
            }
            else { return false; }
        }

        public static ProductoVendido CrearProductoVendido(ProductoVendido productovendido)
        {
            int IdProductoVendidoNuevo = 0;

            using (SqlConnection conectionSql = new SqlConnection(ConnectionString))
            {
                var querySql = "INSERT INTO [SistemaGestion].[dbo].ProductoVendido (Stock,IdProducto,IdVenta) " +
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
