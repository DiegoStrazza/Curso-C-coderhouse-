using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimerosPasos
{
    public class Venta
    {
        //Declaración de atributos
        #region Atributos

        private int _Id, _IdUsuario; //bigint en sql
        private string _Comentarios; //varchar(max) en sql

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Comentarios { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public Venta()
        {

            _Id = 0;
            _IdUsuario = 0;
            _Comentarios = string.Empty;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public Venta(int Id, int IdUsuario, string Comentarios)
        {

            this._Id = Id;
            this._IdUsuario = IdUsuario;
            this._Comentarios = Comentarios;

        }
        #endregion constructor parametrizado

        //Declaración de métodos
        #region metodos sql
        private static string connectionStringMetodo()
        {
            string connectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
            return connectionString;
        }

        public static int EliminarVenta(Venta venta)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM Venta WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = venta.Id });
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

        public static Venta CrearVenta(Venta venta)
        {
            int IdVentaNuevo = 0;
            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "INSERT INTO Venta (Comentarios,IdUsuario) " +
                                "VALUES (@Comentarios,@IdUsuario );" +
                                " select @@IDENTITY";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Comentarios", SqlDbType.VarChar) { Value = venta.Comentarios });
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = venta.IdUsuario });
                    try
                    {
                        conectionSql.Open();
                        IdVentaNuevo = Convert.ToInt32(comandoSql.ExecuteScalar()); // importante incluir en la consulta SQL el ";select @@IDENTITY"
                        venta.Id = IdVentaNuevo;
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return venta;
        }

        public static int ModificarVenta(Venta venta)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "UPDATE Venta" +
                                " SET [Comentarios] = @Comentarios," +
                                    "[IdUsuario] = @IdUsuario" +
                                " WHERE Id = @Id ";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Comentarios", SqlDbType.VarChar) { Value = venta.Comentarios });
                    comandoSql.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.BigInt) { Value = venta.IdUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = venta.Id });

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

        public static List<Venta> DevolverVentas() //devuelve todos los ventas en la base de datos
        {
            var listaVenta = new List<Venta>();

            string consultaSQL = "SELECT [Id],[Comentarios],[IdUsuario] FROM Venta";

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


        public static List<Venta> TraerVentasXIdUsuario(int IdUsuario) //devuelve todos los ventas en la base de datos de un usuario
        {
            var listaVenta = new List<Venta>();

            string consultaSQL = "SELECT [Id],[Comentarios],[IdUsuario] FROM Venta WHERE IdUsuario = @IdUsuario";

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
            Console.WriteLine(errorMessages.ToString());
            Console.WriteLine("Se finaliza la consola de aplicación debido a los errores capturados");
            System.Environment.Exit(0);
        }

        #endregion metodos sql
    }
}
