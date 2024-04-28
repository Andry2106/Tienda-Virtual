using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Distrito
    {
        public int idDistrito { get; set; }
        public string Nombre { get; set; }
        public int idCanton { get; set; }
        public Canton Canton { get; set; }
    }
}
