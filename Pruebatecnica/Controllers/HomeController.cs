using Datos.ClasEx;
using Datos.Model;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pruebatecnica.Controllers
{
    public class HomeController : Controller
    {
        NUsuarios bus = new NUsuarios();
        NRolPermisos nRolPermisos = new NRolPermisos();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult LoginPost(string Usuario, string Password)
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                TempData["error"] = "Por favor, rellena los datos.";
                return View("Index");
            }

            try
            {
                Usuarios us = bus.IniciarSesion(new Usuarios
                {
                    Usuario = Usuario,
                    Password = Password
                });

                if (us == null)
                {
                    TempData["error"] = "Usuario o contraseña incorrectos.";
                    return View("Index");
                }

                if (!us.IdRol.HasValue)
                {
                    TempData["error"] = "El usuario no tiene rol asignado.";
                    return View("Index");
                }

                switch (us.Estatus)
                {
                    case 1:
                        break;
                    case 2:
                        TempData["error"] = "Debe contactar al administrador para cambiar su contraseña.";
                        return RedirectToAction("Index");
                    case 3:
                        TempData["error"] = "Su cuenta está dada de baja, no puede ingresar.";
                        return RedirectToAction("Index");
                    default:
                        TempData["error"] = "Estatus de usuario no válido.";
                        return RedirectToAction("Index");
                }

                var horaActual = DateTime.Now.TimeOfDay;
                if (horaActual < us.HorarioInicio || horaActual > us.HorarioFin)
                {
                    TempData["error"] = $"No puedes ingresar fuera de tu horario ({us.HorarioInicio:hh\\:mm} - {us.HorarioFin:hh\\:mm}).";
                    return RedirectToAction("Index");
                }

                Session["Usuario"] = us;
                Session["UsuarioNombre"] = us.Usuario;
                Session["IdRol"] = us.IdRol.Value;
                Session["Permisos"] = nRolPermisos.ObtenerPorIdRol(us.IdRol.Value)
                                                  .Select(rp => rp.IdPermiso)
                                                  .ToList();

                if (us.IdRol.Value == 1)
                    return RedirectToAction("VentanaInicio", "Usuarios");
                else
                    return RedirectToAction("InicioUsuario", "Registros");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Index");
            }
        }







        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


    }
}
