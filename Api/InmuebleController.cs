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

		public InmuebleController(DataContext context, IConfiguration configuration) {
			this.context = context;
			this.configuration = configuration;
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

				Inmueble i = context.Inmueble.Include(x => x.duenio).Where(x => x.duenio.Email == usuario).SingleOrDefault(x => x.idInmueble == id);

				return Ok(i);
				
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// POST <controller>/actualizar/estado/5"
		[HttpPost("actualizar/estado/{id}")]
		public async Task<IActionResult> actualizarEstado([FromForm] Inmueble i, int id) {
			try {
				if(ModelState.IsValid) {
					if(isValid(i)) {
						string usuario = User.Identity.Name;
						int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);

						Inmueble original = context.Inmueble.Include(x => x.duenio).Where(x => x.duenio.Email == usuario).SingleOrDefault(x => x.idInmueble == id);

						original.disponible = i.disponible;

						context.Inmueble.Update(original);
						await context.SaveChangesAsync();

						return Ok(original);
					}
					return BadRequest("Ingrese los datos correctamente");
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// POST <controller>/actualizar/informacion/5"
		[HttpPost("actualizar/informacion/{id}")]
		public async Task<IActionResult> actualizarInformacion([FromForm] Inmueble i, int id) {
			try {
				if(ModelState.IsValid) {
					if(isValid(i)) {
						string usuario = User.Identity.Name;
						int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);

						Inmueble original = context.Inmueble.AsNoTracking().Include(x => x.duenio).Where(x => x.duenio.Email == usuario).SingleOrDefault(x => x.idInmueble == id);

						i.idInmueble = original.idInmueble;
						i.disponible = original.disponible;

						context.Inmueble.Update(i);
						await context.SaveChangesAsync();

						return Ok(i);
					}
					return BadRequest("Ingrese los datos correctamente");
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// POST <controller>/actualizar/estado/5"
		[HttpPost("agregar")]
		public async Task<IActionResult> agregar([FromForm] Inmueble i) {
			try {
				if(ModelState.IsValid) {
					if(isValid(i)) {

						
					}
					return BadRequest("Ingrese los datos correctamente");
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		public bool isValid(Inmueble i) {
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
