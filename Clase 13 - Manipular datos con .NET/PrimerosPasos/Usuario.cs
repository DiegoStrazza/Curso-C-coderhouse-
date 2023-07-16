using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimerosPasos
{
    public class Usuario
    {
        //Declaración de atributos
        #region Atributos

        private int _Id; //bigint en sql
        private string _Nombre, _Apellido, _NombreUsuario, _Contraseña, _Mail; //varchar(max) en sql
        

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Mail { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public Usuario()
        {

            _Id = 0;
            _Nombre = string.Empty;
            _Apellido = string.Empty;
            _NombreUsuario = string.Empty;
            _Contraseña = string.Empty;
            _Mail = string.Empty;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public Usuario(int Id, string Nombre, string Apellido, string NombreUsuario,string Contraseña, string Mail)
        {

            this._Id = Id;
            this._Nombre = Nombre;
            this._Apellido = Apellido;
            this._NombreUsuario = NombreUsuario;
            this._Contraseña = Contraseña;
            this._Mail = Mail;

        }
        #endregion constructor parametrizado

        //Declaración de métodos
        #region metodos sql

        private static string connectionStringMetodo()
        {
            string connectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
            return connectionString;
        }

        public static int EliminarUsuario(Usuario usuario)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM Usuario WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = usuario.Id });
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

        public static Usuario CrearUsuario(Usuario usuario)
        {
            int IdUsuarioNuevo = 0;
            string connectionString = connectionStringMetodo();

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "INSERT INTO Usuario (Nombre,Apellido,NombreUsuario,Contraseña,Mail) " +
                                "VALUES (@Nombre,@Apellido,@NombreUsuario,@Contraseña,@Mail );" +
                                " select @@IDENTITY";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar) { Value = usuario.Nombre });
                    comandoSql.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar) { Value = usuario.Apellido });
                    comandoSql.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña });
                    comandoSql.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = usuario.Mail });
                    try
                    {
                        conectionSql.Open();
                        IdUsuarioNuevo = Convert.ToInt32(comandoSql.ExecuteScalar()); // importante incluir en la consulta SQL el ";select @@IDENTITY"
                        usuario.Id = IdUsuarioNuevo;
                        conectionSql.Close();
                    }
                    catch (SqlException ex)
                    {
                        TratamientoCatchExcepcionSQL(ex);
                    }
                }
            }
            return usuario;
        }

        public static int ModificarUsuario(Usuario usuario)
        {
            string connectionString = connectionStringMetodo();
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                var querySql = "UPDATE Usuario" +
                                " SET [Nombre] = @Nombre," +
                                    "[Apellido] = @Apellido," +
                                    "[NombreUsuario] = @NombreUsuario," +
                                    "[Contraseña] = @Contraseña," +
                                    "[Mail] = @Mail" +
                                " WHERE Id = @Id ";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar) { Value = usuario.Nombre });
                    comandoSql.Parameters.Add(new SqlParameter("@Apellido", SqlDbType.VarChar) { Value = usuario.Apellido });
                    comandoSql.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña });
                    comandoSql.Parameters.Add(new SqlParameter("@Mail", SqlDbType.VarChar) { Value = usuario.Mail });
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = usuario.Id });

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

        public static List<Usuario> DevolverUsuarios() //devuelve todos los usuarios en la base de datos
        {
            var listaUsuario = new List<Usuario>();

            string consultaSQL = "SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] FROM Usuario";

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
                                    var usuarioAuxiliar = new Usuario();
                                    usuarioAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    usuarioAuxiliar.Nombre = Convert.ToString(dataReader["Nombre"]);
                                    usuarioAuxiliar.Apellido = Convert.ToString(dataReader["Apellido"]);
                                    usuarioAuxiliar.NombreUsuario = Convert.ToString(dataReader["NombreUsuario"]);
                                    usuarioAuxiliar.Contraseña = Convert.ToString(dataReader["Contraseña"]);
                                    usuarioAuxiliar.Mail = Convert.ToString(dataReader["Mail"]);

                                    listaUsuario.Add(usuarioAuxiliar);
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
            return listaUsuario;
        }


        public static Usuario DevolverUsuarioXNombreUsuarioYContraseña(string nombreUsuario, string contraseña) //devuelve todos los usuarios en la base de datos
        {
            var usuarioAuxiliar = new Usuario();
            string consultaSQL = "SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] FROM Usuario WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña";

            string connectionString = connectionStringMetodo();
            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = nombreUsuario });
                    comandoSql.Parameters.Add(new SqlParameter("@Contraseña", SqlDbType.VarChar) { Value = contraseña });

                    try
                    {
                        conectionSql.Open();

                        using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    
                                    usuarioAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    usuarioAuxiliar.Nombre = Convert.ToString(dataReader["Nombre"]);
                                    usuarioAuxiliar.Apellido = Convert.ToString(dataReader["Apellido"]);
                                    usuarioAuxiliar.NombreUsuario = Convert.ToString(dataReader["NombreUsuario"]);
                                    usuarioAuxiliar.Contraseña = Convert.ToString(dataReader["Contraseña"]);
                                    usuarioAuxiliar.Mail = Convert.ToString(dataReader["Mail"]);
                                }
                            }
                            else
                            {
                                usuarioAuxiliar.Id = 0;
                                usuarioAuxiliar.Nombre = string.Empty;
                                usuarioAuxiliar.Apellido = string.Empty;
                                usuarioAuxiliar.NombreUsuario = string.Empty;
                                usuarioAuxiliar.Contraseña = string.Empty;
                                usuarioAuxiliar.Mail = string.Empty;
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
            return usuarioAuxiliar;
        }


        public static Usuario TraerUsuarioXNombreUsuario(string NombreUsuario) //devuelve todos los usuarios en la base de datos por NombreUsuario
        {
            var usuarioAuxiliar = new Usuario();
            string consultaSQL = "SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] FROM Usuario WHERE NombreUsuario = @NombreUsuario";

            string connectionString = connectionStringMetodo();
            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@NombreUsuario", SqlDbType.VarChar) { Value = NombreUsuario });

                    try
                    {
                        conectionSql.Open();

                        using (SqlDataReader dataReader = comandoSql.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    
                                    usuarioAuxiliar.Id = Convert.ToInt32(dataReader["Id"]);
                                    usuarioAuxiliar.Nombre = Convert.ToString(dataReader["Nombre"]);
                                    usuarioAuxiliar.Apellido = Convert.ToString(dataReader["Apellido"]);
                                    usuarioAuxiliar.NombreUsuario = Convert.ToString(dataReader["NombreUsuario"]);
                                    usuarioAuxiliar.Contraseña = Convert.ToString(dataReader["Contraseña"]);
                                    usuarioAuxiliar.Mail = Convert.ToString(dataReader["Mail"]);
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
            return usuarioAuxiliar;
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
