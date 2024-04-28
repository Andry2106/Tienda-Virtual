using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Provincia
    {
        public int idProvincia { get; set; }
        public string Nombre { get; set; }
        public List<Canton> Cantones { get; set; }
    }
}
