using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json.Nodes;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class VentaHandler
    {
        //public static string ConnectionString = "Data Source=(localdb)\\Servidor Ejercicio; Initial Catalog = SistemaGestion; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ConnectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
        public static bool InsertVenta(List<Producto> productos, int IdUsuario)
        {
            bool modificado = false;
            Venta venta= new Venta();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString)) {
                using (SqlCommand sqlCommand = new SqlCommand()) { 
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = @"INSERT INTO [SistemaGestion].[dbo].Venta
                                        ([Comentarios]
                                        ,[IdUsuario])
                                        VALUES
                                        (@Comentarios,
                                            @IdUsuario)";

                    sqlCommand.Parameters.AddWithValue("@Comentarios", "");
                    sqlCommand.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO Venta
                    venta.Id = GetId.Get(sqlCommand);
                    venta.IdUsuario = IdUsuario;

                    foreach (Producto producto in productos)
                    {
                        sqlCommand.CommandText = @"INSERT INTO [SistemaGestion].[dbo].ProductoVendido
                                        ([Stock]
                                        ,[IdProducto]
                                        ,[IdVenta])
                                        VALUES
                                        (@Stock,
                                        @IdProducto,
                                        @IdVenta)";

                        sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                        sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);
                        sqlCommand.Parameters.AddWithValue("@IdVenta", venta.Id);

                        sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO ProductoVendido
                        sqlCommand.Parameters.Clear();

                        sqlCommand.CommandText = @" UPDATE [SistemaGestion].[dbo].Producto
                                                        SET 
                                                        Stock = Stock - @Stock
                                                        WHERE id = @IdProducto";

                        sqlCommand.Parameters.AddWithValue("@Stock", producto.Stock);
                        sqlCommand.Parameters.AddWithValue("@IdProducto", producto.Id);

                        sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el UPDATE Producto actualizando el stock
                        sqlCommand.Parameters.Clear();
                    }
                    sqlCommand.Connection.Close();

                }
            }
            modificado = true;
            return modificado;
        }

        public static bool DeleteVentaByIdVenta(int IdVenta)
        {
            bool modificado = false;

            List<ProductoVendido> listaProductosVendidosByIdVenta = ProductoVendidoHandler.GetProductoVendidosByIdVenta(IdVenta);

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString)) {

                using (SqlCommand sqlCommand = new SqlCommand()) { 
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Connection.Open();
                    foreach (ProductoVendido productosVendidosByIdVenta in listaProductosVendidosByIdVenta)
                    {
                        //Sumo el stock a los productos asociados a los productos vendidos de la venta
                        sqlCommand.CommandText = @" UPDATE [SistemaGestion].[dbo].Producto
                                                        SET 
                                                        Stock = Stock + @Stock
                                                        WHERE id = @IdProducto";

                        sqlCommand.Parameters.AddWithValue("@Stock", productosVendidosByIdVenta.Stock);
                        sqlCommand.Parameters.AddWithValue("@IdProducto", productosVendidosByIdVenta.IdProducto);
                        sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el UPDATE Producto actualizando el stock
                        sqlCommand.Parameters.Clear();

                        //Elimino el producto vendido asociado a la venta
                        ProductoVendidoHandler.DeleteProductoVendidoByIdProductoVendido(productosVendidosByIdVenta.Id);

                    }
                    //Elimino la venta
                    sqlCommand.CommandText = @"DELETE FROM [SistemaGestion].[dbo].Venta WHERE Id=@IdVenta";
                    sqlCommand.Parameters.AddWithValue("@IdVenta", IdVenta);

                    if (sqlCommand.ExecuteNonQuery() > 0)//Se ejecuta realmente el DELETE Venta
                    {
                        modificado= true;
                    }
                    else { modificado= false; }
                    sqlCommand.Connection.Close();
                }
            }
            return modificado;
        }

        public static List<Venta> GetVentas() //devuelve todos los ventas en la base de datos
        {
            var listaVenta = new List<Venta>();

            string consultaSQL = "SELECT [Id],[Comentarios],[IdUsuario] FROM [SistemaGestion].[dbo].Venta";

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
                                    var ventaAuxiliar = new Venta();
                                    ventaAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    ventaAuxiliar.Comentarios = Convert.ToString(dataReader["Comentarios"]);
                                    ventaAuxiliar.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);

                                    listaVenta.Add(ventaAuxiliar);
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
            return listaVenta;
        }

        public static List<string> GetVentasYProductos() //Debe traer todas las ventas de la base, incluyendo sus Productos, cuya información está en ProductosVendidos
        {
            var listaVenta = new List<Venta>();

            string resultadoIndividualConsultaSql = "";
            List<string> resultadoConsultaSql = new List<string>();

            string consultaSQL = "SELECT [Venta].[Id] AS IdVenta,[Venta].[Comentarios] AS ComentariosVenta,[Venta].[IdUsuario] AS IdUsuarioEjecutoVenta, " +
                "[ProductoVendido].Stock AS StockProductoVendido,[ProductoVendido].Id AS IdProductoVendido, " +
                "[Producto].Id AS IdProducto,[Producto].Descripciones AS DescripcionesProducto, [Producto].Stock AS StockProducto " +
                "FROM [SistemaGestion].[dbo].[ProductoVendido] INNER JOIN [SistemaGestion].[dbo].[Venta] ON [Venta].id = [ProductoVendido].IdVenta " +
                "INNER JOIN [SistemaGestion].[dbo].[Producto] ON [Producto].Id = [ProductoVendido].IdProducto";

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
                                    resultadoIndividualConsultaSql = "";
                                    resultadoIndividualConsultaSql += "IdVenta:" + Convert.ToString(dataReader["IdVenta"]) +" - ";
                                    resultadoIndividualConsultaSql += "ComentariosVenta:" + Convert.ToString(dataReader["ComentariosVenta"]) + " - ";
                                    resultadoIndividualConsultaSql += "IdUsuarioEjecutoVenta:" + Convert.ToString(dataReader["IdUsuarioEjecutoVenta"]) + " - ";
                                    resultadoIndividualConsultaSql += "StockProductoVendido:" + Convert.ToString(dataReader["StockProductoVendido"]) + " - ";
                                    resultadoIndividualConsultaSql += "IdProductoVendido:" + Convert.ToString(dataReader["IdProductoVendido"]) + " - ";
                                    resultadoIndividualConsultaSql += "IdProducto:" + Convert.ToString(dataReader["IdProducto"]) + " - ";
                                    resultadoIndividualConsultaSql += "DescripcionesProducto:" + Convert.ToString(dataReader["DescripcionesProducto"]) + " - ";
                                    resultadoIndividualConsultaSql += "StockProducto:" + Convert.ToString(dataReader["StockProducto"]) + " - ";
                                    resultadoConsultaSql.Add(resultadoIndividualConsultaSql);

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
            return resultadoConsultaSql;
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
