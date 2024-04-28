using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleVenta
    {
        public int idDetalleVenta { get; set; }
        public int idVenta { get; set; }
        public int idProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
    }
}
