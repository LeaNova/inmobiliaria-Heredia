using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {

    public class InmuebleController : Controller {

        private IRepositorioInmueble ri;
        private IRepositorioPropietario rp;

        public InmuebleController(IRepositorioInmueble ri, IRepositorioPropietario rp) {
            this.ri = ri;
            this.rp = rp;
        }

        // GET: Inmueble
        [Authorize]
        public ActionResult Index() {
            var lista = ri.ObtenerTodos();
            return View(lista);
        }

        // GET: Inmueble/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = ri.ObtenerPorId(id);
            ViewBag.Propietario = rp.ObtenerPorId(resultado.propietarioId);
            return View(resultado);
        }

        // GET: Inmueble/Create
        [Authorize]
        public ActionResult Create() {
            ViewBag.Propietario = rp.ObtenerTodos();
            ViewBag.Uso = Inmueble.ObtenerUsos();
            ViewBag.Tipo = Inmueble.ObtenerTipos();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble i) {
            try {
                if(!ModelState.IsValid) {
                    ri.Alta(i);
                    return RedirectToAction(nameof(Index));
                } else {
                    ViewBag.Propietario = rp.ObtenerTodos();
                    ViewBag.Uso = Inmueble.ObtenerUsos();
                    ViewBag.Tipo = Inmueble.ObtenerTipos();
                    return View(i);
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Inmueble/Edit/5
        [Authorize]
        public ActionResult Edit(int id) {
            var resultado = ri.ObtenerPorId(id);
            ViewBag.Propietario = rp.ObtenerTodos();
            ViewBag.Uso = Inmueble.ObtenerUsos();
            ViewBag.Tipo = Inmueble.ObtenerTipos();
            return View(resultado);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble i) {
            try {
                i.idInmueble = id;
                ri.Modificar(i);

                return RedirectToAction(nameof(Index));
            } catch {
                ViewBag.Propietario = rp.ObtenerTodos();
                ViewBag.Uso = Inmueble.ObtenerUsos();
                ViewBag.Tipo = Inmueble.ObtenerTipos();
                return View(i);
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble i) {
            try {
                ri.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }
    }
}