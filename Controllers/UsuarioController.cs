using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inmueble_Heredia.Controllers {
    public class UsuarioController : Controller {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private IRepositorioUsuario ru;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioUsuario ru) {
            this.configuration = configuration;
            this.environment = environment;
            this.ru = ru;
        }

        // GET: Usuario
        [Authorize(Policy = "Administrador")]
        public ActionResult Index() {
            var lista = ru.ObtenerTodos();
            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id) {
            if(TempData.ContainsKey("Mensaje")) {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")) {
                ViewBag.Error = TempData["Error"];
            }

            var resultado = ru.ObtenerPorId(id);
            return View(resultado);
        }

        // GET: Usuario/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create() {
            ViewBag.Access = Usuario.ObtenerAccess();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u) {
            if(!ModelState.IsValid) {
                ViewBag.Access = Usuario.ObtenerAccess();
                return View();
            }
            try {
                if(validarDatos(u)) {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.pass,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    u.pass = hashed;
                    int resultado = ru.Alta(u);

                    if(u.avatarFile != null && u.idUsuario > 0) {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");

                        if(!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                        }

                        string fileName = "avatar_" + u.idUsuario + Path.GetExtension(u.avatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        u.avatar = Path.Combine("/Uploads", fileName);
                        
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)) {
                            u.avatarFile.CopyTo(stream);
                        }

                        ru.Modificar(u);
                    }

                    TempData["Mensaje"] = "Usuario cargado correctamente";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error en entrada de datos";
                ViewBag.Access = Usuario.ObtenerAccess();
                if(TempData.ContainsKey("Error")) {
                    ViewBag.Error = TempData["Error"];
                }
                return View(u);
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int id) {
            ViewBag.id = id;

            var resultado = ru.ObtenerPorId(id);
            ViewBag.Access = Usuario.ObtenerAccess();
            return View(resultado);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u) {
            try {
                if(validarDatos(u)) {
                    Usuario original = ru.ObtenerPorId(id);
                    original.nombre = u.nombre;
                    original.apellido = u.apellido;

                    if(User.IsInRole("Administrador")) {
                        original.access = u.access;
                    }

                    ru.Modificar(original);
                    TempData["Mensaje"] = "Usuario actualizado correctamente";

                    if(User.IsInRole("Empleado")) {
                        return View("Index", "Propietario");
                    }
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error en entrada de datos";
                ViewBag.Access = Usuario.ObtenerAccess();
                if(TempData.ContainsKey("Error")) {
                    ViewBag.Error = TempData["Error"];
                }
                return View(u);
            } catch (Exception ex) {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditPass(int id, PassModel p) {
            try {
                Usuario original = ru.ObtenerPorId(id);
                if(ModelState.IsValid) {
                    var passOld = p.passOld;
                    var passNew = p.passNew;
                    string hashedOld = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: passOld,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    if(original.pass == hashedOld) {
                        if(passNew == p.pass) {
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: p.pass,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));

                            original.pass = hashed;
                            TempData["Mensaje"] = "Contraseña actualizada correctamente";
                            ru.Modificar(original);
                        } else {
                            TempData["Error"] = "La nueva contraseña no coinciden";
                            return View("Edit", original);
                        }
                    } else {
                        TempData["Error"] = "Contraseña incorrecta";
                        return View("Edit", original);
                    }
                    
                    if(User.IsInRole("Empleado")) {
                        return View("Index", "Propietario");
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Access = Usuario.ObtenerAccess();
                return View("Edit", original);
            } catch (Exception ex) {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditAvatar(int id, CambiarAvatar c) {
            try {
                Usuario original = ru.ObtenerPorId(id);
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");

                    if(!Directory.Exists(path)) {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = "avatar_" + original.idUsuario + Path.GetExtension(c.avatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);

                    if(System.IO.File.Exists(Path.Combine(environment.WebRootPath, "Uploads", "avatar_" + id + Path.GetExtension(original.avatar))))
						System.IO.File.Delete(Path.Combine(environment.WebRootPath, "Uploads", "avatar_" + id + Path.GetExtension(original.avatar)));

                    original.avatar = Path.Combine("/Uploads", fileName);
                    
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)) {
                        c.avatarFile.CopyTo(stream);
                    }
                
                TempData["Mensaje"] = "Avatar actualizado correctamente";
                ru.Modificar(original);

                if(User.IsInRole("Empleado")) {
                    return View("Index", "Propietario");
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            var resultado = ru.ObtenerPorId(id);
            return View(resultado);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                var resultado = ru.ObtenerPorId(id);
                ru.Baja(resultado.idUsuario);
                
                if(System.IO.File.Exists(Path.Combine(environment.WebRootPath, "Uploads", "avatar_" + id + Path.GetExtension(resultado.avatar))))
                    System.IO.File.Delete(Path.Combine(environment.WebRootPath, "Uploads", "avatar_" + id + Path.GetExtension(resultado.avatar)));

                TempData["Mensaje"] = "Usuario eliminado correctamente";
                return RedirectToAction(nameof(Index));
            } catch {
                throw;
            }
        }

        // GET: Usuarios/Login/
        [AllowAnonymous]
        public ActionResult Login() {
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView u) {
            if(!ModelState.IsValid) {
                return View();
            }
            try {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: u.pass,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                
                var uBase = ru.ObtenerPorMail(u.user);
                if(uBase == null || uBase.pass != hashed) {
                    ModelState.AddModelError("", "Usuario o Contraseña incorrecto");
                    return View();
                }

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, uBase.user),
                    new Claim("FullName", uBase.apellido + ", " + uBase.nombre),
                    new Claim(ClaimTypes.Role, uBase.AccessNombre)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect("/Home");

            } catch (Exception ex) {
                throw;
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout() {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Perfil() {
            var resultado = ru.ObtenerPorMail(User.Identity.Name);
            return View("Details", resultado);
        }

        public bool validarDatos(Usuario u) {
            if(u.nombre.Any(char.IsDigit)) {
                return false;
            }
            if(u.apellido.Any(char.IsDigit)) {
                return false;
            }
            return true;
        }

    }
}