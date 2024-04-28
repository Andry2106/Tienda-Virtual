using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Venta
    {
        public int idVenta { get; set; }
        public int idUsuario { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
