using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {
    public class InquilinoController : Controller {
        
        private IRepositorioInquilino ri;

        public InquilinoController(IRepositorioInquilino ri) {
            this.ri = ri;
        }

        // GET: Inquilino
        public ActionResult Index() {
            var lista = ri.ObtenerTodos();
            return View(lista);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Inquilino/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i) {
            try {
                if(ModelState.IsValid) {
                    ri.Alta(i);
                    return RedirectToAction(nameof(Index)); 
                } else {
                    return View(i);
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i) {
            try {
                i.idInquilino = id;
                ri.Modificar(i);

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id) {
            var resultado = ri.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino i) {
            try {
                ri.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }
    }
}