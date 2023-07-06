using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class Producto
    {
        //Declaración de atributos
        #region Atributos

        private int _Id, _IdUsuario; //bigint en sql
        private string _Descripciones; //varchar en sql
        private double _Costo, _PrecioVenta; //money en sql
        private int _Stock; //int en sql

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Descripciones { get; set; }
        public double Costo { get; set; }
        public double PrecioVenta { get; set; }
        public int Stock { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public Producto()
        {

            _Id = 0;
            _IdUsuario = 0;
            _Descripciones = string.Empty;
            _Costo = 0;
            _PrecioVenta = 0;
            _Stock = 0;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public Producto(int Id, int IdUsuario, string Descripciones, double Costo, double PrecioVenta, int Stock)
        {

            this._Id = Id;
            this._IdUsuario = IdUsuario;
            this._Descripciones = Descripciones;
            this._Costo = Costo;
            this._PrecioVenta = PrecioVenta;
            this._Stock = Stock;

        }
        #endregion constructor parametrizado

        //Declaración de métodos Sql

    }
}
