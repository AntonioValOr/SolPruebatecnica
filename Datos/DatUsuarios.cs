using Datos.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DatUsuarios
    {
        PruebaTecnicaEntities db = new PruebaTecnicaEntities();

        public List<Usuarios> Obtener()
        {
            List<Usuarios> ls = db.Usuarios.ToList();
            return ls;
        }

        public Usuarios ObtenerPorId(int id)
        {
            Usuarios u = db.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();
            return u;
        }


        public void Agregar(Usuarios u)
        {
            db.Usuarios.Add(u);
            db.SaveChanges();
            db.Dispose();
        }

        public void Editar(Usuarios m)
        {
            db.Usuarios.AddOrUpdate(m);
            db.SaveChanges();
            db.Dispose();
        }
        public void EliminarUsuarioConRegistros(int idUsuario)
        {

            var usuario = ObtenerPorId(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            var permisos = db.UsuarioPermisos.Where(up => up.IdUsuario == idUsuario).ToList();
            if (permisos.Any())
            {
                db.UsuarioPermisos.RemoveRange(permisos);
            }


            var registros = db.Registros.Where(r => r.IdUsuario == idUsuario).ToList();
            if (registros.Any())
            {
                db.Registros.RemoveRange(registros);
            }


            db.Usuarios.Remove(usuario);

            db.SaveChanges();
        }



        public void RegistrarUsuario(Usuarios nuevoUsuario)
        {
            bool existe = db.Usuarios.Any(x => x.Usuario == nuevoUsuario.Usuario);
            if (existe)
                throw new Exception("El Usuario ya está registrado.");

            db.Usuarios.Add(nuevoUsuario);
            db.SaveChanges();
        }

        public Usuarios IniciarSesion(Usuarios usuario)
        {
            var us = db.Usuarios
                       .FirstOrDefault(u => u.Usuario == usuario.Usuario && u.Password == usuario.Password);

            if (us == null)
                throw new Exception("Nombre de usuario o contraseña incorrectos.");

            return us;
        }

        public List<UsuarioPermisos> ObtenerPermisosPorUsuario(int idUsuario)
        {
            return db.UsuarioPermisos.Where(up => up.IdUsuario == idUsuario).ToList();
        }

        public void EliminarPermiso(int idPermisoUsuario)
        {
            var permiso = db.UsuarioPermisos.FirstOrDefault(up => up.IdPermiso == idPermisoUsuario);
            if (permiso != null)
            {
                db.UsuarioPermisos.Remove(permiso);
                db.SaveChanges();
            }
        }

        public List<Registros> ObtenerRegistrosPorUsuario(int idUsuario)
        {
            return db.Registros.Where(r => r.IdUsuario == idUsuario).ToList();
        }

        public void EliminarRegistro(int idRegistro)
        {
            var reg = db.Registros.FirstOrDefault(r => r.IdRegistro == idRegistro);
            if (reg != null)
            {
                db.Registros.Remove(reg);
                db.SaveChanges();
            }
        }
    }
}
