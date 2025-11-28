using System.Collections.Generic;

namespace Datos.ClasEx
{
    public class UsuariosEx
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string HorarioInicioStr { get; set; } 
        public string HorarioFinStr { get; set; }    
        public int? Estatus { get; set; }
        public string Rol { get; set; }
        public List<int> PermisosSeleccionados { get; set; } = new List<int>();
        public string EstatusTexto
        {
            get
            {
                switch (Estatus)
                {
                    case 1: return "Activo";
                    case 2: return "Cambiar Contraseña";
                    case 3: return "Baja";
                    default: return "Desconocido";
                }
            }
        
        }

    }
}
