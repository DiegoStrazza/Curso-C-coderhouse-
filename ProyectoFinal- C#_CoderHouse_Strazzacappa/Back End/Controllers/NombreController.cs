using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NombreController : ControllerBase
    {
        /*Este apartado no se suele hacer en los trabajos,
        pero es la forma de que puedan ponerle su nombre a su App sin tocar el Front End*/
        [HttpGet]
        public string Get()
        {
            return "Proyecto Final CoderHouse c# - Sistema de Gestion";
        }
    }
}
