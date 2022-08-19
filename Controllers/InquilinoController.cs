using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Heredia.Controllers {

    public class InquilinoController : Controller {
        
        private RepositorioInquilino ri = new RepositorioInquilino();

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
                // TODO: Add insert logic here
                if(ModelState.IsValid) {
                    ri.Alta(i);
                    return RedirectToAction(nameof(Index)); 

                } else {
                    return View(i);
                }
            } catch {
                return View();
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
        public ActionResult Edit(int id, IFormCollection collection) {
            Inquilino i = null;
            try {
                // TODO: Add update logic here
                i = ri.ObtenerPorId(id);

                i.nombre = collection["nombre"];
                i.apellido = collection["apellido"];
                i.DNI = collection["DNI"];
                i.telefono = collection["telefono"];
                i.Email = collection["Email"];

                ri.Modificar(i);
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
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
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                // TODO: Add delete logic here
                ri.Baja(id);
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
    }
}