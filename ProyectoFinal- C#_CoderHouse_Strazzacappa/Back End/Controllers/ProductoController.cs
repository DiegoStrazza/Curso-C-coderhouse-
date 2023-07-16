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
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Producto> GetAllProductos()
        {
            return ProductoHandler.GetProductos();
        }

        [HttpGet("{idUsuario}")]
        public IEnumerable<Producto> GetProductoPorIdUsuario(int idUsuario)
        {
            return ProductoHandler.GetProductoByIdUsuario(idUsuario);
        }

        [HttpPut]//UPDATE en sql
        //Prueba Postman con JSON
        //{"Id":1,"Descripciones":"ProductoModificado","Costo":150,"PrecioVenta":250,"Stock":40,"IdUsuario":1}
        public bool PutProductos([FromBody] Producto producto)
        {
            
             return ProductoHandler.UpdateProductos(producto);
        }

        [HttpPost]//INSERT en sql
        //Prueba Postman con JSON
        //{"Descripciones":"altaProducto","Costo":100,"PrecioVenta":200,"Stock":50,"IdUsuario":1}
        public bool PostProducto([FromBody] Producto producto)
        {
            return ProductoHandler.InsertProducto(producto);
        }

        [HttpDelete]
        //Prueba Postman con JSON
        //1034
        public bool DeleteProductos([FromBody] int idProducto)
        {
            return ProductoHandler.DeleteProducto(idProducto);
        }
    }
}
