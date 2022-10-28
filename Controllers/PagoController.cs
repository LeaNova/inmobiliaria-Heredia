using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmueble_Heredia.Controllers {

    public class PagoController : Controller {
        
        private IRepositorioPago rp;
        private IRepositorioContrato rc;

        public PagoController(IRepositorioPago rp, IRepositorioContrato rc) {
            this.rp = rp;
            this.rc = rc;
        }

        // GET: Pago
        [Authorize]
        public ActionResult Index(int id) {
            if(id > 0) {
                var resultado = rp.ObtenerPorContrato(id);
                ViewBag.Contrato = rc.ObtenerPorId(id);
                return View(resultado);
            }
            var lista = rp.ObtenerTodos();
            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: Pago/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Pago/Create
        [Authorize]
        public ActionResult Create(int id) {
            Contrato c = rc.ObtenerPorId(id);
            Pago p = new Pago();
            p.fechaPago = DateTime.Now;
            p.importe = c.alquilerMensual;
            p.contratoId = c.idContrato;
            ViewBag.contrato = rc.ObtenerTodos();
            return View(p);
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago p) {
            try {
                if(ModelState.IsValid) {
                    rp.Alta(p);
                    TempData["Mensaje"] = "Pago cargado correctamente";
                    return RedirectToAction(nameof(Index));
                } else {
                    return View(p);
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult Edit(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago p) {
            try {
                p.numPago = id;
                rp.Modificar(p);

                TempData["Mensaje"] = "Pago actualizado correctamente";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                rp.Baja(id);
                TempData["Mensaje"] = "Pago eliminado correctamente";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }
    }
}