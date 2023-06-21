// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography.X509Certificates;

namespace PrimerosPasos
{
    class ProgramaPrincipal
    {
        public static void Main(string[] args)
        {
            //--------------------------------------------------------------------------
            //Test del ABM de Producto
            //TestProducto();

            //List<Producto> listaProductosIdUsuario = new List<Producto>();
            //listaProductosIdUsuario = Producto.TraerProductoXIdUsuario(2);
            //MostrarTestProductoXIdUsuario(listaProductosIdUsuario);
            //--------------------------------------------------------------------------
            //Test del ABM de Venta
            //TestVenta();

            //List<Venta> listaVentas = new List<Venta>();
            //listaVentas = Venta.TraerVentasXIdUsuario(1);
            //MostrarTestVentaXIdUsuario(listaVentas);

            //--------------------------------------------------------------------------
            //Test del ABM de Usuario
            //TestUsuario();

            //Usuario consultaUsuario = new Usuario();
            //consultaUsuario = Usuario.TraerUsuarioXNombreUsuario("tcasazza");
            //MostrarUsuario (consultaUsuario);
            //--------------------------------------------------------------------------
            //Test del ABM de ProductoVendido
            //TestProductoVendido();

            //List<ProductoVendido> listaProductoVendidos = new List<ProductoVendido>();
            //listaProductoVendidos = ProductoVendido.TraerProductoVendidosXIdUsuario(1);
            //MostrarTestProductoVendidoXIdUsuario(listaProductoVendidos);

            //--------------------------------------------------------------------------
            Usuario usuarioInicioSesion = new Usuario();
            usuarioInicioSesion = InicioSesion("tcasazza", "ContraseñaModificada");
            if (usuarioInicioSesion.Id != 0)
            {
                Console.WriteLine("Inicio de sesión exitoso con el usuario: "+ usuarioInicioSesion.NombreUsuario);
                MostrarUsuario(usuarioInicioSesion);
            }
            else { Console.WriteLine("Nombre de usuario o contraseña incorrecto"); }
            //--------------------------------------------------------------------------
        }

        public static Usuario InicioSesion(string nombreUsuario, string contraseña)
        {
            //valida si el usuario puede iniciar sesion
            return Usuario.DevolverUsuarioXNombreUsuarioYContraseña(nombreUsuario, contraseña);
        }


