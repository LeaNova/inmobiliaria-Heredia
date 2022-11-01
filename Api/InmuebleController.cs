using System.Security.Claims;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace inmobiliaria_Heredia.Api;

	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
	public class InmuebleController : ControllerBase {

		private readonly DataContext context;
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment environment;

		public InmuebleController(DataContext context, IConfiguration configuration, IWebHostEnvironment environment) {
			this.context = context;
			this.configuration = configuration;
			this.environment = environment;
		}

		// POST <controller>/agregar"
		[HttpPost("agregar")]
		public async Task<IActionResult> agregar([FromForm] Inmueble i) {
			int propietarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);
			try {
				if(ModelState.IsValid) {
					if(isValid(i)) {
						i.disponible = true;
						i.propietarioId = propietarioId;
						i.precio = i.precio/10;
						
						if(i.foto != "") {
							MemoryStream stream1 = new MemoryStream(Convert.FromBase64String(i.foto));
							IFormFile imgIF = new FormFile(stream1, 0, stream1.Length, "IFoto", ".png");

							string wwwPath = environment.WebRootPath;
							string path = Path.Combine(wwwPath, "Uploads");
							
							if(!Directory.Exists(path)) {
								Directory.CreateDirectory(path);
							}

							Random r = new Random();
							string fileName = "inmueble_" + propietarioId + r.Next(0, 1000000) + Path.GetExtension(imgIF.FileName);
							string pathCompleto = Path.Combine(path, fileName);
							i.foto = Path.Combine("/Uploads", fileName);
							
							using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)) {
								imgIF.CopyTo(stream);
							}
						}
						context.Inmueble.Add(i);
						context.SaveChanges();
						//return Ok(i);
						return CreatedAtAction(nameof(obtener), new { id = i.idInmueble }, i);
					}
					return BadRequest("Ingrese los datos correctamente");
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// POST <controller>/actualizar/estado/5"
		[HttpPost("actualizar/estado/{id}")]
		public async Task<IActionResult> actualizarEstado(int id) {
			try {
				if(ModelState.IsValid) {
					string usuario = User.Identity.Name;
					int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);

					Inmueble original = context.Inmueble
						.Include(x => x.duenio)
						.Where(x => x.duenio.Email == usuario)
						.SingleOrDefault(x => x.idInmueble == id);

					if(original.disponible) {
						original.disponible = false;
					} else {
						original.disponible = true;
					}

					context.Inmueble.Update(original);
					await context.SaveChangesAsync();

					return Ok(original);
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// GET: <controller>
		[HttpGet]
		public async Task<ActionResult<Inmueble>> obtener() {
			try {
				string usuario = User.Identity.Name;
				return Ok(context.Inmueble.Include(x => x.duenio).Where(x => x.duenio.Email == usuario));

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
		
		// GET: <controller>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Inmueble>> obtener(int id) {
			try {
				string usuario = User.Identity.Name;
				int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);

				Inmueble i = context.Inmueble
					.Include(x => x.duenio)
					.Where(x => x.duenio.Email == usuario)
					.SingleOrDefault(x => x.idInmueble == id);

				return Ok(i);
				
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		public bool isValid(Inmueble i) {
			if(i.direccion == "") {
				return false;
			}
			if(i.uso < 1) {
				return false;
			}
			if(i.tipo < 1) {
				return false;
			}
			if(i.cantAmbientes < 1) {
				return false;
			}
			if(!i.coordenadas.Contains(",")) {
				return false;
			}
			if(i.precio < 30000) {
				return false;
			}
			return true;
		}
	}
