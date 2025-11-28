using Datos;
using System.Collections.Generic;

namespace Negocio
{
    public class NUsuarioPermisos
    {
        private DatUsuarioPermisos dat = new DatUsuarioPermisos();


        public void AsignarPermiso(int idUsuario, int idPermiso)
        {
            dat.AsignarPermiso(idUsuario, idPermiso);
        }


        public List<int> ObtenerPermisosPorUsuario(int idUsuario)
        {
            return dat.ObtenerPermisosPorUsuario(idUsuario);
        }


        public void EliminarPermisosUsuario(int idUsuario)
        {
            dat.EliminarPermisosUsuario(idUsuario);
        }

        public void ActualizarPermisos(int idUsuario, List<int> permisos)
        {
            dat.EliminarPermisosUsuario(idUsuario);

            if (permisos != null)
            {
                foreach (var idPermiso in permisos)
                {
                    dat.AsignarPermiso(idUsuario, idPermiso);
                }
            }
        }
    }
}
