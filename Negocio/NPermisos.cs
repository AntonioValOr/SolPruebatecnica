using Datos;
using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NPermisos
    {
        DatPermisos dat = new DatPermisos();
        public List<Permisos> ObtenerTodos()
        {
            return dat.ObtenerTodos();
        }
        public Permisos ObtenerPorId(int id)
        {
            return dat.ObtenerPorId(id);
        }


    }
}
