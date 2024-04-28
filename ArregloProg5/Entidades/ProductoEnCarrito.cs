using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
public class ProductoCarrito
    {
        public int idProductoCarrito { get; set; }
        public int idUsuario { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public decimal Precio { get; set; }
    }

}