        public static void TestProducto()
        {
            //Prueba del ABM de Producto
            Producto productoTest = new Producto();
            productoTest.Descripciones = "Sandalia";
            productoTest.Costo = 1300;
            productoTest.PrecioVenta = 1600;
            productoTest.Stock = 40;
            productoTest.IdUsuario = 3;

            ////Alta
            productoTest = Producto.CrearProducto(productoTest);
            Console.WriteLine("Se da de alta el Producto " + productoTest.Descripciones + " con ID: " + productoTest.Id);
            MostrarTestProducto();

            //Modificacion
            productoTest.Stock = 35;
            Producto.ModificarProducto(productoTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se modifica de " + productoTest.Descripciones + " con ID: " + productoTest.Id + " el stock a " + productoTest.Stock);
            MostrarTestProducto();

            //Baja
            Producto.EliminarProducto(productoTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se elimina el producto " + productoTest.Descripciones + " con ID: " + productoTest.Id);
            MostrarTestProducto();

            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("ABM de Producto exitoso");

        }
        public static void MostrarTestProducto()
        {
            List<Producto> listaProductos = new List<Producto>();
            listaProductos = Producto.DevolverProductos();
            Console.WriteLine("Mostrar todos los productos:");
            foreach (Producto producto in listaProductos)
            {
                Console.WriteLine("Id: " + producto.Id);
                Console.WriteLine("Descripciones: " + producto.Descripciones);
                Console.WriteLine("Costo " + producto.Costo);
                Console.WriteLine("PrecioVenta: " + producto.PrecioVenta);
                Console.WriteLine("Stock: " + producto.Stock);
                Console.WriteLine("IdUsuario: " + producto.IdUsuario);

                Console.WriteLine("----------------");
            }
        }
        public static void MostrarTestProductoXIdUsuario(List<Producto> listaProductosIdUsuario)
        {
            Console.WriteLine("Mostrar todos los productos de un usuario: ");
            foreach (Producto producto in listaProductosIdUsuario)
            {
                Console.WriteLine("Id: " + producto.Id);
                Console.WriteLine("Descripciones: " + producto.Descripciones);
                Console.WriteLine("Costo " + producto.Costo);
                Console.WriteLine("PrecioVenta: " + producto.PrecioVenta);
                Console.WriteLine("Stock: " + producto.Stock);
                Console.WriteLine("IdUsuario: " + producto.IdUsuario);

                Console.WriteLine("----------------");
            }
        }


        public static void TestVenta()
        {
            //Prueba del ABM de Venta
            Venta ventaTest = new Venta();
            ventaTest.Comentarios = "Sandalia";
            ventaTest.IdUsuario = 3;

            ////Alta
            ventaTest = Venta.CrearVenta(ventaTest);
            //ventaTest.Id = idVentaNuevo;
            Console.WriteLine("Se da de alta una Venta " + ventaTest.Comentarios + " con ID: " + ventaTest.Id);
            MostrarTestVenta();

            //Modificacion
            ventaTest.Comentarios = "Modificacion de comentarios de venta";
            Venta.ModificarVenta(ventaTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se modifica el comentario a " + ventaTest.Comentarios + " del ID: " + ventaTest.Id);
            MostrarTestVenta();

            //Baja
            Venta.EliminarVenta(ventaTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se elimina la venta " + ventaTest.Comentarios + " con ID: " + ventaTest.Id);
            MostrarTestVenta();

            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("ABM de Venta exitoso");

        }
        public static void MostrarTestVenta()
        {
            List<Venta> listaVentas = new List<Venta>();
            listaVentas = Venta.DevolverVentas();
            Console.WriteLine("Mostrar todas las ventas:");
            foreach (Venta venta in listaVentas)
            {
                Console.WriteLine("Id: " + venta.Id);
                Console.WriteLine("Descripciones: " + venta.Comentarios);
                Console.WriteLine("IdUsuario: " + venta.IdUsuario);

                Console.WriteLine("----------------");
            }
        }
        public static void MostrarTestVentaXIdUsuario(List<Venta> listaVentas)
        {
            Console.WriteLine("Mostrar todas las ventas:");
            foreach (Venta venta in listaVentas)
            {
                Console.WriteLine("Id: " + venta.Id);
                Console.WriteLine("Descripciones: " + venta.Comentarios);
                Console.WriteLine("IdUsuario: " + venta.IdUsuario);

                Console.WriteLine("----------------");
            }
        }


        public static void TestUsuario()
        {
            //Prueba del ABM de Usuario
            Usuario usuarioTest = new Usuario();
            usuarioTest.Nombre = "Usuario";
            usuarioTest.Apellido = "Test";
            usuarioTest.NombreUsuario = "UsuarioTest";
            usuarioTest.Contraseña = "PassUsuarioTest";
            usuarioTest.Mail = "UsuarioTest@gmail.com";

            ////Alta
            usuarioTest = Usuario.CrearUsuario(usuarioTest);
            Console.WriteLine("Se da de alta el Usuario " + usuarioTest.Nombre + " " + usuarioTest.Apellido + " con ID: " + usuarioTest.Id);
            MostrarTestUsuario();

            //Modificacion
            usuarioTest.Apellido = "Test Modificado";
            Usuario.ModificarUsuario(usuarioTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se modifica el usuario con ID: " + usuarioTest.Id + " el apellido a " + usuarioTest.Apellido);
            MostrarTestUsuario();

            //Baja
            Usuario.EliminarUsuario(usuarioTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se elimina el usuario " + usuarioTest.Nombre + " " + usuarioTest.Apellido + " con ID: " + usuarioTest.Id);
            MostrarTestUsuario();

            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("ABM de Usuario exitoso");
        }
        public static void MostrarTestUsuario()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            listaUsuarios = Usuario.DevolverUsuarios();
            Console.WriteLine("Mostrar todos los usuarios:");
            foreach (Usuario usuario in listaUsuarios)
            {
                Console.WriteLine("Id: " + usuario.Id);
                Console.WriteLine("Nombre: " + usuario.Nombre);
                Console.WriteLine("Apellido: " + usuario.Apellido);
                Console.WriteLine("NombreUsuario: " + usuario.NombreUsuario);
                Console.WriteLine("Contraseña: " + usuario.Contraseña);
                Console.WriteLine("Mail: " + usuario.Mail);

                Console.WriteLine("----------------");
            }
        }
        public static void MostrarUsuario( Usuario usuario)
        {
            //List<Usuario> listaUsuarios = new List<Usuario>();
            //listaUsuarios = Usuario.DevolverUsuarios();
            Console.WriteLine("Mostrar un usuarios:");

                Console.WriteLine("Id: " + usuario.Id);
                Console.WriteLine("Nombre: " + usuario.Nombre);
                Console.WriteLine("Apellido: " + usuario.Apellido);
                Console.WriteLine("NombreUsuario: " + usuario.NombreUsuario);
                Console.WriteLine("Contraseña: " + usuario.Contraseña);
                Console.WriteLine("Mail: " + usuario.Mail);

                Console.WriteLine("----------------");
            
        }



        public static void TestProductoVendido()
        {
            //Prueba del ABM de ProductoVendido
            ProductoVendido productoVendidoTest = new ProductoVendido();
            productoVendidoTest.Stock = 25;
            productoVendidoTest.IdProducto = 1;
            productoVendidoTest.IdVenta = 2;

            ////Alta
            productoVendidoTest = ProductoVendido.CrearProductoVendido(productoVendidoTest);
            Console.WriteLine("Se da de alta el ProductoVendido " + productoVendidoTest.IdProducto + " con ID: " + productoVendidoTest.Id);
            MostrarTestProductoVendido();

            //Modificacion
            productoVendidoTest.Stock = 15;
            //productoVendidoTest.IdVenta = 150;
            ProductoVendido.ModificarProductoVendido(productoVendidoTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se modifica con ID: " + productoVendidoTest.Id + " el stock a " + productoVendidoTest.Stock);
            MostrarTestProductoVendido();

            //Baja
            ProductoVendido.EliminarProductoVendido(productoVendidoTest);
            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("Se elimina el productoVendido " + productoVendidoTest.IdProducto + " con ID: " + productoVendidoTest.Id);
            MostrarTestProductoVendido();

            Console.WriteLine("------------------------------------------------------------------------------------------------");
            Console.WriteLine("ABM de ProductoVendido exitoso");

        }
        public static void MostrarTestProductoVendido()
        {
            List<ProductoVendido> listaProductoVendidos = new List<ProductoVendido>();
            listaProductoVendidos = ProductoVendido.DevolverProductoVendidos();
            Console.WriteLine("Mostrar todos los productoVendidos:");
            foreach (ProductoVendido productovendido in listaProductoVendidos)
            {
                Console.WriteLine("Id: " + productovendido.Id);
                Console.WriteLine("Stock: " + productovendido.Stock);
                Console.WriteLine("IdProducto " + productovendido.IdProducto);
                Console.WriteLine("IdVenta: " + productovendido.IdVenta);

                Console.WriteLine("----------------");
            }
        }
        public static void MostrarTestProductoVendidoXIdUsuario(List<ProductoVendido> listaProductoVendidos)
        {
            Console.WriteLine("Mostrar todos los productoVendidos:");
            foreach (ProductoVendido productovendido in listaProductoVendidos)
            {
                Console.WriteLine("Id: " + productovendido.Id);
                Console.WriteLine("Stock: " + productovendido.Stock);
                Console.WriteLine("IdProducto " + productovendido.IdProducto);
                Console.WriteLine("IdVenta: " + productovendido.IdVenta);

                Console.WriteLine("----------------");
            }
        }

    }
}