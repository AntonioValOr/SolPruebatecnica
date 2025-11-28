using Datos;
using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NConfiguracion
    {
        DatConfiguracion dat = new DatConfiguracion();
        public List<TipoLetra> ObtenerTiposLetra()
        {
            return dat.ObtenerTiposLetra();
        }
        public List<TamanoLetra> ObtenerTamanosLetra()
        {
            return dat.ObtenerTamanosLetra();
        }
        public List<TamanoHoja> ObtenerTamanosHoja()
        {
            return dat.ObtenerTamanosHoja();
        }
        public TipoLetra ObtenerTipoLetraPorId(int id)
        {
            return dat.ObtenerTipoLetraPorId(id);
        }
        public TamanoLetra ObtenerTamanoLetraPorId(int id)
        {
            return dat.ObtenerTamanoLetraPorId(id);
        }
        public TamanoHoja ObtenerTamanoHojaPorId(int id)
        {
            return dat.ObtenerTamanoHojaPorId(id);
        }
    }
}
