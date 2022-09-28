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
            ViewBag.Propietario = rp.ObtenerTodos();

            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }
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
                if(ModelState.IsValid) {
                    ri.Alta(i);
                    TempData["Mensaje"] = "Inmueble cargado correctamente";
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

                TempData["Mensaje"] = "Inmueble actualizado correctamente";
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
            ViewBag.Propietario = rp.ObtenerPorId(resultado.propietarioId);
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
                
                TempData["Error"] = "No se puede borrar un inmueble que participa de un contrato en lista";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmueble-disponibles
        [Authorize]
        public ActionResult Disponibles() {
            ViewBag.Propietario = rp.ObtenerTodos();

            var lista = ri.ObtenerTodos();

            IList<Inmueble> listaD = new List<Inmueble>();
            foreach(var item in lista) {
                if(item.disponible) {
                    listaD.Add(item);
                }
            }
            
            return View("Index", listaD);
        }

        // GET: Inmueble-disponibles
        [Authorize]
        public ActionResult BuscarPropietario(Inmueble i) {
            ViewBag.Propietario = rp.ObtenerTodos();

            var resultado = rp.ObtenerPorId(i.propietarioId);
            var lista = ri.ObtenerPorPropietario(resultado);

            return View("Index", lista);
        }
    }
}