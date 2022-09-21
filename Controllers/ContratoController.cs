using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {
    public class ContratoController : Controller {

        private IRepositorioContrato rc;
        private IRepositorioInmueble rInm;
        private IRepositorioInquilino rInq;

        public ContratoController(IRepositorioContrato rc, IRepositorioInmueble rInm, IRepositorioInquilino rInq) {
            this.rc = rc;
            this.rInm = rInm;
            this.rInq = rInq;
        }

        // GET: Contrato
        [Authorize]
        public ActionResult Index() {
            var lista = rc.ObtenerTodos();
            return View(lista);
        }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = rc.ObtenerPorId(id);
            ViewBag.Inmueble = rInm.ObtenerPorId(id);
            ViewBag.Inquilino = rInq.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Contrato/Create
        [Authorize]
        public ActionResult Create() {
            ViewBag.Inmueble = rInm.ObtenerTodos();
            ViewBag.Inquilino = rInq.ObtenerTodos();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int id) {
            var resultado = rc.ObtenerPorId(id);
            ViewBag.Inmueble = rInm.ObtenerTodos();
            ViewBag.Inquilino = rInq.ObtenerTodos();
            return View(resultado);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = rc.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato c) {
            try {
                rc.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Contrato/Terminate/5
        [Authorize]
        public ActionResult Terminate(int id) {
            return RedirectToAction(nameof(Index));
        }

        // POST: Contrato/Terminate/5
        public ActionResult Terminate(int id, Contrato c) {
            return RedirectToAction(nameof(Index));
        }
    }
}