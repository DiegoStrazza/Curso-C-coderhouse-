using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }//bigint en sql
        public string Nombre { get; set; } //varchar(max) en sql
        public string Apellido { get; set; } //varchar(max) en sql
        public string NombreUsuario { get; set; } //varchar(max) en sql
        public string Contraseña { get; set; } //varchar(max) en sql
        public string Mail { get; set; } //varchar(max) en sql

    }
}
