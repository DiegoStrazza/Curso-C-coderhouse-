using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoFinalCoderHouse.ADO.NET;
using ProyectoFinalCoderHouse.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<ProductoVendido> _logger;

        public VentaController(ILogger<ProductoVendido> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<Venta> GetAllVentas()
        //{
        //    return VentaHandler.GetVentas();
        //}

        [HttpGet]
        public List<string> GetVentasConProductos()
        {
            return VentaHandler.GetVentasYProductos();
        }

        [HttpPost("{idUsuario}")]
        /*Prueba de JSON
        *[{"id": 2,"descripciones": "string","costo": 0,"precioVenta": 0,"stock": 2,"idUsuario": 0},
        *{"id": 3,"descripciones": "string","costo": 0,"precioVenta": 0,"stock": 4,"idUsuario": 0}]
        */

        //Recibe una lista de productos y el número de IdUsuario de quien la efectuó, primero cargar una nueva Venta en la base de datos,
        //luego debe cargar los productos recibidos en la base de ProductosVendidos uno por uno por un lado, y descontar el stock en la base de productos por el otro
        public bool PostVenta(List<Producto> productos, int idUsuario)
        {
            return VentaHandler.InsertVenta(productos, idUsuario);
        }

        [HttpDelete("{idVenta}")]
        /*Prueba de JSON
        *[{"id": 2,"descripciones": "string","costo": 0,"precioVenta": 0,"stock": 2,"idUsuario": 0},
        *{"id": 3,"descripciones": "string","costo": 0,"precioVenta": 0,"stock": 4,"idUsuario": 0}]
        */

        //Recibe una venta con su número de Id, debe buscar en la base de Productos Vendidos cuáles
        //lo tienen y eliminarlos, sumar el stock a los productos incluidos, y eliminar la venta de la base de datos.
        public bool DeleteVenta(int idVenta)
        {
            return VentaHandler.DeleteVentaByIdVenta(idVenta);
        }






    }
}
