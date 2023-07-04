using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class ProductoVendido{

        //Declaración de atributos
        #region Atributos

        private int _Id, _IdProducto, _IdVenta; //bigint en sql
        private int _Stock; // int en sql

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdVenta { get; set; }
        public int Stock { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public ProductoVendido() {

            _Id = 0;
            _IdProducto = 0;
            _IdVenta = 0;
            _Stock = 0;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public ProductoVendido(int Id, int IdProducto, int IdVenta, int Stock)
        {

            this._Id = Id;
            this._IdProducto = IdProducto;
            this._IdVenta = IdVenta;
            this._Stock = Stock;

        }
        #endregion constructor parametrizado

        //Declaración de métodos

    }
}
