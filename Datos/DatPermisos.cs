using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatPermisos
    {
       PruebaTecnicaEntities db= new PruebaTecnicaEntities();

        public List<Permisos>ObtenerTodos()
        {
            return db.Permisos.ToList();
        }
        public Permisos ObtenerPorId(int id)
        {
            return db.Permisos.FirstOrDefault(p => p.IdPermiso == id);
        }


    }
}
