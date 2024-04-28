using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioToken
    {
        public int idToken { get; set; }
        public int idUsuario { get; set; }
        public string Token { get; set; }
    }
}
