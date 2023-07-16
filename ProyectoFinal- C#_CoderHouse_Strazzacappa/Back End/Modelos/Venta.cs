using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Modelos
{
    public class Venta
    {
        public int Id { get; set; }  //bigint en sql
        public string Comentarios { get; set; } //varchar(max) en sql
        public int IdUsuario { get; set; } //bigint en sql
    }
}
