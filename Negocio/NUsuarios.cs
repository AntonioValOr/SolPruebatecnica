using Datos;
using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NUsuarios
    {
        #region[Base de datos]
        DatUsuarios dat = new DatUsuarios();

        public List<Usuarios> Obtener()
        {
            List<Usuarios> lista = dat.Obtener();
            return lista;
        }

        public Usuarios ObtenerPorId(int id)
        {
            Usuarios p = dat.ObtenerPorId(id);
            return p;
        }
        public void agregar(Usuarios u)
        {
            dat.Agregar(u);
        }
        public void Editar(Usuarios u)
        {
            dat.Editar(u);
        }
        public void EliminarUsuarioConRegistros(int idUsuario)
        {
            dat.EliminarUsuarioConRegistros(idUsuario);
        }




        public Usuarios IniciarSesion(Usuarios usuario)
        {
            this.ValidarDatosLogin(usuario);
            this.ValidarDatosInicioSesion(usuario);
            Usuarios us = dat.IniciarSesion(usuario);

            return us;
        }


       

        public void RegistrarUsuario(Usuarios nuevoUsuario)
        {
            this.ValidarCamposVacios(nuevoUsuario);
            dat.RegistrarUsuario(nuevoUsuario);

        }
        #endregion

        #region[Validaciones]
        private void ValidarCamposVacios(Usuarios p)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(p.Usuario))
            {
                error += "Nombre es campo obligatorio<br/>";
            }
           
            if (string.IsNullOrEmpty(p.Password))
            {
                error += "Por favor ingresa una contraseña<br/>";
            }

            if (error != string.Empty)
            {
                throw new Exception(error);
            }

        }

        private void ValidarDatosInicioSesion(Usuarios L)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(L.Usuario))
            {
                error += "El Usuario no puede ir vacio <br/>";
            }
            if (string.IsNullOrEmpty(L.Password))
            {
                error += "Por favor ingresa una contraseña<br/>";
            }
        }
        private void ValidarDatosLogin(Usuarios usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Usuario) || string.IsNullOrEmpty(usuario.Password))
            {
                throw new Exception("Por favor, rellena los datos.");
            }
        }


        #endregion
    }
}
