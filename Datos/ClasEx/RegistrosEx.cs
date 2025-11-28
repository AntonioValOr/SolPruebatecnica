using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.ClasEx
{
    public class RegistrosEx
    {
        public int IdRegistro { get; set; }
        public string Nombre { get; set; }
        public string Contrato { get; set; }
        public decimal Saldos { get; set; }
        public DateTime Fecha { get; set; }
        public string Telefono { get; set; }

        public TipoLetra TipoLetra { get; set; }
        public TamanoLetra TamanoLetra { get; set; }
        public TamanoHoja TamanoHoja { get; set; }
    }
}
