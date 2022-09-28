using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {

    public class PropietarioController : Controller {

        private IConfiguration configuration;
        private IRepositorioPropietario rp;

        public PropietarioController(IConfiguration configuration, IRepositorioPropietario rp) {
            this.configuration = configuration;
            this.rp = rp;
        }

        // GET: Propietario
        [Authorize]
        public ActionResult Index() {
            var lista = rp.ObtenerTodos();
            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: Propietario/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Propietario/Create
        [Authorize]
        public ActionResult Create() {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario p) {
            try {
                if(ModelState.IsValid) {
                    if(validarDatos(p)) {
                        rp.Alta(p);
                        TempData["Mensaje"] = "Propietario cargado correctamente";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = "Error en entrada de datos";
                    if(TempData.ContainsKey("Error")) {
                        ViewBag.Error = TempData["Error"];
                    }
                    return View(p);
                } else {
                    return View(p);
                }
            } catch (Exception ex) {
                
                TempData["Error"] = "No se pudo cargar Propietario, DNI o direccion mail duplicada";
                return View(p);
            }
        }

        // GET: Propietario/Edit/5
        [Authorize]
        public ActionResult Edit(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario p) {
            try {
                if(validarDatos(p)) {
                    p.idPropietario = id;
                    rp.Modificar(p);

                    TempData["Mensaje"] = "Propietario actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error en cargar datos";
                if(TempData.ContainsKey("Error")) {
                    ViewBag.Error = TempData["Error"];
                }
                return View(p);
            } catch (Exception ex) {
                
                TempData["Error"] = "No se pudo cargar Propietario, DNI o direccion mail duplicada";
                return View(p);
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario p) {
            try {
                rp.Baja(id);
                
                TempData["Mensaje"] = "Propietario eliminado correctamente";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                
                TempData["Error"] = "No se puede borrar un propietario con un inmueble en lista";
                return RedirectToAction(nameof(Index));
            }
        }

        public bool validarDatos(Propietario p) {
            if(p.nombre.Any(char.IsDigit)) {
                return false;
            }
            if(p.apellido.Any(char.IsDigit)) {
                return false;
            }
            if(p.DNI.Any(char.IsLetter)) {
                return false;
            }
            if(p.telefono.Any(char.IsLetter)) {
                return false;
            }
            return true;
        }
    }
}