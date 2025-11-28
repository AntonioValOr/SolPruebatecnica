using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatRolPermisos
    {
        PruebaTecnicaEntities db = new PruebaTecnicaEntities();
        public List<RolPermisos> ObtenerPorIdRol(int idRol)
        {
            return db.RolPermisos
                     .Where(rp => rp.Roles.IdRol == idRol)
                     .ToList();
        }
        public RolPermisos ObtenerPorId(int idRolPermiso)
        {
            return db.RolPermisos
                     .FirstOrDefault(rp => rp.IdRolPermiso == idRolPermiso);
        }

    }

}
