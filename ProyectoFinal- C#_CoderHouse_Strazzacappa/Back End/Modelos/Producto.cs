using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Modelos
{
    public class Producto
    {
        public int Id { get; set; } //bigint en sql
        public string Descripciones { get; set; } //varchar en sql
        public double Costo { get; set; } //money en sq
        public double PrecioVenta { get; set; } //money en sq
        public int Stock { get; set; } //int en sql
        public int IdUsuario { get; set; } //bigint en sql
    }
}
