using Datos.Model;
using System.Collections.Generic;
using System.Linq;

namespace Datos
{
    public class DatUsuarioPermisos
    {
        PruebaTecnicaEntities db = new PruebaTecnicaEntities();


        public void AsignarPermiso(int idUsuario, int idPermiso)
        {
            var existente = db.UsuarioPermisos
                              .FirstOrDefault(up => up.IdUsuario == idUsuario && up.IdPermiso == idPermiso);
            if (existente == null)
            {
                var nuevo = new UsuarioPermisos
                {
                    IdUsuario = idUsuario,
                    IdPermiso = idPermiso
                };
                db.UsuarioPermisos.Add(nuevo);
                db.SaveChanges();
            }
        }


        public List<int> ObtenerPermisosPorUsuario(int idUsuario)
        {
            return db.UsuarioPermisos
                     .Where(up => up.IdUsuario == idUsuario)
                     .Select(up => up.IdPermiso)
                     .ToList();
        }


        public void EliminarPermisosUsuario(int idUsuario)
        {
            var permisos = db.UsuarioPermisos.Where(up => up.IdUsuario == idUsuario).ToList();
            db.UsuarioPermisos.RemoveRange(permisos);
            db.SaveChanges();
        }
    }
}
