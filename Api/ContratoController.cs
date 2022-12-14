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
	public class ContratoController : ControllerBase {

		private readonly DataContext context;
		private readonly IConfiguration configuration;

		public ContratoController(DataContext context, IConfiguration configuration) {
			this.context = context;
			this.configuration = configuration;
		}

		// GET: <controller>
		[HttpGet]
		public async Task<ActionResult<Contrato>> obtener() {
			try {
				int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);
				var contratos = context.Contrato
					.Include(x => x.propiedad)
					.Include(x => x.inquilino)
					.Where(x => x.propiedad.propietarioId == usuarioId);
				
				return Ok(contratos);

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// GET: <controller>/id
		[HttpGet("{id}")]
		public async Task<ActionResult<Contrato>> obtener(int id) {
			try {
				var contrato = context.Contrato.Include(x => x.propiedad).Where(x => x.propiedad.idInmueble == id);
				return Ok(contrato);
				
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
