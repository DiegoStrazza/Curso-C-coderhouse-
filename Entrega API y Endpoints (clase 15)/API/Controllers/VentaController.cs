using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class VentaController : ApiController
    {
        public object Enviroment { get; private set; }

        [HttpGet] //SELECT en sql
        //Devuelve todos los elementos en la base de datos
        //Prueba en Postman
        //https://localhost:44336/api/Venta
        public List<Venta> GetVentas()
        {
            return ADO_Venta.TraerVentas();
        }




        [HttpPost] //INSERT en sql
        //Prueba Postman con JSON
        //{"listaProductos": [{"Id":2,"Descripciones": "Pantalon Jean","Costo": 800,"PrecioVenta": 4000,"Stock": 1,"IdUsuario": 1},
        //{"Id":1,"Descripciones": "Remera manga larga","Costo": 500,"PrecioVenta": 1400,"Stock": 1,"IdUsuario": 1}],
        //"venta": {"Comentarios": "Comentarios Nuevo","IdUsuario": 1}}

        //Recibe una lista de productos y el número de IdUsuario de quien la efectuó, primero cargar una nueva Venta en la base de datos,
        //luego debe cargar los productos recibidos en la base de ProductosVendidos uno por uno por un lado, y descontar el stock en la base de productos por el otro.
        public string CargarVenta([FromBody] ListaProductosYVenta listaProductosYVenta)
        {

            List<Producto> listaProductos = listaProductosYVenta.listaProductos;
            Venta venta = listaProductosYVenta.venta;

            string mensajeReturn = "";

            Venta ventaCreada = new Venta();
            ventaCreada = ADO_Venta.CrearVenta(venta);

            if (ventaCreada.Id != 0) //se cargó la venta con éxito
            {
                mensajeReturn = "Venta cargada con éxito!!!";

                //se cargan los productos vendidos en su base de datos ProductoVendido y descuenta el stock en la DB Producto
                ProductoVendido productoVendido = new ProductoVendido();
                productoVendido.IdVenta = ventaCreada.Id;

                Producto productoBaseDatos = new Producto();

                foreach (Producto productoForEach in listaProductos)
                {
                    productoVendido.IdProducto = productoForEach.Id;
                    productoVendido.Stock = productoForEach.Stock;
                    //se carga el producto vendido
                    productoVendido = ADO_ProductoVendido.CrearProductoVendido(productoVendido);

                    if (productoVendido.Id != 0)
                    {
                        mensajeReturn += " El producto vendido " + productoForEach.Descripciones + " fue cargado con éxito.";
                    }
                    else
                    {
                        return "No fue posible cargar el productoVendido " + productoForEach.Descripciones;
                    }

                    //Se descuenta el stock al producto
                    productoBaseDatos = ADO_Producto.TraerProductoXIdProducto(productoForEach.Id);//traigo el producto desde la base de datos

                    //if (productoBaseDatos.Stock > productoForEach.Stock) //verifico que tengo stock disponible
                    //{
                        productoBaseDatos.Stock -= productoForEach.Stock; //actualizo el stock
                        productoBaseDatos.IdUsuario = productoForEach.IdUsuario; //actualizo el usuario que realizó la última modificación del item

                        int numeroFilasAfectadas = ADO_Producto.ModificarStockProducto(productoBaseDatos);

                        if (numeroFilasAfectadas != 0)
                        {
                            mensajeReturn += " Se actualiza el stock del producto " + productoForEach.Descripciones + " a " + productoBaseDatos.Stock + ".";
                        }
                        else
                        {
                            return "No se pudo actualizar el stock del producto " + productoForEach.Descripciones;
                        }

                    //}
                    //else
                    //{
                    //    return "No hay stock disponible del producto " + productoForEach.Descripciones;
                    //}
                }
            }
            else
            {
                return "No fue posible cargar la venta ";
            }

            return mensajeReturn;
        }

        public class ListaProductosYVenta
        {
            public List<Producto> listaProductos { get; set; }
            public Venta venta { get; set; }
        }


        //[HttpPost] //INSERT en sql
        ////Prueba Postman con JSON
        ////{"Comentarios": "Comentarios Nuevo","IdUsuario": 1}
        //public string CrearVenta([FromBody] Venta venta) //crea una venta en la BD
        //{
        //    Venta ventaCreada = new Venta();
        //    ventaCreada = ADO_Venta.CrearVenta(venta);

        //    if (ventaCreada.Id != 0)
        //    {
        //        return "Venta cargada con éxito ";
        //    }
        //    else
        //    {
        //        return "No fue posible cargar el venta ";
        //    }
        //}



        [HttpPut] //UPDATE en sql
        //Prueba Postman con JSON
        //{"Id":2,"Comentarios": "Comentarios Modificado","IdUsuario": 1}
        public string ModificarVenta([FromBody] Venta ventaParaModificar)
        {

            Venta ventaBD = ADO_Venta.TraerVentaXIdVenta(ventaParaModificar.Id);

            if (ventaBD.Id != 0)//verificamos que el venta exista en la BD
            {

                if (ADO_Venta.ModificarVenta(ventaParaModificar) != 0)
                {
                    return "Venta modificada con éxito";
                }
                else
                {
                    return "No fue posible realizar las modificaciones sobre la venta";
                }
            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el venta. El venta ingresado no existe.";
            }
        }



        [HttpDelete] //DELETE en sql
        //Prueba Postman con JSON
        //2
        public string EliminarVenta([FromBody] int idVentaParaEliminar) //parámetros por POST
        {

            Venta ventaBD = ADO_Venta.TraerVentaXIdVenta(idVentaParaEliminar);

            if (ventaBD.Id != 0)//verificamos que el venta exista en la BD
            {

                if (ADO_Venta.EliminarVenta(idVentaParaEliminar) != 0)//se elimina la venta
                {
                    return "Venta eliminada con éxito";
                }
                else
                {
                    return "No fue posible eliminar la venta";
                }
            }
            else
            {
                return "No fue posible realizar las modificaciones sobre el venta. El venta ingresado no existe.";
            }
        }


        
    }
}
