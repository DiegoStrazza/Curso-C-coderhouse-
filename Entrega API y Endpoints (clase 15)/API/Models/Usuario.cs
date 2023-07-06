using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class Usuario
    {
        //Declaración de atributos
        #region Atributos

        private int _Id; //bigint en sql
        private string _Nombre, _Apellido, _NombreUsuario, _Contraseña, _Mail; //varchar(max) en sql
        

        #endregion Atributos

        //Declaración Getter y Setter forma abreviada
        #region getter and setter
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Mail { get; set; }

        #endregion getter and setter
        //Constructor por defecto
        public Usuario()
        {

            _Id = 0;
            _Nombre = string.Empty;
            _Apellido = string.Empty;
            _NombreUsuario = string.Empty;
            _Contraseña = string.Empty;
            _Mail = string.Empty;

        }

        //Constructor parametrizado
        #region constructor parametrizado
        public Usuario(int Id, string Nombre, string Apellido, string NombreUsuario,string Contraseña, string Mail)
        {

            this._Id = Id;
            this._Nombre = Nombre;
            this._Apellido = Apellido;
            this._NombreUsuario = NombreUsuario;
            this._Contraseña = Contraseña;
            this._Mail = Mail;

        }
        #endregion constructor parametrizado

        //Declaración de métodos
        

    }
}
