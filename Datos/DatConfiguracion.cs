using Datos.Model;
using System.Collections.Generic;
using System.Linq;

public class DatConfiguracion
{

    PruebaTecnicaEntities db = new PruebaTecnicaEntities();

    public List<TipoLetra> ObtenerTiposLetra()
    {
        return db.TipoLetra.ToList();
    }

    public List<TamanoLetra> ObtenerTamanosLetra()
    {
        return db.TamanoLetra.ToList();
    }

    public List<TamanoHoja> ObtenerTamanosHoja()
    {
        return db.TamanoHoja.ToList();
    }

    public TipoLetra ObtenerTipoLetraPorId(int id)
    {
        return db.TipoLetra.FirstOrDefault(t => t.IdTipoLetra == id);
    }

    public TamanoLetra ObtenerTamanoLetraPorId(int id)
    {
        return db.TamanoLetra.FirstOrDefault(t => t.IdTamano == id);
    }

    public TamanoHoja ObtenerTamanoHojaPorId(int id)
    {
        return db.TamanoHoja.FirstOrDefault(t => t.IdTamanoHoja == id);
    }
}
