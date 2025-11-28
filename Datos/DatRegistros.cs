using Datos.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos
{
    public class DatRegistros
    {
        private readonly PruebaTecnicaEntities db = new PruebaTecnicaEntities();

        public List<Registros> ObtenerTodos()
        {
            return db.Registros.ToList();
        }
        public List<Registros> ObtenerPorUsuario(int usuarioId)
        {
            return db.Registros.Where(c => c.IdUsuario == usuarioId).ToList();
        }

        public Registros ObtenerPorId(int id)
        {
            return db.Registros.FirstOrDefault(r => r.IdRegistro == id);
        }

        public void Agregar(Registros r)
        {
            if (r == null)
                throw new Exception("Registro no proporcionado.");

            db.Registros.Add(r);
            db.SaveChanges();
        }

        public void Actualizar(Registros r)
        {
            var existente = db.Registros.FirstOrDefault(x => x.IdRegistro == r.IdRegistro);
            if (existente == null)
                throw new Exception("Registro no existe.");

            existente.IdUsuario = r.IdUsuario;
            existente.Nombre = r.Nombre;
            existente.Contrato = r.Contrato;
            existente.Saldos = r.Saldos;
            existente.Fecha = r.Fecha;
            existente.Telefono = r.Telefono;

            db.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var r = db.Registros.FirstOrDefault(x => x.IdRegistro == id);
            if (r == null)
                throw new Exception("Registro no existe.");

            db.Registros.Remove(r);
            db.SaveChanges();
        }
    }
}
