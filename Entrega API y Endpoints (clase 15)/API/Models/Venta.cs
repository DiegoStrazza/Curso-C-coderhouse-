using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class Venta
    {
        //Declaración de atributos
        #region Atributos

        private int _Id, _IdUsuario; //bigint en sql
        private string _Comentarios; //varchar(max) en sql

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Comentarios { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public Venta()
        {

            _Id = 0;
            _IdUsuario = 0;
            _Comentarios = string.Empty;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public Venta(int Id, int IdUsuario, string Comentarios)
        {

            this._Id = Id;
            this._IdUsuario = IdUsuario;
            this._Comentarios = Comentarios;

        }
        #endregion constructor parametrizado

        //Declaración de métodos

    }
}
