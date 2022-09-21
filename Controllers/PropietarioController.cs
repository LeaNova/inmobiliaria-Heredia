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
                    rp.Alta(p);
                    return RedirectToAction(nameof(Index));
                } else {
                    return View(p);
                }
            } catch (Exception ex) {
                throw;
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
                p.idPropietario = id;
                rp.Modificar(p);

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
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