using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using inmobiliaria_Heredia.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace inmobiliaria_Heredia.Api;

	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
	public class PagoController : ControllerBase {

		private readonly DataContext context;
		private readonly IConfiguration configuration;

		public PagoController(DataContext context, IConfiguration configuration) {
			this.context = context;
			this.configuration = configuration;
		}

		// GET: <controller>
		[HttpGet("{id}")]
		public async Task<ActionResult<Pago>> Get(int id) {
			try {
				string usuario = User.Identity.Name;
				int usuarioId = Int32.Parse(User.Claims.First(x => x.Type == "id").Value);

                var pagos = context.Pago
                    .Include(x => x.contrato)
                    .Where(x => x.contratoId == id)
                    .Include(x => x.contrato.propiedad)
                    .Where(x => x.contrato.propiedad.propietarioId == usuarioId);

				return Ok(pagos);

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
