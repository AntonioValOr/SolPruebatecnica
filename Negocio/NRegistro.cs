using Datos;
using Datos.Model;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class NRegistros
    {
        DatRegistros dat = new DatRegistros();
       
        public List<Registros> ObtenerTodos()
        {
            return dat.ObtenerTodos();
        }
        public List<Registros> ObtenerPorUsuario(int usuarioId)
        {
            var listacontactos = dat.ObtenerPorUsuario(usuarioId);
            
            return listacontactos;
        }

        public Registros ObtenerPorId(int id)
        {
            return dat.ObtenerPorId(id);
        }

        public void Agregar(Registros r)
        {
            this.ValidarCamposVacios(r);
            dat.Agregar(r);
        }

        public void Actualizar(Registros r)
        {
            dat.Actualizar(r);
        }

        public void Eliminar(int id)
        {
            dat.Eliminar(id);
        }

        #region[Validaciones]

        private void ValidarCamposVacios(Registros p)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(p.Nombre))
            {
                error += "Nombre es campo obligatorio<br/>";
            }
            if (string.IsNullOrEmpty(p.Contrato))
            {
                error += "selecciona una opcion de contrato<br/>";

            }
            if (p.Saldos == 0)
            {
                error += "Por favor ingresa un saldo<br/>";
            }
            if (error != string.Empty)
            {
                throw new Exception(error);
            }

        }
        #endregion
    }
}
