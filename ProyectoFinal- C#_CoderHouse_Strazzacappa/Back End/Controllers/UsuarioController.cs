using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.ADO.NET;
using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public UsuarioController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        [HttpGet("{nombreUsuario}/{contraseña}")] //Inicio de sesión
        //Prueba en Postman
        //https://localhost:5001/api/Usuario/eperez/SoyErnestoPerez
        //https://localhost:5001/api/Usuario/asdf/sdf
        public Usuario GetUsuarioByContraseña(string nombreUsuario, string contraseña)
        {
            return UsuarioHandler.GetUsuarioByNombreUsuarioYContraseña(nombreUsuario, contraseña);
        }

        [HttpGet("{nombreUsuario}")]
        //Prueba Postman con JSON
        //https://localhost:5001/api/Usuario/eperez
        public Usuario GetUsuarioByNombre(string nombreUsuario) //traer usuario
        {
            return UsuarioHandler.GetUsuarioByNombreUsuario(nombreUsuario);
        }

        [HttpPost]
        //Prueba Postman con JSON
        //{"Nombre": "Nombre Nuevo","Apellido": "Apellido Nuevo","NombreUsuario": "NombreUsuario Nuevo",
        //"Contraseña": "Contraseña Nueva","Mail": "Mail Nuevo"}
        public bool PostUsuario([FromBody] Usuario usuario)// Crear Usuario
        {
            return UsuarioHandler.InsertUsuario(usuario);
        }

        [HttpPut]
        //Prueba Postman con JSON
        //{"Id":8,"Nombre":"Nombre Modificado","Apellido":"Apellido Modificado","NombreUsuario":"NombreUsuario Modificado",
        //"Contraseña":"Contraseña Modificada","Mail":"Mail Modificado"}
        public bool PutUsuario([FromBody] Usuario usuario)//Modificar usuario
        {
            return UsuarioHandler.UpdateUsuario(usuario);
        }

        [HttpDelete]
        public bool DeleteUsuario([FromBody] int idUsuarioParaEliminar) //Eliminar Usuario
        {
            return UsuarioHandler.DeleteUsuario(idUsuarioParaEliminar);
        }
    }
}
