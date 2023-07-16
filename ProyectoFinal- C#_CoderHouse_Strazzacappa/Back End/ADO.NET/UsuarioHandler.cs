using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProyectoFinalCoderHouse.ADO.NET
{
    public static class UsuarioHandler
    {
        //public static string connectionString = "Data Source=(localdb)\\Servidor Ejercicio;Initial Catalog=SistemaGestion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string connectionString = "Server=PROMETEO;Database=SistemaGestion;Trusted_Connection=True;";
        public static List<Usuario> GetUsuarios(DataTable table)
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (DataRow row in table.Rows)
            {
                Usuario getUsuario = new Usuario();
                getUsuario.Id = Convert.ToInt32(row["Id"]);
                getUsuario.Nombre = row["Nombre"].ToString();
                getUsuario.Apellido = row["Apellido"].ToString();
                getUsuario.NombreUsuario = row["NombreUsuario"].ToString();
                getUsuario.Contraseña = row["Contraseña"].ToString();
                getUsuario.Mail = row["Mail"].ToString();

                usuarios.Add(getUsuario);
            }
            return usuarios;
        }

        public static Usuario GetUsuarioByNombreUsuarioYContraseña(string nombreUsuario, string contraseña) //Inicio Sesión
        {

            List<Usuario> usuarios = new List<Usuario> ();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    command.CommandText = @" SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] 
                                FROM [SistemaGestion].[dbo].Usuario 
                                WHERE NombreUsuario = @nombreUsuario
                                AND   Contraseña = @contraseña;";

                    command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                    command.Parameters.AddWithValue("@contraseña", contraseña);

                    command.Connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)//la consulta no trae resultados
                    {
                        return new Usuario();
                        throw new Exception("Error: Nombre de usuario o contraseña incorrecto");
                    }

                    usuarios = GetUsuarios(table);

                    command.Connection.Close();
                }
            }
            return usuarios[0];
        }

        public static bool InsertUsuario(Usuario usuario) //Crear Usuario
        {
            bool alta = false;
            Usuario usuarioRepetido = GetUsuarioByNombreUsuario(usuario.NombreUsuario);

            if (usuario.NombreUsuario == null || 
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contraseña == null ||
                usuario.Contraseña.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return alta;
                throw new Exception("Faltan datos obligatorios");
            }
            else if(usuarioRepetido.Id != 0)
            {
                return alta;
                throw new Exception("El nombre de usuario ya existe");
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Connection.Open();
                sqlCommand.CommandText = @"INSERT INTO [SistemaGestion].[dbo].Usuario
                                    ([Nombre]
                                    ,[Apellido]
                                    ,[NombreUsuario]
									,[Contraseña]
									,[Mail] )
                                    VALUES
                                    (@Nombre,
                                        @Apellido,
                                        @NombreUsuario,
										@Contraseña,
										@Mail)";

                sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                sqlCommand.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                sqlCommand.Parameters.AddWithValue("@Mail", usuario.Mail);

                sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente el INSERT INTO
                usuario.Id = GetId.Get(sqlCommand);

                alta = usuario.Id != 0 ? true : false;
                sqlCommand.Connection.Close();
                return alta;
                
            }
        }

        public static bool UpdateUsuario(Usuario usuario) //Modificar Usuario
        {
            bool modificado = false;

            if (usuario.NombreUsuario == null ||
                usuario.NombreUsuario.Trim() == "" ||
                usuario.Contraseña == null ||
                usuario.Contraseña.Trim() == "" ||
                usuario.Nombre == null ||
                usuario.Nombre.Trim() == "" ||
                usuario.Apellido == null ||
                usuario.Apellido.Trim() == "")
            {
                return modificado;
                throw new Exception("Faltan datos obligatorios");
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandText = @" UPDATE [SistemaGestion].[dbo].Usuario
                                                SET 
                                                   Nombre = @Nombre,
                                                   Apellido = @Apellido,
                                                   NombreUsuario = @NombreUsuario,
										           Contraseña = @Contraseña,
										           Mail = @Mail
                                                WHERE id = @ID";

                        sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        sqlCommand.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                        sqlCommand.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        sqlCommand.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                        sqlCommand.Parameters.AddWithValue("@Mail", usuario.Mail);
                        sqlCommand.Parameters.AddWithValue("@ID", usuario.Id);


                        int recordsAffected = sqlCommand.ExecuteNonQuery(); //Se ejecuta realmente UPDATE

                        sqlCommand.Connection.Close();
                        if (recordsAffected == 0){
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

        public static Usuario GetUsuarioByNombreUsuario(string nombreUsuario) //traer usuario
        {
            //Usuario usuario = new Usuario();
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    
                    command.CommandText = @"SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] 
                                FROM [SistemaGestion].[dbo].Usuario 
                                WHERE NombreUsuario = @nombreUsuario;";

                    command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);

                    command.Connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count < 1)
                    {
                        return new Usuario();
                    }


                   usuarios = GetUsuarios(table);

                    command.Connection.Close();
                }
            }
            return usuarios[0];
        }

        public static bool DeleteUsuario(int usuarioId) //Eliminar Usuario 
        {
            int filasAfectadasSql = 0;

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                string querySql = "DELETE FROM [SistemaGestion].[dbo].Usuario WHERE Id=@Id";

                using (SqlCommand comandoSql = new SqlCommand(querySql, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = usuarioId });
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
            return filasAfectadasSql > 0 ? true : false;
        }

        public static Usuario GetUsuarioByIdUsuario(int idUsuario) //devuelve un usuario buscado por su ID
        {
            var usuarioAuxiliar = new Usuario();
            string consultaSQL = "SELECT [Id],[Nombre],[Apellido],[NombreUsuario],[Contraseña],[Mail] FROM Usuario WHERE Id = @Id";

            using (SqlConnection conectionSql = new SqlConnection(connectionString))
            {
                using (SqlCommand comandoSql = new SqlCommand(consultaSQL, conectionSql))
                {
                    comandoSql.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = idUsuario });

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
            //Console.WriteLine(errorMessages.ToString());
            //Console.WriteLine("Se finaliza la consola de aplicación debido a los errores capturados");
            //System.Environment.Exit(0);
        }




    }
}
