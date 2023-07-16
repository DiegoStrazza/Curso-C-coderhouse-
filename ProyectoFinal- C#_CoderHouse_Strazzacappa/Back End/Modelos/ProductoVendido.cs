using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Modelos
{
    public class ProductoVendido
    {
        public int Id { get; set; } //bigint en sql
        public int Stock { get; set; } // int en sql
        public int IdProducto { get; set; } //bigint en sql
        public int IdVenta { get; set; } //bigint en sql

    }
}
