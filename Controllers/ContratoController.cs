using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {
    public class ContratoController : Controller {

        private RepositorioContrato rc = new RepositorioContrato();
        private RepositorioInmueble rInm = new RepositorioInmueble();
        private RepositorioInquilino rInq = new RepositorioInquilino();

        // GET: Contrato
        public ActionResult Index() {
            var lista = rc.ObtenerTodos();
            return View(lista);
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int id) {
            var resultado = rc.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Contrato/Create
        public ActionResult Create() {
            ViewBag.Inmueble = rInm.ObtenerTodos();
            ViewBag.Inquilino = rInq.ObtenerTodos();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c) {
            try {
                if(!ModelState.IsValid) {
                    if(c.fechaFinal > c.fechaInicio) {
                        rc.Alta(c);
                        return RedirectToAction(nameof(Index));
                    } else {
                        ViewBag.Inmueble = rInm.ObtenerTodos();
                        ViewBag.Inquilino = rInq.ObtenerTodos();
                        return View(c);
                    }
                } else {
                    ViewBag.Inmueble = rInm.ObtenerTodos();
                    ViewBag.Inquilino = rInq.ObtenerTodos();
                    return View(c);
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Contrato/Edit/5
        public ActionResult Edit(int id) {
            var resultado = rc.ObtenerPorId(id);
            ViewBag.Inmueble = rInm.ObtenerTodos();
            ViewBag.Inquilino = rInq.ObtenerTodos();
            return View(resultado);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c) {
            try {
                c.idContrato = id;
                rc.Modificar(c);

                return RedirectToAction(nameof(Index));
            } catch {
                ViewBag.Inmueble = rInm.ObtenerTodos();
                ViewBag.Inquilino = rInq.ObtenerTodos();
                return View(c);
            }
        }

        // GET: Contrato/Delete/5
        public ActionResult Delete(int id) {
            var resultado = rc.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato c) {
            try {
                rc.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }
    }
}