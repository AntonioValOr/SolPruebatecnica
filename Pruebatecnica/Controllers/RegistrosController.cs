using Datos.ClasEx;
using Datos.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetZip = Ionic.Zip.ZipFile;

namespace Pruebatecnica.Controllers
{
    public class RegistrosController : Controller
    {
        NRegistros nRegistros = new NRegistros();
        NConfiguracion nConfig = new NConfiguracion();
        NUsuarios nUsuarios = new NUsuarios();

        // ==================== LISTAR ====================
            public ActionResult InicioUsuario()
        {
            var usuarioSesion = Session["Usuario"] as Usuarios;
            if (usuarioSesion == null)
            {
                TempData["error"] = "Debe iniciar sesión primero.";
                return RedirectToAction("Index", "Home");
            }

            int usuarioId = usuarioSesion.IdUsuario;
            var registros = nRegistros.ObtenerPorUsuario(usuarioId)?.ToList() ?? new List<Registros>();

            ViewBag.TiposHoja = nConfig.ObtenerTamanosHoja()?.ToList() ?? new List<TamanoHoja>();
            ViewBag.TiposLetra = nConfig.ObtenerTiposLetra()?.ToList() ?? new List<TipoLetra>();
            ViewBag.TamanosLetra = nConfig.ObtenerTamanosLetra()?.ToList() ?? new List<TamanoLetra>();

            return View(registros);
        }
        // ==================== CREAR REGISTRO ====================
        public ActionResult CrearRegistro()
        {

            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2)) 
            {
                TempData["error"] = "No tienes permiso para crear registros.";
                return RedirectToAction("Index", "Home");
            }


