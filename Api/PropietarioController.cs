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
	public class PropietarioController : ControllerBase {

		private readonly DataContext context;
		private readonly IConfiguration configuration;

		public PropietarioController(DataContext context, IConfiguration configuration) {
			this.context = context;
			this.configuration = configuration;
		}

		// GET: <controller>
		[HttpGet]
		public async Task<ActionResult<Propietario>> Get() {
			try {
				string usuario = User.Identity.Name;
				Propietario p = await context.Propietario.SingleOrDefaultAsync(x => x.Email == usuario);

				PropietarioView pView = new PropietarioView(p);

				return Ok(pView);

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/*
		// GET: api/<controller>/GetAll
		[HttpGet("GetAll")]
		public async Task<ActionResult> GetAll() {
			try {
				return Ok(await context.Propietario.ToListAsync());

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// GET: api/<controller>/5
		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id) {
			try {
				return Ok(context.Propietario.Find(id));

			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
		*/

		// POST <controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView) {
			try {
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.pass,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				Propietario p = await context.Propietario.FirstOrDefaultAsync(x => x.Email == loginView.user);

				if (p == null || p.pass != hashed) {
					return BadRequest("Usuario o contraseña incorrecta");

				} else {
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

					var claims = new List<Claim> {
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("id", p.idPropietario.ToString()),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: configuration["TokenAuthentication:Issuer"],
						audience: configuration["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		// POST <controller>/actualizar/perfil
		[HttpPost("actualizar/perfil")]
		public async Task<IActionResult> actualizarPerfil([FromForm] PerfilView perfil) {
			try {
				if(ModelState.IsValid) {
					if(isValid(perfil)) {
						string usuario = User.Identity.Name;
						Propietario original = await context.Propietario.AsNoTracking().SingleOrDefaultAsync(x => x.Email == usuario);

						original.nombre = perfil.nombre;
						original.apellido = perfil.apellido;
						original.DNI = perfil.DNI;
						original.telefono = perfil.telefono;
						original.Email = perfil.Email;

						var key = new SymmetricSecurityKey(
							System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
						var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

						var claims = new List<Claim> {
							new Claim(ClaimTypes.Name, original.Email),
							new Claim("id", original.idPropietario.ToString()),
							new Claim(ClaimTypes.Role, "Propietario"),
						};

						var token = new JwtSecurityToken(
							issuer: configuration["TokenAuthentication:Issuer"],
							audience: configuration["TokenAuthentication:Audience"],
							claims: claims,
							expires: DateTime.Now.AddMinutes(60),
							signingCredentials: credenciales
						);

						context.Propietario.Update(original);
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
		
		// POST <controller>/actualizar/contraseña
		[HttpPost("actualizar/contraseña")]
		public async Task<IActionResult> actualizarContraseña([FromForm] PassView contraseña) {
			try {
				if(ModelState.IsValid) {
					string usuario = User.Identity.Name;
					Propietario original = await context.Propietario.AsNoTracking().SingleOrDefaultAsync(x => x.Email == usuario);
					
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: contraseña.passOld,
						salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));
					
					if(original.pass == hashed) {
						if(contraseña.passNew == contraseña.pass) {
							original.pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: contraseña.passNew,
								salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));

							context.Propietario.Update(original);
							await context.SaveChangesAsync();

							return Ok();
						}
						return BadRequest("Las contraseñas no coinciden");
					}
					return BadRequest("Contraseña incorrecta");
				}
				return BadRequest();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		public bool isValid(PerfilView pView) {
			if(pView.nombre.Any(char.IsDigit)) {
				return false;
			}
			if(pView.apellido.Any(char.IsDigit)) {
				return false;
			}
			if(pView.DNI.Any(char.IsLetter)) {
				return false;
			}
			if(pView.telefono.Any(char.IsLetter)) {
				return false;
			}
			if(!(pView.Email.Contains("@mail.com") || pView.Email.Contains("@gmail.com"))) {
				return false;
			}
			return true;
		}
	}
