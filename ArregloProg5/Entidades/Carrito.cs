using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Carrito
    {
        public int idCarrito { get; set; }
        public int idUsuario { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
    }
}
