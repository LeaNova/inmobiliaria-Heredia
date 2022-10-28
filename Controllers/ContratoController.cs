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
        private IRepositorioPago rp;
        private IRepositorioInmueble rInm;
        private IRepositorioInquilino rInq;

        public ContratoController(IRepositorioContrato rc, IRepositorioPago rp, IRepositorioInmueble rInm, IRepositorioInquilino rInq) {
            this.rc = rc;
            this.rp = rp;
            this.rInm = rInm;
            this.rInq = rInq;
        }

        // GET: Contrato
        [Authorize]
        public ActionResult Index() {
            var lista = rc.ObtenerTodos();

            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }

            return View(lista);
        }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Details(int id) {
            var resultado = rc.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Contrato/Create
        [Authorize]
        public ActionResult Create(int id) {

            Contrato c = new Contrato();
            c.fechaInicio = DateTime.Now;
            c.fechaFinal = DateTime.Now;
            if(id > 0) {
                var resultado = rc.ObtenerPorId(id);
                c.inmuebleId = resultado.inmuebleId;
                c.inquilinoId = resultado.inquilinoId;
                c.fechaInicio = resultado.fechaFinal;
                c.fechaFinal = resultado.fechaFinal;
                c.alquilerMensual = resultado.alquilerMensual;
            }
            ViewBag.Inmueble = rInm.ObtenerTodos();
            ViewBag.Inquilino = rInq.ObtenerTodos();

            return View(c);
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato c) {
            try {
                if(ModelState.IsValid) {
                    if(c.fechaFinal > c.fechaInicio) {
                        var lista = rc.ObtenerPorInmueble(c.inmuebleId);

                        foreach(var item in lista) {
                            if(c.fechaInicio < item.fechaFinal) {
                                TempData["Error"] = "Las fechas se superpone con un contrato existente";
                                if(TempData.ContainsKey("Error")) {
                                    ViewBag.Error = TempData["Error"];
                                }

                                ViewBag.Inmueble = rInm.ObtenerTodos();
                                ViewBag.Inquilino = rInq.ObtenerTodos();
                                return View(c);
                            }
                        }

                        var inmueble = rInm.ObtenerPorId(c.inmuebleId);
                        c.alquilerMensual = inmueble.precio;

                        rc.Alta(c);
                        TempData["Mensaje"] = "Contrato creado correctamente";
                        return RedirectToAction(nameof(Index));
                    } else {
                        TempData["Error"] = "Formato de fechas invalidas";
                        if(TempData.ContainsKey("Error")) {
                            ViewBag.Error = TempData["Error"];
                        }

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
                if(c.fechaFinal > c.fechaInicio) {
                    var lista = rc.ObtenerPorInmueble(c.inmuebleId);

                    foreach(var item in lista) {
                        if(c.fechaInicio < item.fechaFinal) {
                            TempData["Error"] = "Las fechas se superpone con un contrato existente";
                            if(TempData.ContainsKey("Error")) {
                                ViewBag.Error = TempData["Error"];
                            }

                            ViewBag.Inmueble = rInm.ObtenerTodos();
                            ViewBag.Inquilino = rInq.ObtenerTodos();
                            return View(c);
                        }
                    }

                    c.idContrato = id;
                    rc.Modificar(c);
                    TempData["Mensaje"] = "Contrato Actualizado correctamente";
                    return RedirectToAction(nameof(Index));
                } else {

                    TempData["Error"] = "Formato de fechas invalidas";
                    if(TempData.ContainsKey("Error")) {
                        ViewBag.Error = TempData["Error"];
                    }

                    ViewBag.Inmueble = rInm.ObtenerTodos();
                    ViewBag.Inquilino = rInq.ObtenerTodos();
                    return View(c);
                }

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
                TempData["Mensaje"] = "Contrato borrado correctamente";
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                
                TempData["Error"] = "No se puede borrar un contrato con un pago efectuado";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contrato-por-inmueble
        [Authorize]
        public ActionResult BuscarPorInmueble(Contrato c) {
            var lista = rc.ObtenerTodos();
            IList<Contrato> listaC = new List<Contrato>();
            ViewBag.Inmueble = rInm.ObtenerTodos();

            foreach(var item in lista) {
                if(item.inmuebleId == c.inmuebleId) {
                    listaC.Add(item);
                }
            }
            return View("Index", listaC);
        }

         // GET: Pago/Details/5
        [Authorize]
        public ActionResult Terminate(int id) {
            Contrato terminado = rc.ObtenerPorId(id);

            //Calculo de dias
            var minimoDias = (terminado.fechaFinal - terminado.fechaInicio)/2;
            var diasTranscurridos = DateTime.Now - terminado.fechaInicio;

            Pago p = new Pago();
            p.importe = terminado.alquilerMensual;
            p.detalle = "Multa: importe de 1 (un) meses de alquiler";

            if(diasTranscurridos < minimoDias) {
                p.importe *= 2;
                p.detalle = "Multa: importe de 2 (dos) meses de alquiler";
            }

            ViewBag.Pago = p;
            return View("Details", terminado);
        }

        // POST: Contrato/Terminate/5
        [Authorize]
        public ActionResult TerminateCreate(int id) {
            Contrato terminado = rc.ObtenerPorId(id);

            //Calculo de dias
            var minimoDias = (terminado.fechaFinal - terminado.fechaInicio)/2;
            var diasTranscurridos = DateTime.Now - terminado.fechaInicio;

            terminado.fechaFinal = DateTime.Now;
            rc.Modificar(terminado);

            //Armo nuevo pago
            Pago p = new Pago();
            p.fechaPago = DateTime.Now;
            p.importe = terminado.alquilerMensual;
            p.contratoId = terminado.idContrato;
            p.detalle = "Multa: importe de 1 (un) meses de alquiler";

            if(diasTranscurridos < minimoDias) {
                p.importe *= 2;
                p.detalle = "Multa: importe de 2 (dos) meses de alquiler";
            }

            rp.Alta(p);
            TempData["Mensaje"] = "Contrato terminado correctamente";
            return RedirectToAction(nameof(Index), "Contrato");
        }

        
        // GET: Contrato
        [Authorize]
        public ActionResult Search() {
            ViewBag.Inmueble = rInm.ObtenerTodos();

            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }

            return View();
        }

        // GET: Contrato-Por Fechas
        [Authorize]
        public ActionResult BuscarPorFechas(Contrato c) {
            try {
                if(c.fechaFinal > c.fechaInicio) {
                    var lista = rc.ObtenerPorFechas(c.fechaInicio, c.fechaFinal);
                    ViewBag.Inmueble = rInm.ObtenerTodos();

                    return View("Index", lista);
                } else {
                    TempData["Error"] = "Formato de fechas incorrecto";

                    return RedirectToAction(nameof(Search));
                }
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Contrato-Por Fechas
        [Authorize]
        public ActionResult InmueblesPorFechas(Contrato c) {
            try {
                if(c.fechaFinal > c.fechaInicio) {
                    var lista = rc.ObtenerInmueblesDisponibles(c.fechaInicio, c.fechaFinal);
                    ViewBag.Inmueble = rInm.ObtenerTodos();
                    ViewBag.Lista = lista;

                    return View("Search");
                } else {
                    TempData["Error"] = "Formato de fechas incorrecto";

                    return RedirectToAction(nameof(Search));
                }
            } catch (Exception ex) {
                throw;
            }
        }
    }
}