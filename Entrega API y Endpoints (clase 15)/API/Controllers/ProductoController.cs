using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProductoController : ApiController
    {
        [HttpGet] //SELECT en sql
        //Devuelve todos los elementos en la base de datos
        //Prueba en Postman
        //https://localhost:44336/api/Producto
        public List<Producto> GetProductos()
        {
            return ADO_Producto.TraerProductos();
        }

        [HttpPost] //INSERT en sql
        //Prueba Postman con JSON
        //{"Descripciones":"altaProducto","Costo":100,"PrecioVenta":200,"Stock":50,"IdUsuario":1}
        public string CrearProducto([FromBody] Producto producto)
        {
            Producto productoCreado = new Producto();
            productoCreado = ADO_Producto.CrearProducto(producto);

            if (productoCreado.Id != 0)
            {
                return "Producto cargado con éxito ";
            }
            else
            {
                return "No fue posible cargar el producto " + productoCreado.Descripciones;
            }
        }

        [HttpPut] //UPDATE en sql
        //Prueba Postman con JSON
        //{"Id":1,"Descripciones":"ProductoModificado","Costo":150,"PrecioVenta":250,"Stock":40,"IdUsuario":1}
        //
        public string ModificarProducto([FromBody] Producto productoParaModificar)
        {
            Producto productoBD = ADO_Producto.TraerProductoXIdProducto(productoParaModificar.Id);

            if (productoBD.Id  != 0)//verificamos que el producto exista en la BD
            {
                if (ADO_Producto.ModificarProducto(productoParaModificar) != 0)//se modifica el producto
                {
                    return "Producto modificado con éxito";
                }
                else
                {
                    return "No fue posible realizar las modificaciones sobre el producto";
                }
            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el producto. El producto ingresado no existe.";
            }

        }



        [HttpDelete] //DELETE en sql
        //Prueba Postman con JSON
        //5
        public string EliminarProducto([FromBody] int idProductoParaEliminar) //parámetros por POST
        {
            string mensajeProductosVendidosEliminados = "";
            int cantidadProductosVendidosEliminados = 0;

            Producto productoBD = ADO_Producto.TraerProductoXIdProducto(idProductoParaEliminar);

            if (productoBD.Id != 0)//verificamos que el producto exista en la BD
            {

                //eliminar los productos vendidos del idProductoParaEliminar (dependencia por foreign key en base datos SQL)
                cantidadProductosVendidosEliminados = ADO_ProductoVendido.EliminarProductoVendidoXIdProducto(idProductoParaEliminar); //se eliminan los Productos vendidos con el idProductoParaEliminar
            
                if (cantidadProductosVendidosEliminados  != 0 ) 
                {
                    mensajeProductosVendidosEliminados = " Además, se eliminaron "+ cantidadProductosVendidosEliminados +" productos vendidos asociados al producto " + idProductoParaEliminar;
                }
                if (ADO_Producto.EliminarProducto(idProductoParaEliminar) != 0)//se elimina el producto
                {
                    return "Producto eliminado con éxito." + mensajeProductosVendidosEliminados;
                }
                else
                {
                    return "No fue posible eliminar el producto";
                }
            }
            else
            {
                return "No fue posible eliminar el producto. El producto ingresado no existe.";
            }
        }
    }
}
