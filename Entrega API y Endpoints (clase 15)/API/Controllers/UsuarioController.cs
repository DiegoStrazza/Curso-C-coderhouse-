using API.Repository;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {



        [HttpGet] //SELECT en sql
        //Devuelve todos los elementos en la base de datos
        //Prueba en Postman
        //https://localhost:44336/api/Usuario
        public List<Usuario> GetUsuarios()//Devuelve todos los elementos en la base de datos
        {
            return ADO_Usuario.TraerUsuarios();
        }




        [HttpGet] //SELECT en sql
        //Prueba en Postman
        //https://localhost:44336/api/Usuario?nombreUsuario=Diego&contrasenaUsuario=1234 usuario incorrecto
        //https://localhost:44336/api/Usuario?nombreUsuario=eperez&contrasenaUsuario=SoyErnestoPerez usuario correcto
        public string InicioSesion(string nombreUsuario, string contraseñaUsuario)
        {
            Usuario usuarioInicioSesion = new Usuario();
            usuarioInicioSesion = ADO_Usuario.TraerUsuarioXNombreUsuarioYContraseña(nombreUsuario, contraseñaUsuario);
            
            if (usuarioInicioSesion.Id != 0)
            {
                return "Bienvenido usuario " + nombreUsuario;
            }
            else
            {
                return "Usuario (" + nombreUsuario + ") o contraseña ("+ contraseñaUsuario + ") incorrecto";
            }
        }




        [HttpPost] //INSERT en sql
        //Prueba Postman con JSON
        //{"Nombre": "Nombre Nuevo","Apellido": "Apellido Nuevo","NombreUsuario": "NombreUsuario Nuevo",
        //"Contraseña": "Contraseña Nueva","Mail": "Mail Nuevo"}
        public string CrearUsuario([FromBody] Usuario usuario)
        {
            Usuario usuarioCreado = new Usuario();
            usuarioCreado = ADO_Usuario.CrearUsuario(usuario);

            if (usuarioCreado.Id != 0)
            {
                return "Usuario cargado con éxito ";
            }
            else
            {
                return "No fue posible cargar el usuario " + usuarioCreado.NombreUsuario;
            }
        }



        [HttpPut] //UPDATE en sql
        //Prueba Postman con JSON
        //{"Id":8,"Nombre":"Nombre Modificado","Apellido":"Apellido Modificado","NombreUsuario":"NombreUsuario Modificado",
        //"Contraseña":"Contraseña Modificada","Mail":"Mail Modificado"}
        public string ModificarUsuario([FromBody] Usuario usuarioParaModificar)
        {
            Usuario usuarioBD = ADO_Usuario.TraerUsuarioXIdUsuario(usuarioParaModificar.Id);

            if (usuarioBD.Id != 0)//verificamos que el usuario exista en la BD
            {

                if ( ADO_Usuario.ModificarUsuario(usuarioParaModificar) != 0)//se modifica el usuario
                {
                    return "Usuario modificado con éxito";
                }
                else
                {
                    return "No fue posible realizar las modificaciones sobre el usuario";
                }

            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el usuario. El usuario ingresado no existe.";
            }
        }



        [HttpDelete] //DELETE en sql
        //Prueba Postman con JSON
        //9
        public string EliminarUsuario([FromBody] int idUsuarioParaEliminar) //parámetros por POST
        {
            Usuario usuarioBD = ADO_Usuario.TraerUsuarioXIdUsuario(idUsuarioParaEliminar);

            if (usuarioBD.Id != 0)//verificamos que el usuario exista en la BD
            {

                if (ADO_Usuario.EliminarUsuario(idUsuarioParaEliminar) != 0)//se elimina el usuario
                {
                    return "Usuario eliminado con éxito";
                }
                else
                {
                    return "No fue posible eliminar el usuario";
                }
            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el usuario. El usuario ingresado no existe.";
            }
        }




        private static string TratamientoCatchExcepcion(Exception ex)
        {
            return ex.Message.ToString();
            //System.Environment.Exit(0);
        }


    }

}
