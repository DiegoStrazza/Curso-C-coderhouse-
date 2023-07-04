using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProductoVendidoController : ApiController
    {
        [HttpGet] //SELECT en sql
        //Devuelve todos los elementos en la base de datos
        //Prueba en Postman
        //https://localhost:44336/api/ProductoVendido
        public List<ProductoVendido> GetProductosVendidos()
        {
            return ADO_ProductoVendido.TraerProductoVendidos();
        }


        [HttpPost] //INSERT en sql
        //Prueba Postman con JSON
        //{"Stock": 20,"IdProducto": 1,"IdVenta": 1}
        public string CrearProductoVendido([FromBody] ProductoVendido productoVendido)
        {
            ProductoVendido productoVendidoCreado = new ProductoVendido();
            productoVendidoCreado = ADO_ProductoVendido.CrearProductoVendido(productoVendido);

            if (productoVendidoCreado.Id != 0)
            {
                return "ProductoVendido cargado con éxito ";
            }
            else
            {
                return "No fue posible cargar el productoVendido ";
            }
        }



        [HttpPut] //UPDATE en sql
        //Prueba Postman con JSON
        //{"Id":2,"Stock": 25,"IdProducto": 1,"IdVenta": 1}
        public string ModificarProductoVendido([FromBody] ProductoVendido productoVendidoParaModificar)
        {
            ProductoVendido productoVendidoBD = ADO_ProductoVendido.TraerProductoVendidoXIdProductoVendido(productoVendidoParaModificar.Id);

            if (productoVendidoBD.Id != 0)//verificamos que el productoVendido exista en la BD
            {

                if (ADO_ProductoVendido.ModificarProductoVendido(productoVendidoParaModificar) != 0)//se modifica el productoVendido
                {
                    return "ProductoVendido modificado con éxito";
                }
                else
                {
                    return "No fue posible realizar las modificaciones sobre el productoVendido";
                }

            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el producto vendido. El producto vendido ingresado no existe.";
            }
        }



        [HttpDelete] //DELETE en sql
        //Prueba Postman con JSON
        //9
        public string EliminarProductoVendido([FromBody] int idProductoVendidoParaEliminar) //parámetros por POST
        {
            ProductoVendido productoVendidoBD = ADO_ProductoVendido.TraerProductoVendidoXIdProductoVendido(idProductoVendidoParaEliminar);

            if (productoVendidoBD.Id != 0)//verificamos que el productoVendido exista en la BD
            {
                if (ADO_ProductoVendido.EliminarProductoVendido(idProductoVendidoParaEliminar) != 0)//se elimina el productoVendido
                {
                    return "ProductoVendido eliminado con éxito";
                }
                else
                {
                    return "No fue posible eliminar el productoVendido";
                }

            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el producto vendido. El producto vendido ingresado no existe.";
            }
        }



    }
}
