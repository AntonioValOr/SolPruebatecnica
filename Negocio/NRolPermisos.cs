using Datos;
using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NRolPermisos

    {
        DatRolPermisos dat = new DatRolPermisos();

        public List<RolPermisos> ObtenerPorIdRol(int IdRol)
        {
            return dat.ObtenerPorIdRol(IdRol);
        }

        public bool RolTienePermiso(int idRol, int idPermiso)
        {
            var permisos = dat.ObtenerPorIdRol(idRol);
            return permisos.Any(rp => rp.IdPermiso == idPermiso);
        }

    }
}
