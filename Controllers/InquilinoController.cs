using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {
    public class InquilinoController : Controller {
        
        private IRepositorioInquilino ri;

        public InquilinoController(IRepositorioInquilino ri) {
            this.ri = ri;
        }

        // GET: Inquilino
        [Authorize]
        public ActionResult Index() {
            var lista = ri.ObtenerTodos();
            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Inquilino/Create
        [Authorize]
        public ActionResult Create() {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino i) {
            try {
                if(ModelState.IsValid) {
                    if(validarDatos(i)) {
                        ri.Alta(i);
                        TempData["Mensaje"] = "Inquilino cargado correctamente";
                        return RedirectToAction(nameof(Index)); 
                    }

                    TempData["Error"] = "Error en entrada de datos";
                    if(TempData.ContainsKey("Error")) {
                        ViewBag.Error = TempData["Error"];
                    }
                    return View(i);
                } else {
                    return View(i);
                }
            } catch (Exception ex) {
                
                TempData["Error"] = "No se pudo cargar Inquilino, DNI o direccion mail duplicada";
                return View(i);
            }
        }

        // GET: Inquilino/Edit/5
        [Authorize]
        public ActionResult Edit(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino i) {
            try {
                if(validarDatos(i)) {
                    i.idInquilino = id;
                    ri.Modificar(i);

                    TempData["Mensaje"] = "Inquilino actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error en cargar datos";
                if(TempData.ContainsKey("Error")) {
                    ViewBag.Error = TempData["Error"];
                }
                return View(i);
            } catch (Exception ex) {
                
                TempData["Error"] = "No se pudo cargar Inquilino, DNI o direccion mail duplicada";
                return View(i);
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino i) {
            try {
                ri.Baja(id);

                TempData["Mensaje"] = "Inquilino eliminado correctamente";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                
                TempData["Error"] = "No se puede borrar un inquilino que participa de un contrato en lista";
                return RedirectToAction(nameof(Index));
            }
        }

        public bool validarDatos(Inquilino i) {
            if(i.nombre.Any(char.IsDigit)) {
                return false;
            }
            if(i.apellido.Any(char.IsDigit)) {
                return false;
            }
            if(i.DNI.Any(char.IsLetter)) {
                return false;
            }
            if(i.telefono.Any(char.IsLetter)) {
                return false;
            }
            return true;
        }
    }
}