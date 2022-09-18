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
            return View(lista);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id) {
            return View();
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
                return RedirectToAction(nameof(Index));

            } catch (Exception ex) {
                throw;
            }
        }

        // GET: Usuario/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id) {
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
                Usuario original = ru.ObtenerPorId(id);
                if(u.nombre != null && u.apellido != null) {
                    u.user = original.user;
                    u.pass = original.pass;
                    u.avatar = original.avatar;
                } else if(u.user != null && u.pass != null) {
                    u.nombre = original.nombre;
                    u.apellido = original.apellido;
                    u.avatar = original.avatar;
                    u.access = original.access;

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.pass,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    u.pass = hashed;

                } else {
                    u.nombre = original.nombre;
                    u.apellido = original.apellido;
                    u.user = original.user;
                    u.pass = original.pass;
                    u.access = original.access;

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
                }
                u.idUsuario = id;
                ru.Modificar(u);

                return RedirectToAction(nameof(Index));
            } catch (Exception ex) {
                ViewBag.Access = Usuario.ObtenerAccess();
                return View(u);
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {

                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: Usuarios/Login/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
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
    }
}