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
        [AllowAnonymous]
        public ActionResult Index(int id) {
            if(id == 0) {
                var lista = rp.ObtenerTodos();
                return View(lista);
            } else {
                var lista = rp.ObtenerPorContrato(id);
                return View(lista);
            }
        }

        // GET: Pago/Details/5
        public ActionResult Details(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Pago/Create
        public ActionResult Create(int id) {
            Contrato c = rc.ObtenerPorId(id);
            Pago p = new Pago();
            p.fechaPago = DateTime.Now;
            p.importe = c.alquilerMensual;
            p.contratoId = c.idContrato;
            return View(p);
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p) {
            try {
                if(!ModelState.IsValid) {
                    rp.Alta(p);
                    return RedirectToAction(nameof(Index));
                } else {
                    return View(p);
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p) {
            try {
                p.numPago = id;
                rp.Modificar(p);

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int id) {
            var resultado = rp.ObtenerPorId(id);
            return View();
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                rp.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }
    }
}