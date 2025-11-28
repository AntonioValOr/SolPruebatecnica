using Datos.ClasEx;
using Datos.Model;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pruebatecnica.Controllers
{
    public class UsuariosController : Controller
    {
        NUsuarios bus = new NUsuarios();
        NPermisos nPermisos = new NPermisos();
        NUsuarioPermisos nUsuarioPermisos = new NUsuarioPermisos();
        NRolPermisos nRolPermisos = new NRolPermisos();

        // ==================== LISTAR ====================
        public ActionResult VentanaInicio()
        {
            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(1))
            {
                TempData["error"] = "No tienes permiso para ver esta sección.";
                return RedirectToAction("Index", "Home");
            }

            var lista = bus.Obtener();
            return View(lista);
        }


        // ==================== CREAR ====================

        public ActionResult CrearUsuario()
        {
            var permisosSesion = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisosSesion.Contains(1))
            {
                TempData["error"] = "No tienes permiso para crear usuarios.";
                return RedirectToAction("Index", "Home");
            }


            var modelo = new UsuariosEx
            {
                Rol = "Usuario",  
                Estatus = 1        
            };

            ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos();

            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuarioPost(UsuariosEx usuarioEx, int[] PermisosSeleccionados)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos(); 
                return View(usuarioEx);
            }

            try
            {

                TimeSpan inicio = TimeSpan.Parse(usuarioEx.HorarioInicioStr);
                TimeSpan fin = TimeSpan.Parse(usuarioEx.HorarioFinStr);

                string rol = string.IsNullOrEmpty(usuarioEx.Rol) ? "Usuario" : usuarioEx.Rol;
                int idRol = rol == "Admin" ? 1 : 2;


                Usuarios usuario = new Usuarios
                {
                    Usuario = usuarioEx.Usuario,
                    Password = usuarioEx.Password,
                    HorarioInicio = inicio,
                    HorarioFin = fin,
                    Estatus = usuarioEx.Estatus,
                    IdRol = idRol,
                    Rol = rol
                };


                bus.agregar(usuario);


                if (PermisosSeleccionados != null)
                {
                    foreach (var idPermiso in PermisosSeleccionados)
                    {
                        nUsuarioPermisos.AsignarPermiso(usuario.IdUsuario, idPermiso);
                    }
                }

                TempData["m"] = "Usuario creado correctamente.";
                return RedirectToAction("VentanaInicio");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos();
                return View(usuarioEx);
            }
        }





        // ==================== EDITAR ====================
        public ActionResult EditarUsuario(int id)
        {
            var permisosSesion = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisosSesion.Contains(1))
            {
                TempData["error"] = "No tienes permiso para editar usuarios.";
                return RedirectToAction("Index", "Home");
            }

            var usuario = bus.ObtenerPorId(id);
            if (usuario == null) return HttpNotFound();

            var modelo = new UsuariosEx
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                HorarioInicioStr = usuario.HorarioInicio.ToString(@"hh\:mm"),
                HorarioFinStr = usuario.HorarioFin.ToString(@"hh\:mm"),
                Estatus = usuario.Estatus,
                Rol = usuario.IdRol == 1 ? "Admin" :
                      usuario.IdRol == 2 ? "Usuario" : ""
            };

            ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos();
            ViewBag.PermisosSeleccionados = nUsuarioPermisos.ObtenerPermisosPorUsuario(id);

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuarioPost(UsuariosEx usuarioEx, int[] PermisosSeleccionados)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos();
                ViewBag.PermisosSeleccionados = nUsuarioPermisos.ObtenerPermisosPorUsuario(usuarioEx.IdUsuario);
                return View(usuarioEx);
            }

            try
            {
                TimeSpan inicio = TimeSpan.Parse(usuarioEx.HorarioInicioStr);
                TimeSpan fin = TimeSpan.Parse(usuarioEx.HorarioFinStr);

                int? idRol = usuarioEx.Rol == "Admin" ? 1 :
                              usuarioEx.Rol == "Usuario" ? 2 : (int?)null;

                Usuarios usuario = new Usuarios
                {
                    IdUsuario = usuarioEx.IdUsuario,
                    Usuario = usuarioEx.Usuario,
                    Password = usuarioEx.Password,
                    HorarioInicio = inicio,
                    HorarioFin = fin,
                    Estatus = usuarioEx.Estatus,
                    IdRol = idRol
                };

                bus.Editar(usuario);

                nUsuarioPermisos.ActualizarPermisos(usuario.IdUsuario, PermisosSeleccionados?.ToList());

                TempData["m"] = "Usuario actualizado correctamente.";
                return RedirectToAction("VentanaInicio");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                ViewBag.TodosLosPermisos = nPermisos.ObtenerTodos();
                ViewBag.PermisosSeleccionados = nUsuarioPermisos.ObtenerPermisosPorUsuario(usuarioEx.IdUsuario);
                return View(usuarioEx);
            }
        }
        // ==================== ELIMINAR ====================
        public ActionResult EliminarUsuario(int id)
        {

            var permisosSesion = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisosSesion.Contains(1))
            {
                TempData["error"] = "No tienes permiso para eliminar usuarios.";
                return RedirectToAction("VentanaInicio");
            }

            var usuario = bus.ObtenerPorId(id);
            if (usuario == null) return HttpNotFound();

            var usuarioEx = new UsuariosEx
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                Estatus = usuario.Estatus,
                HorarioInicioStr = usuario.HorarioInicio.ToString(@"hh\:mm"),
                HorarioFinStr = usuario.HorarioFin.ToString(@"hh\:mm"),
                Rol = usuario.Rol != null ? usuario.IdRol.HasValue.ToString() : "",
            };

            return View(usuarioEx);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuarioPost(int id)
        {
            try
            {
                bus.EliminarUsuarioConRegistros(id);
                TempData["m"] = "Usuario eliminado correctamente.";
                return RedirectToAction("VentanaInicio");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al eliminar usuario: " + ex.Message;
                return RedirectToAction("VentanaInicio");
            }
        }

        // ==================== SECCION DE CONTRASEÑAS ====================
        public ActionResult CambiarContrasena(int id)
        {
            var usuario = bus.ObtenerPorId(id);
            if (usuario == null)
            {
                TempData["error"] = "Usuario no encontrado.";
                return RedirectToAction("VentanaInicio", "Usuarios");
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(int IdUsuario, string PasswordNueva, string ConfirmarPassword)
        {
            var usuario = bus.ObtenerPorId(IdUsuario);
            if (usuario == null)
            {
                TempData["error"] = "Usuario no encontrado.";
                return RedirectToAction("VentanaInicio");
            }

            if (string.IsNullOrWhiteSpace(PasswordNueva) || string.IsNullOrWhiteSpace(ConfirmarPassword))
            {
                TempData["error"] = "Debe completar ambos campos de contraseña.";
                return View(usuario);
            }

            if (PasswordNueva != ConfirmarPassword)
            {
                TempData["error"] = "Las contraseñas no coinciden.";
                return View(usuario);
            }

            try
            {
                usuario.Password = PasswordNueva;
                usuario.Estatus = 1;
                bus.Editar(usuario);

                TempData["m"] = "Contraseña actualizada correctamente.";
                return RedirectToAction("VentanaInicio");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al actualizar contraseña: " + ex.Message;
                return View(usuario);
            }
        }


    }
}
