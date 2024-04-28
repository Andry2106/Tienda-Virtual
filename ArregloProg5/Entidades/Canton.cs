using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Canton
    {
        public int idCanton { get; set; }
        public string Nombre { get; set; }
        public int idProvincia { get; set; }
        public Provincia Provincia { get; set; }
        public List<Distrito> Distritos { get; set; }
    }
}
