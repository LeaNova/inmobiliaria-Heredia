using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using inmobiliaria_Heredia.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

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

		// POST <controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginView loginView) {
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
						new Claim("nombreCompleto", p.apellido + " " + p.nombre),
						new Claim(ClaimTypes.Role, "Propietario")
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
						if(contraseña.passNew == contraseña.passNewR) {
							original.pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: contraseña.passNew,
								salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));

							context.Propietario.Update(original);
							await context.SaveChangesAsync();

							return Ok(new PropietarioView(original));
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

		[HttpGet("token")]
		public async Task<IActionResult> token() {
			try {
				var perfil = new {
					Email = User.Identity.Name,
					Nombre = User.Claims.First(x => x.Type == "nombreCompleto").Value
				};

				Random random = new Random(Environment.TickCount);
				string rdmChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
				string newPass = "";

				for(int i = 0; i < 8; i++) {
					newPass += rdmChars[random.Next(0, rdmChars.Length)];
				}

				string rdmPass = newPass;

				newPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: newPass,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				Propietario p = await context.Propietario.AsNoTracking().FirstOrDefaultAsync(x => x.Email == perfil.Email);
				p.pass = newPass;

				context.Propietario.Update(p);
				await context.SaveChangesAsync();

				var message = new MimeKit.MimeMessage();;
				message.To.Add(new MailboxAddress(perfil.Nombre, "leandro.heredia.96@gmail.com"));
				message.From.Add(new MailboxAddress("Sistema", configuration["SMTPUser"]));
				message.Subject = "Reestablecer contraseña";
				message.Body = new TextPart("html") {
					Text = @$"<h3>Hola {perfil.Nombre}</h3>
                    <p>Gracias por confirmar su restablecimiento de contraseña</p>
                    <p>Su nueva contraseña es: {rdmPass}></p>
                    <p>Le reiteramos que una vez entrado a su cuenta cambie la contraseña por una nueva que solo usted conozca.</p>"
				};
				message.Headers.Add("Encabezado", "Valor");

				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) => { return true; };
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(configuration["SMTPUser"], configuration["SMTPPass"]);
				await client.SendAsync(message);

				return Ok(perfil);

			} catch (Exception e) {
				return BadRequest(e.Message);
			}
		}

		[HttpPost("pass_change")]
		[AllowAnonymous]
		public async Task<IActionResult> passChange([FromForm] string email) {
			try {
				Propietario p = await context.Propietario.FirstOrDefaultAsync(x => x.Email == email);
				
				if(p != null) {
					PropietarioView perfil = new PropietarioView(p);

					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
					
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

					var claims = new List<Claim> {
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("id", p.idPropietario.ToString()),
						new Claim("nombreCompleto", p.apellido + " " + p.nombre),
						new Claim(ClaimTypes.Role, "Propietario")
					};

					var token = new JwtSecurityToken(
						issuer: configuration["TokenAuthentication:Issuer"],
						audience: configuration["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);

					var wToken = new JwtSecurityTokenHandler().WriteToken(token);

					var url = "http://192.168.0.17:5000/Propietario/token?access_token=" + wToken;

					var message = new MimeKit.MimeMessage();
					message.To.Add(new MailboxAddress(perfil.nombre, "leandro.heredia.96@gmail.com"));
					message.From.Add(new MailboxAddress("Sistema", configuration["SMTPUser"]));
					message.Subject = "Reestablecer contraseña";
					message.Body = new TextPart("html") {
						Text = @$"<h3>Hola {perfil.apellido + ' ' + perfil.nombre}</h3>
                        <p>Hemos recibido una petición para restablecer su contraseña</p>
                        <p>Ingresa en el siguiente <a href={url}>enlace</a> para confirmar el restablecimiento de su contraseña por una nueva. Una vez confirmada la petición se le enviara un nuevo correo con su nueva clase, le pedimos que una vez entrado a su perfil cambie a una contraseña nueva.</p>
                        <p>Si no ha sido usted por favor desestime este mensaje.</p>"
					};
					message.Headers.Add("Encabezado", "Valor");

					MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
					client.ServerCertificateValidationCallback = (object sender,
						System.Security.Cryptography.X509Certificates.X509Certificate certificate,
						System.Security.Cryptography.X509Certificates.X509Chain chain,
						System.Net.Security.SslPolicyErrors sslPolicyErrors) => { return true; };
					client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
					client.Authenticate(configuration["SMTPUser"], configuration["SMTPPass"]);
					await client.SendAsync(message);

					return p != null ? Ok(p) : NotFound();
				}
				return BadRequest("No se encontro usuario");
			} catch (Exception e) {
				return BadRequest(e.Message);
			}
		}

		public bool isValid(PerfilView pView) {
			if(pView.nombre.Any(char.IsDigit) || pView.nombre == "") {
				return false;
			}
			if(pView.apellido.Any(char.IsDigit) || pView.apellido == "") {
				return false;
			}
			if(pView.DNI.Any(char.IsLetter) || pView.DNI == "") {
				return false;
			}
			if(pView.telefono.Any(char.IsLetter) || pView.telefono == "") {
				return false;
			}
			if(!(pView.Email.Contains("@mail.com") || pView.Email.Contains("@gmail.com")) || pView.Email == "") {
				return false;
			}
			return true;
		}
	}