            return View(new Registros());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearRegistroPost(Registros registro)
        {

            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2))
            {
                TempData["error"] = "No tienes permiso para crear registros.";
                return RedirectToAction("Index", "Home");
            }

            var usuarioSesion = Session["Usuario"] as Usuarios;
            if (usuarioSesion == null)
            {
                TempData["error"] = "No se pudo identificar al usuario.";
                return View("CrearRegistro", registro);
            }

            registro.IdUsuario = usuarioSesion.IdUsuario;


            if (!ModelState.IsValid)
                return View("CrearRegistro", registro);

            try
            {
                nRegistros.Agregar(registro);
                TempData["m"] = "Registro creado correctamente.";
                return RedirectToAction("InicioUsuario");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("CrearRegistro", registro);
            }
        }
        // ==================== EDITAR REGISTRO ====================
        public ActionResult EditarRegistro(int id)
        {
            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2))
            {
                TempData["error"] = "No tienes permiso para editar registros.";
                return RedirectToAction("InicioUsuario");
            }


            var registro = nRegistros.ObtenerPorId(id);
            if (registro == null) return HttpNotFound();

            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRegistroPost(Registros registro)
        {

            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2))
            {
                TempData["error"] = "No tienes permiso para actualizar registros.";
                return RedirectToAction("InicioUsuario");
            }


            if (!ModelState.IsValid)
                return View(registro);

            try
            {

                nRegistros.Actualizar(registro);
                TempData["m"] = "Registro actualizado correctamente.";
                return RedirectToAction("InicioUsuario");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View(registro);
            }
        }

        // ==================== ELIMINAR REGISTRO ====================
        public ActionResult EliminarRegistro(int id)
        {
            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2))
            {
                TempData["error"] = "No tienes permiso para eliminar registros.";
                return RedirectToAction("InicioUsuario");
            }

            var registro = nRegistros.ObtenerPorId(id);
            if (registro == null) return HttpNotFound();

            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRegistroPost(int id)
        {

            var permisos = Session["Permisos"] as List<int> ?? new List<int>();
            if (!permisos.Contains(2))
            {
                TempData["error"] = "No tienes permiso para eliminar registros.";
                return RedirectToAction("InicioUsuario");
            }

            try
            {
                nRegistros.Eliminar(id);
                TempData["m"] = "Registro eliminado correctamente.";
                return RedirectToAction("InicioUsuario");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("InicioUsuario");
            }
        }
        // ==================== REGISTROS DE CADA USUARIO ====================
        public ActionResult VerRegistrosUsuario(int idUsuario)
        {
            var permisos = Session["Permisos"] as List<int> ?? new List<int>();

            if (!permisos.Contains(3))
            {
                TempData["error"] = "No tienes permiso para ver los registros.";
                return RedirectToAction("Index", "Home");
            }

            var usuario = nUsuarios.ObtenerPorId(idUsuario);
            if (usuario == null)
                return HttpNotFound();

            var registros = usuario.Registros.ToList();
            return View(registros);
        }
        // ==================== GENERADOR DE PDF'S ====================
        public ActionResult GenerarPDF()
        {
            var usuarioSesion = Session["Usuario"] as Usuarios;
            if (usuarioSesion == null)
            {
                TempData["error"] = "Debe iniciar sesión primero.";
                return RedirectToAction("Index", "Home");
            }

            int usuarioId = usuarioSesion.IdUsuario;

            var registros = nRegistros.ObtenerPorUsuario(usuarioId)?.ToList() ?? new List<Registros>();
            if (registros.Count == 0)
            {
                TempData["error"] = "No tienes registros para generar PDF.";
                return RedirectToAction("InicioUsuario");
            }


            ViewBag.TiposHoja = nConfig.ObtenerTamanosHoja()?.ToList() ?? new List<TamanoHoja>();
            ViewBag.TiposLetra = nConfig.ObtenerTiposLetra()?.ToList() ?? new List<TipoLetra>();
            ViewBag.TamanosLetra = nConfig.ObtenerTamanosLetra()?.ToList() ?? new List<TamanoLetra>();

            return View("SeleccionarRegistros", registros);
        }



        public ActionResult SeleccionarRegistros()
        {
            var usuarioSesion = Session["Usuario"] as Usuarios;
            if (usuarioSesion == null)
            {
                TempData["error"] = "Debe iniciar sesión primero.";
                return RedirectToAction("Index", "Home");
            }

            int usuarioId = usuarioSesion.IdUsuario;


            var registros = nRegistros.ObtenerPorUsuario(usuarioId)?.ToList() ?? new List<Registros>();


            ViewBag.TiposHoja = nConfig.ObtenerTamanosHoja()?.ToList() ?? new List<TamanoHoja>();
            ViewBag.TiposLetra = nConfig.ObtenerTiposLetra()?.ToList() ?? new List<TipoLetra>();
            ViewBag.TamanosLetra = nConfig.ObtenerTamanosLetra()?.ToList() ?? new List<TamanoLetra>();

            return View(registros); 
        }

        public class WatermarkHelper : PdfPageEventHelper
        {
            private readonly iTextSharp.text.Image _marcaAgua;

            public WatermarkHelper(iTextSharp.text.Image marcaAgua)
            {
                _marcaAgua = marcaAgua;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                if (_marcaAgua != null)
                {
                    float x = (document.PageSize.Width - _marcaAgua.ScaledWidth) / 2;
                    float y = (document.PageSize.Height - _marcaAgua.ScaledHeight) / 2;
                    _marcaAgua.SetAbsolutePosition(x, y);

                    PdfContentByte under = writer.DirectContentUnder;
                    under.AddImage(_marcaAgua);
                }
            }
        }

        [HttpPost]
        public ActionResult GenerarPDFSeleccionados(int[] registrosSeleccionados, string tipoHoja, string tipoLetra, int tamanoLetra, HttpPostedFileBase marcaAgua)
        {

            var usuarioSesion = Session["Usuario"] as Usuarios;
            if (usuarioSesion == null)
            {
                TempData["error"] = "Debe iniciar sesión primero.";
                return RedirectToAction("Index", "Home");
            }

            if (registrosSeleccionados == null || registrosSeleccionados.Length == 0)
            {
                TempData["error"] = "Debe seleccionar al menos un registro.";
                return RedirectToAction("SeleccionarRegistros");
            }


            tipoHoja = string.IsNullOrEmpty(tipoHoja) ? nConfig.ObtenerTamanosHoja().FirstOrDefault()?.Nombre ?? "A4" : tipoHoja;
            tipoLetra = string.IsNullOrEmpty(tipoLetra) ? nConfig.ObtenerTiposLetra().FirstOrDefault()?.Nombre ?? "Arial" : tipoLetra;
            tamanoLetra = tamanoLetra == 0 ? nConfig.ObtenerTamanosLetra().FirstOrDefault()?.Valor ?? 12 : tamanoLetra;

            iTextSharp.text.Image imgMarcaAgua = null;
            if (marcaAgua != null && marcaAgua.ContentLength > 0)
            {
                imgMarcaAgua = iTextSharp.text.Image.GetInstance(marcaAgua.InputStream);
                imgMarcaAgua.ScaleToFit(300f, 300f); 
            }

            Rectangle pageSize;
            switch (tipoHoja)
            {
                case "Carta": pageSize = PageSize.LETTER; break;
                case "A3": pageSize = PageSize.A3; break;
                case "Oficio": pageSize = PageSize.LEGAL; break;
                case "A4":
                default: pageSize = PageSize.A4; break;
            }
            string tipoLetraSwitch;
            switch (tipoLetra)
            {
                case "Times New Roman": tipoLetraSwitch = "Times-Roman"; break;
                case "Courier": tipoLetraSwitch = "Courier"; break;
                case "Calibri": tipoLetraSwitch = "Helvetica"; break;
                case "Georgia": tipoLetraSwitch = "Times-Roman"; break;
                case "Arial":
                default: tipoLetraSwitch = "Arial"; break;
            }
            int tamanoLetraSwitch;
            switch (tamanoLetra)
            {
                case 8: tamanoLetraSwitch = 8; break;
                case 10: tamanoLetraSwitch = 10; break;
                case 12: tamanoLetraSwitch = 12; break;
                case 14: tamanoLetraSwitch = 14; break;
                case 16: tamanoLetraSwitch = 16; break;
                case 18: tamanoLetraSwitch = 18; break;
                default: tamanoLetraSwitch = 12; break;
            }

            if (registrosSeleccionados.Length == 1)
            {
                var reg = nRegistros.ObtenerPorId(registrosSeleccionados[0]);
                if (reg == null)
                {
                    TempData["error"] = "Registro no encontrado.";
                    return RedirectToAction("SeleccionarRegistros");
                }

                using (var ms = new MemoryStream())
                {
                    Document doc = new Document(pageSize);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                    if (imgMarcaAgua != null)
                        writer.PageEvent = new WatermarkHelper(imgMarcaAgua);

                    doc.Open();

                    var font = FontFactory.GetFont(tipoLetraSwitch, tamanoLetraSwitch);
                    doc.Add(new Paragraph($"Registro ID: {reg.IdRegistro}", font));
                    doc.Add(new Paragraph($"Nombre: {reg.Nombre}", font));
                    doc.Add(new Paragraph($"Contrato: {reg.Contrato}", font));
                    doc.Add(new Paragraph($"Saldos: {reg.Saldos}", font));
                    doc.Add(new Paragraph($"Teléfono: {reg.Telefono}", font));

                    doc.Close();
                    return File(ms.ToArray(), "application/pdf", $"Registro_{reg.IdRegistro}.pdf");
                }
            }
            else
            {
                using (var zipStream = new MemoryStream())
                {
                    using (var zip = new Ionic.Zip.ZipFile())
                    {
                        foreach (var id in registrosSeleccionados)
                        {
                            var reg = nRegistros.ObtenerPorId(id);
                            if (reg == null) continue;

                            using (var msPdf = new MemoryStream())
                            {
                                Document doc = new Document(pageSize);
                                PdfWriter writer = PdfWriter.GetInstance(doc, msPdf);

                                if (imgMarcaAgua != null)
                                    writer.PageEvent = new WatermarkHelper(imgMarcaAgua);

                                doc.Open();

                                var font = FontFactory.GetFont(tipoLetraSwitch, tamanoLetraSwitch);
                                doc.Add(new Paragraph($"Registro ID: {reg.IdRegistro}", font));
                                doc.Add(new Paragraph($"Nombre: {reg.Nombre}", font));
                                doc.Add(new Paragraph($"Contrato: {reg.Contrato}", font));
                                doc.Add(new Paragraph($"Saldos: {reg.Saldos}", font));
                                doc.Add(new Paragraph($"Teléfono: {reg.Telefono}", font));

                                doc.Close();
                                zip.AddEntry($"Registro_{reg.IdRegistro}.pdf", msPdf.ToArray());
                            }
                        }
                        zip.Save(zipStream);
                    }
                    zipStream.Position = 0;
                    return File(zipStream.ToArray(), "application/zip", "RegistrosSeleccionados.zip");
                }
            }
        }


    }
}
